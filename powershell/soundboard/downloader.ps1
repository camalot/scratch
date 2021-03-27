
$board = "main";
$cat = "arnold";
$output = "D:\Data\OBS\audio\soundboards\$cat-$board";

if (!(Test-Path -Path $output)) {
    Write-Host "Create Path $output";
    New-Item -Path $output -ItemType directory | Out-Null;
}

$urlBase = "http://www.realmofdarkness.net/audio/$cat/$board";
$list = (Invoke-WebRequest "http://www.realmofdarkness.net/scripts/sb/$cat/$board/sounds.js").Content | foreach { $_.Replace('const sounds = ', '').Replace(';', '') } | ConvertFrom-Json;

$list | foreach {
    Write-Host "Downloading: $_";
    Invoke-WebRequest -Uri "$urlBase/$_.mp3" -OutFile "$output\$_.mp3";
}
