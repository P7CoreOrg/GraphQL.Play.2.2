using AppIdentity.Contracts;

namespace AppIdentity.Services
{
    class InMemoryAppIdentityConfiguration : IAppIdentityConfiguration
    {
        public int MaxSubjectLength { get; set; }
        public int MaxAppIdLength { get; set; }
        public int MaxMachineIdLength { get; set; }
    }
}
