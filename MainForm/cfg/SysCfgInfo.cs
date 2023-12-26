using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainForm.cfg
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SysCfgInfo
    {
        [JsonProperty(PropertyName = "ipInfos")]
        public List<SerialIpInfo> InfoList
        {
            get;
            set;
        }
    }
}
