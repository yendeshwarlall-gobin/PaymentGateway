using StructureMap;
namespace Payment.Gateway.Web.Api.DependencyResolution {
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
        }

        #endregion
    }
}