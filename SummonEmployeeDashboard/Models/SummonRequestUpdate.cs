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
    public enum UpdateType {
        Accept, Reject, Cancel,
        Create
    }
    public class SummonRequestUpdate
    {
        public UpdateType UpdateType { get; set; }
        public SummonRequest Request { get; set; }
    }
}
