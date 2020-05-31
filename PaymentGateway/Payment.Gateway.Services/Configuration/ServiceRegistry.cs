using StructureMap;

namespace Payment.Gateway.Services.Configuration
{
    public class ServiceRegistry:Registry
    {
        public ServiceRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
