Write-Host ">> run-integration-tests"
$ErrorActionPreference = "Stop";

$testPath = Resolve-Path "$PSScriptRoot\..\..\..\tests\smartFunds.IntegrationTests\"
$testProjPath = "$testPath\smartFunds.IntegrationTests.csproj"
$settingFilePath = Resolve-Path "$PSScriptRoot\appsettings.json"
Copy-Item -Force $settingFilePath $testPath

$res = dotnet test $testProjPath  --logger "trx;LogFileName=<TestResults.trx>"

if ([string]$res -match 'Failed: 0') {
	Write-Host $res
}
else {
    throw $res
}


Write-Host "<< run-integration-tests"