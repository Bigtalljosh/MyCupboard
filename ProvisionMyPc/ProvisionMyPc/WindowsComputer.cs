using Cupboard;

namespace ProvisionMyPc
{
    public sealed class WindowsComputer : Catalog
    {
        public override void Execute(CatalogContext context)
        {
            if (!context.Facts.IsWindows())
            {
                return;
            }

            context.UseManifest<Chocolatey>();
        }
    }
}
