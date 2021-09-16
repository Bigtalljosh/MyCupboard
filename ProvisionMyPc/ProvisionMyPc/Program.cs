using Cupboard;
using ProvisionMyPc;

return CupboardHost.CreateBuilder()
            .AddCatalog<WindowsComputer>()
            .Run(args);
