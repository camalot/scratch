$ErrorActionPreference = 'Stop'; # stop on all errors
$packageName = 'PackageManagement' # arbitrary name for the package, used in messages
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url = 'https://download.microsoft.com/download/4/1/A/41A369FA-AA36-4EE9-845B-20BCC1691FC5/PackageManagement_x86.msi' # download url
$url64 = 'https://download.microsoft.com/download/4/1/A/41A369FA-AA36-4EE9-845B-20BCC1691FC5/PackageManagement_x64.msi' # 64bit URL here or remove - if installer is both, use $url


New-Item -Path "$env:TEMP\chocolatey\$packageName" -Force | Out-Null;

$packageArgs = @{
  packageName   = $packageName
  unzipLocation = $toolsDir
  fileType      = 'msi' #only one of these: exe, msi, msu
  url           = $url
  url64bit      = $url64

  #MSI
  silentArgs    = "/qn /norestart /l*v '$env:TEMP\chocolatey\$packageName\install.log'" # ALLUSERS=1 DISABLEDESKTOPSHORTCUT=1 ADDDESKTOPICON=0 ADDSTARTMENU=0
  validExitCodes= @(0, 3010, 1641)
  # optional
  registryUninstallerKey = '{57E5A8BB-41EB-4F09-B332-B535C5954A28}' #ensure this is the value in the registry
  #checksum      = '{{Checksum}}'
  #checksumType  = 'md5' #default is md5, can also be sha1
  #checksum64    = '{{Checksumx64}}'
  #checksumType64= 'md5' #default is checksumType
}

Install-ChocolateyPackage @packageArgs

## Main helper functions - these have error handling tucked into them already
## see https://github.com/chocolatey/choco/wiki/HelpersReference
