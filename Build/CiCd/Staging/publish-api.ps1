$ErrorActionPreference = "Stop";

$destFolder = Resolve-Path "$PSScriptRoot\..\.."
$destFolder =  "$destFolder\PublishApi"
$commonScriptPath = Resolve-Path "$PSScriptRoot\..\..\Common"

& "$commonScriptPath\publish-api.ps1" -config 'Release' -destFolder $destFolder
& "$commonScriptPath\update-config-environment.ps1" -configPath "$destFolder\web.config" -environment 'Staging'
& "$commonScriptPath\copy-aws-spec-files.ps1" -srcFolder  (Resolve-Path "$PSScriptRoot\Aws") -destFolder $destFolder




