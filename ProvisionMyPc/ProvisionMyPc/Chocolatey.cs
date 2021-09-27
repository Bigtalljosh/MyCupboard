using Cupboard;

namespace ProvisionMyPc
{
    public sealed class Chocolatey : Manifest
    {
        private readonly List<string> _appsToInstall;

        public Chocolatey()
        {
            _appsToInstall = new List<string>{
                "googlechrome",
                "7zip",
                "vlc",
                "vscode",
                "sql-server-management-studio",
                "microsoft-windows-terminal",
                "spotify",
                "slack",
                "postman",
                "docker-desktop",
                "powertoys",
                "sublimetext3",
                "1password",
                "discord",
                "visualstudio2019community",
                "github-desktop"
            };
        }

        public override void Execute(ManifestContext context)
        {
            // Download
            context.Resource<Download>("https://chocolatey.org/install.ps1")
                .ToFile("~/install-chocolatey.ps1");

            // Set execution policy
            context.Resource<RegistryValue>("Set execution policy")
                .Path(@"HKLM:\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell")
                .Value("ExecutionPolicy")
                .Data("Unrestricted", RegistryValueKind.String);

            // Install
            context.Resource<PowerShell>("Install Chocolatey")
                .Script("~/install-chocolatey.ps1")
                .Flavor(PowerShellFlavor.PowerShell)
                .RequireAdministrator()
                .Unless("if (Test-Path \"$($env:ProgramData)/chocolatey/choco.exe\") { exit 1 }")
                .After<RegistryValue>("Set execution policy")
                .After<Download>("https://chocolatey.org/install.ps1");

            foreach(var app in _appsToInstall)
            {
                context.Resource<ChocolateyPackage>(app)
                    .Ensure(PackageState.Installed)
                    .After<PowerShell>("Install Chocolatey");
            }
        }
    }
}
