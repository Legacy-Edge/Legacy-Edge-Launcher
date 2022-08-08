using Microsoft.Win32;
using ShortDev.Spartan.Launcher.Helpers;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ShortDev.Spartan.Launcher
{
    internal class Program
    {
        const string EdgeBasePath = @"C:\Windows\SystemApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\";
        const string MicrosoftEdge_exe = $"{EdgeBasePath}MicrosoftEdge.exe";

        static void Main(string[] args)
            => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            EnableSxS();
            DisableBrowserReplacement();

            if (!File.Exists(MicrosoftEdge_exe))
            {
                // Copy binaries
            }

            var anaheimKey = GetAnaheimStableKey();
            if (anaheimKey != null)
            {
                await UninstallAnaheimStableAsync(anaheimKey);
            }
            
            ApplicationActivationManager.ActivateApplication("Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge", null);
        }

        static void EnableSxS()
            => Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\EdgeUpdate", true).SetValue("Allowsxs", 1);

        static void DisableBrowserReplacement()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\EdgeUpdate\ClientState\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}", true);
            if (key == null)
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\ClientState\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}", true);

            key?.DeleteValue("BrowserReplacement");
        }

        static RegistryKey? GetAnaheimStableKey()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge");
            if (key == null)
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge");
            return key;
        }

        static async Task UninstallAnaheimStableAsync(RegistryKey anaheimKey)
        {
            string? uninstallCmd = anaheimKey.GetValue("UninstallString") as string;
            if (uninstallCmd == null)
                throw new InvalidDataException();

            TaskCompletionSource<bool> promise = new();

            var procces = Process.Start("cmd", $"/c {uninstallCmd}");
            procces.EnableRaisingEvents = true;
            procces.Exited += (s, e) => promise.TrySetResult(true);
            if (procces.HasExited)
                promise.TrySetResult(true);

            await promise.Task;
        }
    }
}
