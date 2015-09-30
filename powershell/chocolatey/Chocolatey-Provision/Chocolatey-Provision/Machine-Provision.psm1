<#
	
#>

function Invoke-ProvisionRestore {
  Param (
		[Parameter(Mandatory=$false, Position=0)]
		[Alias("f")]
		[string] $FilePath,
		[Parameter(Mandatory=$false, Position=1)]
		[Alias("g")]
		[bool] $gui = $false,
		[Parameter(Mandatory=$false, Position=2)]
		[Alias("v")]
		[bool] $specificVersion = $true
  );

	Check-RunAsAdministrator;
	$configFile = (Get-Item -Path $FilePath);
	[xml]$config = (Get-Content $configFile.FullName -Raw);

	$packagesList = $config.DocumentElement.SelectNodes("package") | where { $_.id -ne "Chocolatey"; };
	if($gui -eq $true) {
		$packagesList | Out-GridView -PassThru -Title "Select Packages" | foreach { 
			#Invoke-ProvisionInstall -Name $_.id -Version $_.version -Source $_.source;
			"Invoke-ProvisionInstall: -Name " + $_.id + " -Version " + $_.version + " -Source " + $_.source;
		}
	} else {
		$packagesList | foreach { 
			#Invoke-ProvisionInstall -Name $_.id -Version $_.version -Source $_.source;
			"Invoke-ProvisionInstall: -Name " + $_.id + " -Version " + $_.version + " -Source " + $_.source;
		}
	}
}

function Invoke-ProvisionInstall {
  Param (
    [Parameter(Mandatory=$true, Position=0)]
	  [Alias("n")]
    [string] $Name,
    [Parameter(Mandatory=$false, Position=1)]
	  [Alias("v")]
    [string] $Version = $null,
    [Parameter(Mandatory=$false, Position=2)]
	  [Alias("s")]
    [string] $Source = "install"
  );
  Check-RunAsAdministrator;
	$cho = "$env:ProgramData\chocolatey\choco.exe";
  if(!(Test-Path -Path $cho) ) {
      throw [System.ArgumentException] "Unable to locate chocolatey.";
  }
  if($Version -ne $null -and $Source -ieq "install") {
    & $cho install $Name -version $Version -y;
  } else {
    & $cho $Source $Name -y;
  }
}


function Get-AvailableWindowsFeatures {
	Check-RunAsAdministrator;
	$items = [System.Collections.ArrayList]@();
	$dism = @("$env:SystemRoot\sysnative\dism.exe", "$env:SystemRoot\system32\dism.exe", "dism.exe") | where { Test-Path -Path $_ } | Select-Object -First 1;
	if($dism -eq $null) {
		throw [System.ArgumentException] "Unable to locate DISM.exe";
	}
	$curItem = [PSCustomObject]@{
		ID = ""
		Enabled = $false
		Category = "Windows"
		Source = "windowsFeature"
	};
	Write-Host "Gathering Available System Features...";
	& $dism /online /Get-Features | where { $_ -match "^(Feature\sName|^State)\s\:\s(.*?)`$" } | foreach {
		switch($matches[1]) {
			"Feature Name" {
				$curItem = [PSCustomObject]@{
					ID = $matches[2]
					Enabled = $false
					Category = "Windows"
					Source = "windowsFeature"
				};
			}
			"State" {
				if($matches[2] -eq "Disabled") {
					$curItem.Enabled = $false;
				} elseif ($matches[2] -eq "Enabled") {
					$curItem.Enabled = $true;
				} else {
					throw [System.ArgumentException] "Unknown status for the state";
				}
				$items.Add($curItem) | Out-Null;
			}
		}
	};
	return $items | sort { !$_.Enabled, $_.ID };
}


function Get-AvailableWebPlatformInstallPackages {
		Check-RunAsAdministrator;
		$items = [System.Collections.ArrayList]@();
		$wpi = @("$env:ProgramFiles\microsoft\web platform installer\webpicmd.exe", "${env:ProgramFiles(x86)}\microsoft\web platform installer\webpicmd.exe", "webpicmd.exe") | where { Test-Path -Path $_ } | Select-Object -First 1;

		if($wpi -eq $null) {
			throw [System.ArgumentException] "Unable to locate webpicmd.exe";
		}
    
		$currentCategory = $null;
		$curItem = [PSCustomObject]@{
			Name = ""
			ID = ""
			Category = $currentCategory
			Source = "webpi"
		}
    Write-Host "Gathering Available Web Platform Features...";
    Invoke-Command { & $wpi /List /ListOption:All } | where { $_ -ne "" -and $_ -ne $null -and ( $_ -imatch "^(--)[a-z0-9]" -or $_ -imatch "^[a-z0-9]") -and ($_ -inotmatch "^ID\s+Title`$") -and ( ($_ -inotmatch "^the\ssoftware\sthat\s") -and $_ -inotmatch "^successfully\sloaded\s" )} | foreach {
		if($matches[1] -eq "--") {
			# new category
			$currentCategory = $_.Substring(2);
		} else {
			if($_ -imatch "^([^\s]+)\s+(.*?)$" -and $currentCategory -ne $null ) {
				# We have a category, so we can get the product.
				$id = $matches[1];
				$name = $matches[2];
				$curItem = [PSCustomObject]@{
					Name = $name
					ID = $id
					Category = $currentCategory
					Source = "webpi"
				}
				$items.Add($curItem) | Out-Null;
			}
		}
  }

  return $items | sort { $_.Category, $_.Name };
}

