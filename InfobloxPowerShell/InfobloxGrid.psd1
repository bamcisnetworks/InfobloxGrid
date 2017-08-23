#
# Module manifest for module 'InfobloxGrid'
#
# Generated by: Michael Haken
#
# Generated on: 5/26/2017
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'InfobloxGrid.dll'

# Version number of this module.
ModuleVersion = '2.0.0.3'

# Supported PSEditions
# CompatiblePSEditions = @()

# ID used to uniquely identify this module
GUID = '8ec495df-0002-47ad-b785-133684c7c5e3'

# Author of this module
Author = 'Michael Haken'

# Company or vendor of this module
CompanyName = 'BAMCIS'

# Copyright statement for this module
Copyright = '(c) 2017 BAMCIS. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Providers a wrapper around the Infoblox WAPI. The current SDK supports objects up to version 2.0 of the WAPI, although the PowerShell cmdlets can be used with WAPI versions above 2.0. Extensible attributes on objects are not currently supported.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '3.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# CLRVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @("System.Management.Automation")

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
# FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = @()

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
CmdletsToExport = @(
	"Enter-IBXSession", 
	"New-IBXSession", 
	"Exit-IBXSession", 
	"Get-IBXObject", 
	"New-IBXObject", 
	"Set-IBXObject", 
	"Remove-IBXObject", 
	"Get-IBXGrid",
	"New-IBXZoneObject",
	"Set-IBXZoneObject",
	"New-IBXDhcpObject",
	"Set-IBXDhcpObject",
	"Add-IBXIPAddressToHostRecord",
	"New-IBXDnsHostRecord",
	"New-IBXDnsRecord",
	"Remove-IBXIPAddressFromHostRecord",
	"Set-IBXDnsHostRecord",
	"Set-IBXDnsRecord"
)

# Variables to export from this module
VariablesToExport = @()

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
AliasesToExport = @()

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        # Tags applied to this module. These help with module discovery in online galleries.
        Tags = @("PSModule", "Infoblox", "NIOS", "WAPI")

        # A URL to the license for this module.
        LicenseUri = 'https://github.com/bamcisnetworks/InfobloxGrid/blob/master/LICENSE'

        # A URL to the main website for this project.
        ProjectUri = 'https://github.com/bamcisnetworks/InfobloxGrid'

        # A URL to an icon representing this module.
        # IconUri = ''

        # ReleaseNotes of this module
        ReleaseNotes = '*2.0.0.3
Updated the unbound parameter methods to handle unbound parameters surrounded in quotes.
		
*2.0.0.2
Fixed the handling of getting the latest version where the JArray of supported_versions could not be cast to an IEnumerable<string>. Added functionality to catch and unroll aggregate exceptions.

*2.0.0.1
Implemented a "Basic" attribute to properties to enable the "BASIC" option for the -FieldsToReturn parameter on get objects.
		
*2.0.0.0
The module has been split into two separate projects. The first component is an SDK library that is compatible with .NET Framework and .NET Core. It contains all of the models and HTTP methods to execute against the Infoblox WAPI.

The second component is the PowerShell module that calls into the SDK. The whole module has been updated with the way it utilizes the cookie provided by Infoblox. You can now choose to do an Enter-IBXSession to set up persistent credentials, create a new session
with New-IBXSession and provide it to each IBX command, or provide explicit credentials to each command. The persistent cookie is updated with each new call to the Grid Master. You can clear the persistent credentials with Exit-IBXSession.

System.Net.Http 4.3.2, Newtonsoft.Json 10.0.2, System.Security.Cryptography.Algorithms 4.3.0, System.Security.Cryptography.Encoding 4.3.0, System.Security.Cryptography.Primitives 4.3.0, and System.Security.Cryptography.X509Certificates are bundled with the module.
'

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

