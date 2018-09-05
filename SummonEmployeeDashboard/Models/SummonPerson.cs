using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    public class SummonPerson : Person
    {
        private long? _inactive = null;

        [JsonProperty(PropertyName = "inactive")]
        public long? Inactive
        {
            get => _inactive; set
            {
                _inactive = value;
                OnPropertyChanged("Inactive");
            }
        }
    }
}
