Import-Module WebAdministration
$iisbinding = Get-WebBinding -Name "com.orbitteam.api" | where {$_.bindingInformation -like "*smartFunds-api-staging.orbitteam.com*"}
if ($iisbinding -eq $null)
{
    Write-Output $iisbinding
    New-WebBinding -Name "com.orbitteam.api" -IPAddress "*" -Port 80 -HostHeader smartFunds-api-staging.orbitteam.com
}

$webDir = "C:\inetpub\wwwroot\com.orbitteam.api\PDFFiles"
if (Test-Path $webDir) { 
    $acl = Get-Acl $dir
	$rule = New-Object  System.Security.Accesscontrol.FileSystemAccessRule("IIS_IUSRS","FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")
	$acl.SetAccessRule($rule)
	Set-Acl $dir $acl
}
