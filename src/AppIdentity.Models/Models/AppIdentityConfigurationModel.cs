using System;
using System.Collections.Generic;
using System.Text;

namespace AppIdentity.Models
{
    public class AppIdentityConfigurationModel
    {
        public int MaxSubjectLength { get; set; }
        public int MaxAppIdLength { get; set; }
        public int MaxMachineIdLength { get; set; }
    }
}
