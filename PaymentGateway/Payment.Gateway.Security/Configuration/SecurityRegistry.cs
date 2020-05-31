using StructureMap;

namespace Payment.Gateway.Security.Configuration
{
    public class SecurityRegistry: Registry
    {
        public SecurityRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
