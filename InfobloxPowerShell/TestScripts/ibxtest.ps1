ipmo infoblox

[string]$grid = "192.168.100.2"

Write-Host "Creating new zone" -ForegroundColor Green
$ref = New-IbxZoneObject -ZoneType ZONE_AUTH -ByAttribute -GridMaster $grid -Fqdn mike.test -PassThru
Write-Host "Zone ref " $ref
Write-Host "Getting zone" -ForegroundColor Green
$zone = Get-IBXObject -GridMaster $grid -SearchValue "mike.test" -ObjectType ZONE_AUTH -SearchField fqdn -SearchType Equality 
Write-Host "Searched zone " $zone.fqdn
$test = New-Object BAMCIS.Infoblox.InfobloxObjects.DNS.a("arecord.mike.test","192.168.100.3")
Write-Host "Creating DNS record" -ForegroundColor Green
$ref2 = New-IbxDnsRecord -InputObject $test -GridMaster $grid -PassThru
Write-Host "DNS record ref " $ref2
Write-Host "Creating Host Record" -ForegroundColor Green
$ref3 = New-IBXDnsHostRecord -HostName "mike2.mike.test" -IP 192.168.100.3 -GridMaster $grid -PassThru
Write-Host "Host record ref " $ref3
Write-Host "Adding IP to host record" -ForegroundColor Green
$ref4 = Add-IBXIPAddressToHostRecord -Reference $ref3 -IP 192.168.100.4 -GridMaster $grid -PassThru
Write-Host "Added IP " $ref4
Write-Host "Getting Host Record By Ref" -ForegroundColor Green
$ref5 = Get-IBXObject -Reference $ref3 -GridMaster $grid 
Write-Host "Host record obj " $ref5._ref
Write-Host "Deleting host record" -ForegroundColor Green
$ref6 = Remove-IBXObject -Reference $ref5._ref -GridMaster $grid  -Force -PassThru
Write-Host "Deleted host " $ref6
Write-Host "Deleting zone" -ForegroundColor Green
Remove-IBXObject -Reference $ref -GridMaster $grid  -Force -PassThru