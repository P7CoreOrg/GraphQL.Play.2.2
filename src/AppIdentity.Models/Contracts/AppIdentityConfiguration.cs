using System;
using System.Collections.Generic;
using System.Text;

namespace AppIdentity.Contracts
{
    public interface IAppIdentityConfiguration
    {

        int MaxSubjectLength { get; }
        int MaxAppIdLength { get; }
        int MaxMachineIdLength { get; }
    }
}
