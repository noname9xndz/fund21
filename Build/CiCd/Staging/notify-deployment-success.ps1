$ErrorActionPreference = "Stop";

$commonScriptPath = Resolve-Path "$PSScriptRoot\..\..\Common"

& "$commonScriptPath\notify-deployment-success.ps1" -environment 'Staging'
