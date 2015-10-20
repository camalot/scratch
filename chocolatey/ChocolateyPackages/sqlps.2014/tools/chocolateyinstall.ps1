$ErrorActionPreference = 'Stop'; # stop on all errors
$packageName = 'sqlps.2014' # arbitrary name for the package, used in messages
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url = 'http://download.microsoft.com/download/1/3/0/13089488-91FC-4E22-AD68-5BE58BD5C014/ENU/x86/PowerShellTools.msi' # download url
$url64 = 'http://download.microsoft.com/download/1/3/0/13089488-91FC-4E22-AD68-5BE58BD5C014/ENU/x64/PowerShellTools.msi' # 64bit URL here or remove - if installer is both, use $url

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
  registryUninstallerKey = '{6A7D0067-CF88-453E-9EA5-A2008EC7CB19}' #ensure this is the value in the registry
  #checksum      = '{{Checksum}}'
  #checksumType  = 'md5' #default is md5, can also be sha1
  #checksum64    = '{{Checksumx64}}'
  #checksumType64= 'md5' #default is checksumType
}

Install-ChocolateyPackage @packageArgs

## Main helper functions - these have error handling tucked into them already
## see https://github.com/chocolatey/choco/wiki/HelpersReference
