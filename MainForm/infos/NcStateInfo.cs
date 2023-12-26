using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainForm.infos
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class NcStateInfo
    {
        [JsonProperty(PropertyName = "dtList")]
        public List<MeasuringInfo> NcList
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "result")]
        public bool OK
        {
            get;
            set;
        }
    }
}
