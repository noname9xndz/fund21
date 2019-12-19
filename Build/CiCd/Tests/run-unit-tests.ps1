Write-Host ">> run-unit-tests"
$ErrorActionPreference = "Stop";

$testProjPath = Resolve-Path "$PSScriptRoot\..\..\..\tests\smartFunds.Service.UnitTests\"
$testProjPath = "$testProjPath\smartFunds.Service.UnitTests.csproj"

$res = dotnet test $testProjPath  --logger "trx;LogFileName=<TestResults.trx>"

if ([string]$res -match 'Failed: 0') {
	Write-Host $res
}
else {
    throw $res
}

Write-Host "<< run-unit-tests"