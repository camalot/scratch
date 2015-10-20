$ErrorActionPreference = 'Stop'; # stop on all errors

$packageName = 'sqlps.2014'
# registry uninstaller key name is the key that is found at HKLM:\Software\Windows\CurrentVersion\Uninstall\ THE NAME
$registryUninstallerKeyName = '{6A7D0067-CF88-453E-9EA5-A2008EC7CB19}' #ensure this is the value in the registry
$msiProductCodeGuid = '{6A7D0067-CF88-453E-9EA5-A2008EC7CB19}'
$shouldUninstall = $true

$local_key     = "HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall\$registryUninstallerKeyName"
# local key 6432 probably never exists
$local_key6432   = "HKCU:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\$registryUninstallerKeyName" 
$machine_key   = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$registryUninstallerKeyName"
$machine_key6432 = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\$registryUninstallerKeyName"

$file = @($local_key, $local_key6432, $machine_key, $machine_key6432) `
    | ?{ Test-Path $_ } `
    | Get-ItemProperty `
    | Select-Object -ExpandProperty UninstallString

if ($file -eq $null -or $file -eq '') {
    Write-Host "$packageName has already been uninstalled by other means."
    $shouldUninstall = $false
}

# The below is somewhat naive and built for EXE installers
#$installerType = 'EXE' 
#$silentArgs = '/S'
#$validExitCodes = @(0)

#if (!(Test-Path $file)) {
#    Write-Host "$packageName has already been uninstalled by other means."
#    $shouldUninstall = $false
#}

# The below is somewhat naive and built for MSI installers
$installerType = 'MSI' 
# The Product Code GUID is all that should be passed for MSI, and very FIRST,
# because it comes directly after /x, which is already set in the 
# Uninstall-ChocolateyPackage msiargs (facepalm).
$silentArgs = "$msiProductCodeGuid /qn /norestart"
# https://msdn.microsoft.com/en-us/library/aa376931(v=vs.85).aspx
$validExitCodes = @(0, 3010, 1605, 1614, 1641)
# Don't pass anything for file, it is ignored for msi (facepalm number 2) 
# Alternatively if you need to pass a path to an msi, determine that and use
# it instead of $msiProductCodeGuid in silentArgs, still very first
$file = ''

if ($shouldUninstall) {
 Uninstall-ChocolateyPackage -PackageName $packageName -FileType $installerType -SilentArgs $silentArgs -validExitCodes $validExitCodes -File $file
}

## OTHER HELPERS
## https://github.com/chocolatey/choco/wiki/HelpersReference
#Uninstall-ChocolateyZipPackage
#Uninstall-BinFile # Only needed if you added one in the installer script, choco will remove the ones it added automatically.
#remove any shortcuts you added

