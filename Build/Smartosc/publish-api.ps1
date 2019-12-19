$ErrorActionPreference = "Stop";

$destFolder =  "C:\Temp\Deployment\Smartosc\smartFunds.Presentation"
$commonScriptPath = Resolve-Path "$PSScriptRoot\..\Common"

& "$commonScriptPath\publish-api.ps1" -config 'Debug' -destFolder $destFolder
& "$commonScriptPath\update-config-environment.ps1" -configPath "$destFolder\web.config" -environment 'Smartosc'

