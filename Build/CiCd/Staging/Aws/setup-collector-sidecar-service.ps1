Write-Host '>> setup-collector-sidecar-service'

$ErrorActionPreference = "Stop";

$destDirectory = "C:\Program Files\Graylog\collector-sidecar";

if (Test-Path $destDirectory) { 
     Copy-Item -Path (Resolve-Path "$PSScriptRoot\collector_sidecar.yml") -Destination $destDirectory 
	 Restart-Service -Name collector-sidecar
}

Write-Host '<< setup-collector-sidecar-service'

