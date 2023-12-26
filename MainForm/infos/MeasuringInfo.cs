using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainForm.infos
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasuringInfo
    {
        [JsonProperty(PropertyName = "equipip")]
        public string Ip
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string Data
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "id")]
        public long DtId
        {
            get;
            set;
        }
        public int Result
        {
            get;
            set;
        }

    }
}
