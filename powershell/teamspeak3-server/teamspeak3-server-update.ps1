param(
    [ValidateScript({Test-Path -Path $_ -PathType "Container"})]
    [string] $Path = (Resolve-Path -Path ".\"),
    [switch] $Use64bit
);

function Download-TeamspeakServer {
    param(
        [Parameter(Mandatory=$true)]
        [ValidateScript({Test-Path -Path $_ -PathType "Container"})]
        [string] $Path,
        [switch] $Use64bit

    );
    begin {
        $fileName = "teamspeak3-server_win{0}-{1}.zip"
        $4netplayers = "http://dl.4players.de/ts/releases/{0}/{1}";
        $gemedde = "http://teamspeak.gameserver.gamed.de/ts3/releases/{0}/{1}";
        $ResolvedPath = (Resolve-Path -Path $Path);
        $ts64Exists = Test-Path -Path (Join-Path -Path $ResolvedPath -ChildPath "ts3server_win64.exe");
        $get64bit = $Use64bit.IsPresent -or $ts64Exists -eq $true;
    }
    process {
        try {
            $doc = Invoke-WebRequest -URI "https://www.teamspeak.com/downloads";

            $downloadList = $doc.ParsedHtml.getElementById("ts-downloads")
            $listItems = $downloadList.getElementsByTagName("li");
            if ($listItems.Length -gt 1) {
                $groups = ($listItems[1].getElementsByClassName("download__icon-offset") | select -First 2);
                foreach ( $group in $groups ) {
                    $version = ($group.getElementsByClassName("version") | select -First 1).innerText
                    $checksum = ($group.getElementsByClassName("checksum") | select -First 1).innerText -replace "SHA1: ", ""
                    $m = $group.innerText -imatch "^Server\s((?:32|64))-bit";
                    if ($m) {
                        $architecture = $matches[1];
                    } else {
                        throw "unable to get architecture";
                    }

                    if ($architecture -ine "64" -and $get64bit) {
                        "Skipping 32-bit" | Write-Host;
                        continue;
                    } elseif ( $architecture -ieq "64" -and !($get64bit) ) {
                        "Skipping 64-bit" | Write-Host;
                        continue;
                    }
                    $processName = "ts3server_win$($architecture)"
                    
                    if ( Test-FileVersion -Version $version -Path $ResolvedPath ) {
                        "Version $version already exists; exiting." | Write-Host;
                        return;
                    }

                    # delete all version files in the directory
                    Get-ChildItem -Path $ResolvedPath -Filter "*.version" | Remove-Item -Force;

                    if ( Test-Process -Name $processName ) {
                        "Stopping Process: $processName" | Write-Host;
                        Stop-Process -Name $processName -Force | Write-Host;
                    }
            
                    $dlFile = $fileName -f $architecture, $version
                    try {
                        $url = ($4netplayers -f $version, $dlFile);
                        "Downloading $url" | Write-Host;
                        $tempFile = Download-ZipFile -Url $url -Checksum $checksum;
                    } catch {
                        $url = ( $gemedde -f $version, $dlFile);
                        "First host failed. Downloading from mirror: $url" | Write-Host;
                        $tempFile = Download-ZipFile -Url ( $gemedde -f $version, $dlFile) -Checksum $checksum;
                    }

                    if ( !(Test-Path -Path $tempFile) ) {
                        throw "unable to locate the downloaded file";
                    }

                    "Expanding archive $tempFile -> $ResolvedPath" | Write-Host;
                    $tempPath = (Join-Path -Path $env:TEMP -ChildPath "ts3server");
                    if(!(Test-Path -Path $tempPath)) {
                        New-Item -Path $tempFile -ItemType Directory -Force | Out-Null;
                    }
                    Expand-Archive -Path $tempFile -DestinationPath $tempPath | Write-Host;
                    "Unblocking all files" | Write-Host;
                    Get-ChildItem -Path $tempPath -File -Recurse | Unblock-File | Out-Null;

                    # Copy all to $Path
                    Get-ChildItem -Path (Join-Path -Path $tempPath -ChildPath "teamspeak3-server_win$architecture") | % { 
                        Copy-Item $_.fullname $ResolvedPath -Recurse -Force
                    }

                    "Starting Process: $processName" | Write-Host;
                    Start-Process -WorkingDirectory $Path -FilePath "$processName.exe";
                    
                    # Create version file
                    New-Item -Path (Join-Path -Path $ResolvedPath -ChildPath "$version.version") -ItemType File -Force | Out-Null;

                    "Clean up of '$tempPath'" | Write-Host;
                    Remove-Item -Path $tempPath -Recurse -Force | Out-Null;
                    return;
                }
            }
        } catch {
            throw;
        }
        throw "Error while trying to process url";
    }
}

function Test-FileVersion {
    param(
        [Parameter(Mandatory=$true)]
        [string] $Version,
        [Parameter(Mandatory=$true)]
        [string] $Path
    );
    $rpath = Resolve-Path -Path $Path;
    $verFile = (Join-Path -Path $rpath -ChildPath "$Version.version");
    return (Test-Path -Path $verFile);
}

function Test-Process {
    param(
        [Parameter(Mandatory=$true)]
        [string] $Name
    );
    $result = Get-Process -Name $Name -ErrorAction SilentlyContinue;
    $result | Write-Host;
    return $result -ne $null;
}

function Download-ZipFile {
    param(
        [Parameter(Mandatory=$true)]
        [string] $Url,
        [Parameter(Mandatory=$true)]
        [string] $Checksum
    );

    process {
        try {
        $file = (Join-Path -Path $env:TEMP -ChildPath "ts3server.zip");
		$downloader = new-object System.Net.WebClient;
		#$downloader.Proxy.Credentials=[System.Net.CredentialCache]::DefaultNetworkCredentials;
        "Downloading from $Url -> $file" | Write-Host;
		$downloader.DownloadFile($Url, $file);

        $hash = (Get-FileHash -Algorithm SHA1 -Path $file).Hash;
        if($hash -ine $Checksum) {
            throw "'$hash' does not match checksum: '$Checksum'";
        }
        return $file;
        } catch [Exception] {
            $_ | Write-Host;
        }
	}
}

Download-TeamspeakServer -Path (Resolve-Path -Path $Path) -Use64bit:$Use64bit.IsPresent