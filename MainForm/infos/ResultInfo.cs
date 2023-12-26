using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainForm.infos
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ResultInfo
    {
        [JsonProperty(PropertyName = "id")]
        public long ID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "result")]
        public int Result
        {
            get;
            set;
        }



        public string Reason
        {
            get;
            set;
        }
    }
}
