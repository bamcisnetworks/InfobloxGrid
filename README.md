# Infoblox Grid WAPI Cmdlets

This project was designed to provide an object-oriented wrapper to the Infoblox WAPI. It uses .NET objects to dynamically populate object types and attributes that are supported by
the API, which extrememly simplifies what the end user needs to know in order to interact with the API. No real existing knowledge of the Infoblox object types or structs are needed.

The current version of the project is stable, previous commits could contain serious bugs. I know that the current version of the project does not contain a help file for the binary
module. It's on the list to do, but this project was mostly for myself.

Although the Version parameter of the cmdlets have values in the validate set of 2.1 to 2.3 as well as LATEST, the Infoblox objects that are part of this code were derived from the 
WAPI version 2.0 definition. Maybe I'll add an attribute to all of the objects with their supported versions to help support better backwards compatibility, or update the objects to 
version 2.3, but it's a lot of work to go through that stupidly long Infoblox WAPI pdf document and then actually see what changed and make a bunch of code updates.

## Example Usage

    Import-Module -Name InfobloxGrid

    [System.String]$GM = "192.168.0.90"

    Enter-IBXSession -GridMaster $GM -Credential (New-Object -TypeName System.Management.Automation.PSCredential("admin", (ConvertTo-SecureString -String "infoblox" -AsPlainText -Force))) -Version LATEST

    $ZoneRef = New-IbxZoneObject -ZoneType ZONE_AUTH -Fqdn contoso.com -PassThru

    $Zones = Get-IBXObject -SearchValue "contoso.com" -ObjectType ZONE_AUTH -SearchField fqdn -SearchType Equality 

    $ARecord = New-Object BAMCIS.Infoblox.InfobloxObjects.DNS.a("arecord.contoso.com","192.168.100.3")

    $RecordRef = New-IbxDnsRecord -InputObject $ARecord -PassThru -Verbose

    $HostRecordRef = New-IBXDnsHostRecord -HostName "server.contoso.com" -IP 192.168.100.3 -PassThru

    $HostIPRef = Add-IBXIPAddressToHostRecord -Reference $HostRecordRef -IP 192.168.100.4 -PassThru -Force

    $HostRecordObject = Get-IBXObject -Reference $HostRecordRef

    $RemovedRecord = Remove-IBXObject -Reference $HostRecordObject._ref -Force -PassThru

    Remove-IBXObject -Reference $Zones[0]._ref -Force -PassThru

    Exit-IBXSession

This could have also been done with a New-IBXSession command like this:

    $Session = New-IBXSession -GridMaster $GM -Credential (New-Object -TypeName System.Management.Automation.PSCredential("admin", (ConvertTo-SecureString -String "infoblox" -AsPlainText -Force))) -Version LATEST

And then supplied the $Session variable to each command. The Grid Master and credentials parameters could also explicitly by specified for each command, but this is slower. The Session, either by providing the session or
calling Enter-IBXSession uses an HTTP cookie for authentication and resorts to Basic HTTP authentication only if the cookie expires. This approach provides faster HTTP communication.

## Handling Exceptions

The cmdlets all throw a custom exception, which you can catch and enumerate details about the error from the grid master. The following is an example of the Grid Master timing out which
causes an exception to be thrown and how to deal with processing records. The ErrorActionPreference variable should be set to Stop in order to catch the exception.

    $TopLevelDomains = @("contoso.com", "tailspintoys.com")

    [System.Collections.Generic.Stack[System.String]]$Zones = New-Object -TypeName System.Collections.Generic.Stack[System.String]

    foreach ($Zone in $TopLevelDomains)
    {
        $Zones.Push($Zone)
    }

    while ($Zones.Count -gt 0)
    {
        $Zone = $Zones.Pop()

        try
        {
			Write-Host "ZONE: $Zone" -ForegroundColor Green

			New-IBXZoneObject -ZoneType ZONE_AUTH -Fqdn $Zone -Timeout 300 -ErrorAction Stop
		}
		catch [BAMCIS.Infoblox.Errors.InfobloxCustomException]
		{
			if ($_.Exception.HttpResponseCode -eq 502 -or $_.Exception.Error -eq "ConnectFailure")
			{
				Write-Host -Object "`tRETRY: Adding retry zone $Zone and all associated records" -ForegroundColor Yellow
				$Zones.Push($Zone)
			}
		}
	}

The custom exception has the following properties:

Text - The text of the error returned from Infoblox. If Infoblox didn't throw the error, this is the same as the message.
HttpResponseCode - The HTTP response code. If the error didn't occur as part of the response, the HResult from the exception is included.
HttpStatus - The friendly name for the response code.
HttpErrorReason - The HTTP reason phrase.
Error - The error message returned from Infoblox as part of the response body, or the exception status.

## Revision History

### 2.0.0.3
Updated the unbound parameter methods to handle unbound parameters surrounded in quotes.

### 2.0.0.2
Fixed the handling of getting the latest version where the JArray of supported_versions could not be cast to an IEnumerable<string>. Added functionality to catch and unroll aggregate exceptions.

### 2.0.0.1
Implemented a "Basic" attribute to properties to enable the "BASIC" option for the -FieldsToReturn parameter on get objects.

### 2.0.0.0
The module has been split into two separate projects. The first component is an SDK library that is compatible with .NET Framework and .NET Core. It contains all of the models and HTTP methods to execute against the Infoblox WAPI.

The second component is the PowerShell module that calls into the SDK. The whole module has been updated with the way it utilizes the cookie provided by Infoblox. You can now choose to do an Enter-IBXSession to set up persistent credentials, create a new session
with New-IBXSession and provide it to each IBX command, or provide explicit credentials to each command. The persistent cookie is updated with each new call to the Grid Master. You can clear the persistent credentials with Exit-IBXSession.

System.Net.Http 4.3.2, Newtonsoft.Json 10.0.2, System.Security.Cryptography.Algorithms 4.3.0, System.Security.Cryptography.Encoding 4.3.0, System.Security.Cryptography.Primitives 4.3.0, and System.Security.Cryptography.X509Certificates are bundled with the module.