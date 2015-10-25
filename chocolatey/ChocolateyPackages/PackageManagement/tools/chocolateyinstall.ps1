function Invoke-DownloadFile {
	Param (
		[string]$Url,
		[string]$File
	);
	process {
		"Downloading $Url to $File" | Write-Host;
		$downloader = new-object System.Net.WebClient;
		$downloader.Proxy.Credentials=[System.Net.CredentialCache]::DefaultNetworkCredentials;
		$downloader.DownloadFile($Url, $File);
	}
}

function Expand-7ZipArchive {
	Param (
		[Parameter(Mandatory=$true, Position=0)]
		[string] $Path,
		[Parameter(Mandatory=$true, Position=1)]
		[string] $DestinationPath
	);

	$scriptRootPath = Get-ScriptRoot;
	$toolsDir = (Join-Path -Path $scriptRootPath -ChildPath "tools");

	if(!(Test-Path -Path $toolsDir)) {
		New-Item -Path $toolsDir -ItemType Directory | Out-Null;
	}
	$7zaExe = (Join-Path -Path $toolsDir -ChildPath "7za.exe");
	if(!(Test-Path -Path $7zaExe)) {
		# download 7zip
		Write-Host "Download 7Zip commandline tool";
		Invoke-DownloadFile -Url 'https://raw.githubusercontent.com/camalot/psievm/master/psievm/.tools/7za.exe' -File "$7zaExe";
	}
	Start-Process "$7zaExe" -ArgumentList "x -o`"$DestinationPath`" -y `"$Path`"" -Wait -NoNewWindow | Write-Host;
}

function Install-PackageManagement {
	param (
		[Parameter(Mandatory=$true)]
		[string] $ModulesPath
	);
	begin {
		$url = "http://de.bit13.com/psievm/PackageManagement.zip";

		$tempDir = (Join-Path $env:TEMP "pm");
		if(!(Test-Path -Path $tempDir)) {
			New-Item -Path $tempDir -ItemType Directory | Out-Null;
		}
		$file = (Join-Path $tempDir "PackageManagement.zip");

		if(!(Test-Path -Path $ModulesPath)) {
			New-Item -Path $ModulesPath -ItemType Directory | Out-Null;
		}
	}
	process {
		Invoke-DownloadFile -url $url -file $file;
		Expand-7ZipArchive -Path $file -DestinationPath $ModulesPath;
		Get-ChildItem -Path "$ModulesPath" -File -Recurse | Unblock-File | Out-Null;
	}
}

function Get-PSModulePath {

	#$psmp = (Join-Path -Path $env:ProgramFiles -ChildPath "WindowsPowerShell\Modules\");

	#if(Test-Path -Path $psmp) {
	#	return $psmp;
	#}

	$docsPath = (Get-EnvironmentFolderPath -Name MyDocuments);
	if(-not $docsPath) {
		# if MyDocuments doesn't give anything, use the user profile
		return (Join-Path -Path $env:USERPROFILE -ChildPath "Documents\WindowsPowerShell\Modules\");
	} else {
		return (Join-Path -Path $docsPath -ChildPath "WindowsPowerShell\Modules\");
	}
}

function Get-EnvironmentFolderPath {
	Param (
		[Parameter(Mandatory=$true)]
		[string] $Name
	);

	return [Environment]::GetFolderPath($Name);
}

function Get-ScriptRoot {
	if(-not $PSScriptRoot) {
		if(-not $PSCommandPath) {
			return (Split-Path -Path $MyInvocation.MyCommand.Path -Parent);
		} else {
			return (Split-Path -Path $PSCommandPath -Parent);
		}
	} else {
		return (Resolve-Path -Path $PSScriptRoot);
	}
}

function Invoke-Setup {
		$ModulesRoot = Get-PSModulePath;
	Install-PackageManagement -ModulesPath $ModulesRoot;
}



if( ($DoSetup -eq $null) -or ($DoSetup -eq $true) ) {
	Invoke-Setup;
}