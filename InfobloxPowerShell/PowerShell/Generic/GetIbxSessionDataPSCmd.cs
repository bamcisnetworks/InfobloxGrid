using BAMCIS.Infoblox.Core;
using Newtonsoft.Json;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
        VerbsCommon.Get,
        "IBXSessionData"
    )]
    public class GetIbxSessionDataPSCmd : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            string Content = $@"{{
""GridMaster"" : ""{InfobloxSessionData.GridMaster}""
""Version"" : ""{InfobloxSessionData.Version}""
""Cookie"" : ""{JsonConvert.SerializeObject(InfobloxSessionData.Cookie)}""
""UseSessionData : ""{InfobloxSessionData.UseSessionData}""
}}";
            WriteObject(Content);
        }
    }
}
