using StructureMap;

namespace Payment.Gateway.Data.Configuration
{
    public class DataRegistry:Registry
    {
        public DataRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
