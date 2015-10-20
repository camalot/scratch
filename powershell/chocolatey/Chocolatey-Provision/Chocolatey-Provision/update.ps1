Import-Module ".\Machine-Provision.psm1";

Invoke-ChocolateyUpdate -FilePath "$env:AppData\Chocolatey\Provision.config";