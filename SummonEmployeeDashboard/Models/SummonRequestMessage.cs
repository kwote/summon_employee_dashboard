using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Models
{
    class SummonRequestMessage
    {
        [JsonProperty(PropertyName = "target")]
        public int Target
        {
            get; set;
        }
        [JsonProperty(PropertyName = "data")]
        public SummonRequest Data { get; set; }

        [JsonProperty(PropertyName = "caller")]
        public Person Caller { get; set; }

        [JsonProperty(PropertyName = "callee")]
        public Person Callee { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string MessageType
        {
            get; set;
        }
    }
}
