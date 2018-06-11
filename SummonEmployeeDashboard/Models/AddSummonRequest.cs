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
    class AddSummonRequest
    {
        [JsonProperty(PropertyName = "callerId")]
        public int CallerId { get; set; }
        [JsonProperty(PropertyName = "targetId")]
        public int TargetId { get; set; }
    }
}
