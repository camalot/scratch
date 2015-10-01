<#
	
#>

function Invoke-InstallChocolatey {
	iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1')) | Write-Host;
}

function Get-ChocolateyExe {
	$cho = "$env:ProgramData\chocolatey\choco.exe";
	if(!(Test-Path -Path $cho) ) {
		Write-Host -BackgroundColor Red -ForegroundColor White "Unable to locate chocolatey. Installing chocolatey...";
		Invoke-InstallChocolatey;
		if(!(Test-Path -Path $cho) ) {
			throw [System.IO.FileNotFoundException] "Still unable to locate chocolatey, even after install attempt."
		}
	}
	return $cho;
}

function Get-DismExe {
	$dism = @("$env:SystemRoot\sysnative\dism.exe", "$env:SystemRoot\system32\dism.exe", "dism.exe") | where { Test-Path -Path $_ } | Select-Object -First 1;
	if($dism -eq $null) {
		throw [System.ArgumentException] "Unable to locate DISM.exe";
	}
	return $dism;
}

function Get-WebPiExe {
	$wpi = @("$env:ProgramFiles\microsoft\web platform installer\webpicmd.exe", "${env:ProgramFiles(x86)}\microsoft\web platform installer\webpicmd.exe", "webpicmd.exe");
	$wpiloc = $wpi | where { Test-Path -Path $_ } | Select-Object -First 1;
	if($wpiloc -eq $null) {
		Write-Host -BackgroundColor Red -ForegroundColor White "Unable to locate webpi. Installing webpi...";
		Invoke-ProvisionInstall -Name "webpi" -Source "Chocolatey";
		$wpiloc = $wpi | where { Test-Path -Path $_ } | Select-Object -First 1;
		if($wpiloc -eq $null) {
			throw [System.IO.FileNotFoundException] "Still unable to locate webpi, even after install attempt."
		}
	}
	return $wpi;
}

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
		[bool] $SpecificVersion = $false
  );

	Check-RunAsAdministrator;
	$configFile = (Get-Item -Path $FilePath);

	if(!(Test-Path -Path $configFile)) {
		throw [System.IO.FileNotFoundException] "Unable to locate config file: '$configFile'";
	}

	[xml]$config = (Get-Content $configFile.FullName);

	$packagesList = $config.DocumentElement.SelectNodes("package") | where { $_.id -ne "Chocolatey"; };
	if($gui -eq $true) {
		$packagesList | Out-GridView -PassThru -Title "Select Packages" | foreach { 
			try {
				$version = @{$true=$_.version;$false=""}[$SpecificVersion -eq $true];
				$id = $_.id;
				$source = $_.source;
				Invoke-ProvisionInstall -Name $id -Version $version -Source $_.source;
				Write-Host "Invoke-ProvisionInstall: -Name $id -Version $version -Source $source";
			} catch [System.Exception] {
				Write-Error -Exception $_.Exception;
			}
		}
	} else {
		$packagesList | foreach { 
			try {
				$version = @{$true=$_.version;$false=""}[$SpecificVersion -eq $true];
				$id = $_.id;
				$source = $_.source;
				Invoke-ProvisionInstall -Name $id -Version $version -Source $_.source;
				Write-Host "Invoke-ProvisionInstall: -Name $id -Version $version -Source $source";
			} catch [System.Exception] {
				Write-Error -Exception $_.Exception;
			}
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

	switch -regex ($Source) {
		"^([Cc]hocolatey|install)$" {
			$cho = Get-ChocolateyExe;
			if($Version -ne $null -and $Version -ne "" ) {
				& $cho install $Name -version $Version -y;
			} else {
				& $cho install $Name -y;
			}
		};
		"^webpi$" {
			$wpi = Get-WebPiExe;
			& $wpi /Install /Products:$Name;
		};
		"^windows[fF]eatures$" {
			$dism = Get-DismExe;
			& $dism /online /Enable-Feature /FeatureName:$Name /All;
		};
	}

}

function Invoke-ProvisionUninstall {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[Alias("n")]
		[string] $Name,
		[Parameter(Mandatory=$false, Position=1)]
		[Alias("s")]
		[string] $Source = "install"
	);

	Check-RunAsAdministrator;

	switch($Source) {
		"^([Cc]hocolatey|install)$" {
			$cho = Get-ChocolateyExe;
			& $cho uninstall $Name -version $Version -y;
		};
		"^webpi$" {
			throw "Uninstall from WebPI is not implemented.";
		};
		"^windows[fF]eatures$" {
			$dism = Get-DismExe;
			& $dism /online /Disable-Feature /FeatureName:$Name;
		};
	}
}

