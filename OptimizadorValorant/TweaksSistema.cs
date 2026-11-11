using System.Diagnostics;
using System.IO;

namespace OptimizadorValorant
{
    public static class TweaksSistema
    {
        static void Cmd(string prog, string args)
        {
            var p = new ProcessStartInfo { FileName = prog, Arguments = args, UseShellExecute = false, CreateNoWindow = true };
            Process.Start(p)?.WaitForExit();
        }

        static string PowerShell(string script)
        {
            var p = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };
            var proc = Process.Start(p);
            string outp = proc?.StandardOutput.ReadToEnd() ?? "";
            proc?.WaitForExit();
            return outp;
        }

        public static void PlanMaximoRendimiento()
        {
            Cmd("powercfg", "/setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
        }

        public static void DesactivarHPET()
        {
            Cmd("bcdedit", "/deletevalue useplatformclock");
            Cmd("bcdedit", "/set disabledynamictick yes");
        }

        public static void LimpiarTemporales()
        {
            string temp = Path.GetTempPath();
            try
            {
                foreach (var f in Directory.GetFiles(temp)) try { File.Delete(f); } catch { }
                foreach (var d in Directory.GetDirectories(temp)) try { Directory.Delete(d, true); } catch { }
            }
            catch { }
        }

        // tamaño fijo en MB, por WMI
        public static void AjustarPaginacion(int mb = 8192)
        {
            PowerShell("$cs = Get-WmiObject Win32_ComputerSystem -EnableAllPrivileges; $cs.AutomaticManagedPagefile = $false; $cs.Put() | Out-Null");
            PowerShell($"$pf = Get-WmiObject Win32_PageFileSetting; if($pf){{ $pf.InitialSize = {mb}; $pf.MaximumSize = {mb}; $pf.Put() | Out-Null }} else {{ Set-WmiInstance -Class Win32_PageFileSetting -Arguments @{{Name='C:\\pagefile.sys'; InitialSize={mb}; MaximumSize={mb}}} | Out-Null }}");
        }

        public static string ListarProgramasInicio()
        {
            return PowerShell("Get-CimInstance Win32_StartupCommand | Select-Object Name, Command | Format-Table -AutoSize | Out-String");
        }

        // lo borra de la clave Run del usuario
        public static void DesactivarProgramaInicio(string nombre)
        {
            var p = new ProcessStartInfo
            {
                FileName = "reg",
                Arguments = $"delete \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\" /v \"{nombre}\" /f",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(p)?.WaitForExit();
        }
    }
}