function Get-InstalledChocolateyPackages {
	Check-RunAsAdministrator;
	$cho = "$env:ProgramData\chocolatey\choco.exe";
	if(!(Test-Path -Path $cho) ) {
		throw [System.ArgumentException] "Unable to locate chocolatey.";
	}
	$packages = [System.Collections.ArrayList]@();
	Write-Host "Gathering installed chocolatey packages..."
	& $cho list -lo | foreach {
		if($_ -inotmatch "^\d+\spackages\sinstalled\.$") {
			$id = Select-String -InputObject $_ -Pattern "^([\S]+)" -AllMatches | % { $_.Matches } | % { $_.Value };
			$ver = (Select-String -InputObject $_ -Pattern "\s([\S]+)\s?$" -AllMatches | % { $_.Matches } | % { $_.Value }) -replace "\s","";
			$packages.Add( [PSCustomObject]@{
				ID = $id;
				Version = $ver
				Category = "Chocolatey"
				Source = "Chocolatey"
			} ) | Out-Null;
		}
	}

  return $packages | sort { $_.ID };
}

function Get-AvailableChocolateyPackages {
	Check-RunAsAdministrator;
	$cho = "$env:ProgramData\chocolatey\choco.exe";
	if(!(Test-Path -Path $cho) ) {
		throw [System.ArgumentException] "Unable to locate chocolatey.";
	}
	$packages = [System.Collections.ArrayList]@();
	Write-Host "Gathering packages from Chocolatey repositories. This will take a little time..."
	& $cho list | foreach {
		if($_ -inotmatch "^\d+\spackages\sinstalled\.$") {
			$id = Select-String -InputObject $_ -Pattern "^([\S]+)" -AllMatches | % { $_.Matches } | % { $_.Value };
			$ver = (Select-String -InputObject $_ -Pattern "\s([\S]+)\s?$" -AllMatches | % { $_.Matches } | % { $_.Value }) -replace "\s","";
			$packages.Add( [PSCustomObject]@{
				ID = $id;
				Version = $ver
				Source = "Chocolatey"
				Category = "Chocolatey"
			} ) | Out-Null;
		}
	}

  return $packages | sort { $_.ID };
}

function Get-ProvisionPackageConfig {
  Check-RunAsAdministrator;
  $wpi = Get-AvailableWebPlatformInstallPackages | where { $_.Category -eq "Previously Installed Products" };
  $wf = Get-AvailableWindowsFeatures | where { $_.Enabled -eq $true };
  $choco = Get-InstalledChocolateyPackages;
  "<?xml version=`"1.0`"?>";
  "<packages>";
  (( $wpi | Select-Object ID, Source ) + ( $wf |  Select-Object ID, Source ) + ( $choco | Select-Object ID, Source, Version ) ) | sort { $_.Source, $_.ID } | foreach {
    $id = $_.ID;
		$version = $_.Version;
    # if the source is chocolatey, change to "install" because that is the command for choco.
    $source = @{$true="install";$false=$_.Source}[$_.Source -eq 'Chocolatey'];
		if( $version -eq "" -or $version -eq $null ) {
			"  <package id=`"$id`" source=`"$source`" />";
		} else {
			"  <package id=`"$id`" source=`"$source`" version=`"$version`" />";
		}
  };
  "</packages>";
}

function Out-ProvisionPackageConfig {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[Alias("f")]
		[string] $FilePath
	)
  Check-RunAsAdministrator;
	Get-ProvisionPackageConfig | Out-File -FilePath $FilePath -Force;
}

function Find-ProvisionPackage {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[Alias("s")]
		[string] $Search
	);

	$wpi = Get-AvailableWebPlatformInstallPackages | where { $_.ID -like "*$Search*" };
	$wf = Get-AvailableWindowsFeatures | where { $_.ID -like "*$Search*" };
	$choco = Get-AvailableChocolateyPackages | where { $_.ID -like "*$Search*" };

	(( $wpi | Select-Object ID, Source, Category ) + 
		( $wf |  Select-Object ID, Source, Category ) + 
		( $choco | Select-Object ID, Source, Version, Category ) ) | sort { $_.Source, $_.ID } 
}

function Check-RunAsAdministrator {
  if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    throw "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!";
		#$arguments = "& '" + $MyInvocation.MyCommand.Definition + "'";
		#Start-Process powershell -Verb runAs -ArgumentList $arguments
  }
}

Export-ModuleMember -Function Out-ProvisionPackageConfig -Alias cexport;
Export-ModuleMember -Function Invoke-ProvisionInstall;
Export-ModuleMember -Function Invoke-ProvisionRestore -Alias crestore;
Export-ModuleMember -Function Get-InstalledChocolateyPackages;
Export-ModuleMember -Function Get-AvailableWebPlatformInstallPackages;
Export-ModuleMember -Function Get-AvailableWindowsFeatures;
Export-ModuleMember -Function Get-AvailableChocolateyPackages;
Export-ModuleMember -Function Find-ProvisionPackage;