function Get-AvailableWindowsFeatures {
	Check-RunAsAdministrator;
	$items = [System.Collections.ArrayList]@();
	$dism = Get-DismExe;
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

function Find-WindowsFeatures {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[Alias("s")]
		[string] $Search
	);
	$items = Get-AvailableWindowsFeatures | where { $_.ID -like "*$Search*"; }
  return $items | sort { $_.Category, $_.Name };
}

function Get-AvailableWebPlatformInstallPackages {
		Check-RunAsAdministrator;
		$items = [System.Collections.ArrayList]@();
		$wpi = Get-WebPiExe;    
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

function Find-WebPlatformInstallPackages {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[Alias("s")]
		[string] $Search
	);
	$items = Get-AvailableWebPlatformInstallPackages | where { $_.ID -like "*$Search*"; }
  return $items | sort { $_.Category, $_.Name };
}

function Get-InstalledChocolateyPackages {
	Check-RunAsAdministrator;
	$cho = Get-ChocolateyExe;
	$packages = [System.Collections.ArrayList]@();
	Write-Host "Gathering installed chocolatey packages..."
	& $cho list -lo | foreach {
		if($_ -inotmatch "^\d+\spackages\s(found|installed)\.$") {
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
	$cho = Get-ChocolateyExe;
	$packages = [System.Collections.ArrayList]@();
	Write-Host "Gathering packages from Chocolatey repositories. This will take a little time..."
	& $cho list | foreach {
		if($_ -inotmatch "^\d+\spackages\s(found|installed)\.$") {
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

function Find-ChocolateyPackages {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[Alias("s")]
		[string] $Search
	);

	Check-RunAsAdministrator;
	$cho = Get-ChocolateyExe;
	$packages = [System.Collections.ArrayList]@();
	Write-Host "Searching for packages from Chocolatey repositories. This will take a little time..."
	& $cho search $Search | foreach {
		if($_ -inotmatch "^\d+\spackages\s(found|installed)\.$") {
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

function Get-AllInstalledPackages {
	Check-RunAsAdministrator;
	$wpi = Get-AvailableWebPlatformInstallPackages | where { $_.Category -eq "Previously Installed Products" };
	$wf = Get-AvailableWindowsFeatures | where { $_.Enabled -eq $true };
	$choco = Get-InstalledChocolateyPackages;
	return (( $wpi | Select-Object ID, Source, Category ) + ( $wf |  Select-Object ID, Source, Category ) + ( $choco | Select-Object ID, Source, Category, Version ) ) | sort { $_.Source, $_.ID };
}

function Get-ProvisionPackageConfig {
	Check-RunAsAdministrator;
	"<?xml version=`"1.0`"?>";
	"<packages>";
	Get-AllInstalledPackages | foreach {
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

function Find-ProvisionPackages {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[Alias("s")]
		[string] $Search
	);

	$wpi = Find-WebPlatformInstallPackages -Search $Search;
	$wf = Find-WindowsFeatures -Search $Search;
	$choco = Find-ChocolateyPackages -Search $Search;

	(( $wpi | Select-Object ID, Source, Category ) + 
		( $wf |  Select-Object ID, Source, Category ) + 
		( $choco | Select-Object ID, Source, Version, Category ) ) | sort { $_.Source, $_.Category, $_.ID };
}

function Check-RunAsAdministrator {
	if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
		throw "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!";
		#$arguments = "& '" + $MyInvocation.MyCommand.Definition + "'";
		#Start-Process powershell -Verb runAs -ArgumentList $arguments
  }
}

Export-ModuleMember -Function Out-ProvisionPackageConfig;

Export-ModuleMember -Function Invoke-ProvisionInstall;
Export-ModuleMember -Function Invoke-ProvisionRestore;

Export-ModuleMember -Function Get-InstalledChocolateyPackages;
Export-ModuleMember -Function Get-AvailableWebPlatformInstallPackages;
Export-ModuleMember -Function Get-AvailableWindowsFeatures;
Export-ModuleMember -Function Get-AvailableChocolateyPackages;
Export-ModuleMember -Function Get-AllInstalledPackages;

Export-ModuleMember -Function Find-ProvisionPackages;
Export-ModuleMember -Function Find-ChocolateyPackages;
Export-ModuleMember -Function Find-WebPlatformInstallPackages;
Export-ModuleMember -Function Find-WindowsFeatures;