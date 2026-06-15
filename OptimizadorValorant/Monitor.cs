using LibreHardwareMonitor.Hardware;
using System.Text;
using System.Management;
using System.Threading;

namespace OptimizadorValorant
{
    public static class Monitor
    {
        public static string LeerTemperaturas()
        {
            var sb = new StringBuilder();
            var computer = new Computer { IsCpuEnabled = true, IsGpuEnabled = true };
            computer.Open();

            foreach (var hw in computer.Hardware)
            {
                hw.Update();

                foreach (var s in hw.Sensors)
                {
                    if (s.SensorType == SensorType.Temperature && s.Value.HasValue)
                    {
                        double t = s.Value.Value;
                        string a = t >= 90 ? "  <-- THROTTLING PROBABLE"
                                 : t >= 80 ? "  <-- temp alta"
                                 : "";

                        sb.AppendLine($"{hw.Name} - {s.Name}: {t:0.#} grados{a}");
                    }
                }
            }

            computer.Close();

            return sb.Length == 0
                ? "No se detectaron sensores de temperatura."
                : sb.ToString();
        }

        public static string VersionDriverGPU()
        {
            var sb = new StringBuilder();
            var sr = new ManagementObjectSearcher(
                "SELECT Name, DriverVersion, DriverDate FROM Win32_VideoController");

            foreach (ManagementObject mo in sr.Get())
            {
                sb.AppendLine($"GPU: {mo["Name"]}");
                sb.AppendLine($"Version driver: {mo["DriverVersion"]}");

                string f = mo["DriverDate"]?.ToString() ?? "";

                if (f.Length >= 8)
                {
                    var fecha = new System.DateTime(
                        int.Parse(f.Substring(0, 4)),
                        int.Parse(f.Substring(4, 2)),
                        int.Parse(f.Substring(6, 2)));

                    int dias = (System.DateTime.Now - fecha).Days;

                    sb.AppendLine($"Fecha driver: {fecha:dd/MM/yyyy}");
                    sb.AppendLine(
                        dias > 180
                            ? $">> Driver de {dias} dias. Conviene actualizar."
                            : $">> Driver actualizado ({dias} dias).");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string DetectarCuelloBotella()
        {
            var c = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true
            };

            c.Open();

            foreach (var hw in c.Hardware)
                hw.Update();

            Thread.Sleep(1000);

            foreach (var hw in c.Hardware)
                hw.Update();

            double cpu = -1;
            double gpu = -1;

            foreach (var hw in c.Hardware)
            {
                foreach (var s in hw.Sensors)
                {
                    if (s.SensorType == SensorType.Load && s.Value.HasValue)
                    {
                        if (hw.HardwareType == HardwareType.Cpu &&
                            s.Name.Contains("Total"))
                        {
                            cpu = s.Value.Value;
                        }

                        if ((hw.HardwareType == HardwareType.GpuNvidia ||
                             hw.HardwareType == HardwareType.GpuAmd ||
                             hw.HardwareType == HardwareType.GpuIntel) &&
                            s.Name.Contains("Core"))
                        {
                            gpu = s.Value.Value;
                        }
                    }
                }
            }

            c.Close();

            var sb = new StringBuilder();

            sb.AppendLine($"Uso CPU: {(cpu < 0 ? "N/D" : cpu.ToString("0") + "%")}");
            sb.AppendLine($"Uso GPU: {(gpu < 0 ? "N/D" : gpu.ToString("0") + "%")}");
            sb.AppendLine();

            if (cpu >= 0 && gpu >= 0)
            {
                sb.AppendLine(
                    cpu > 90 && gpu < 70
                        ? ">> Posible cuello de botella en CPU."
                        : gpu > 90 && cpu < 70
                            ? ">> Posible cuello de botella en GPU."
                            : ">> Sin cuello de botella claro ahora.");
            }
            else
            {
                sb.AppendLine(">> No se pudo leer la carga de un componente.");
            }

            return sb.ToString();
        }

        // Uso de RAM (%) y tamaño de archivos temporales (MB)
        public static (int ramPct, int tempMB) EstadoRapido()
        {
            int ramPct = 0;

            try
            {
                var sr = new ManagementObjectSearcher(
                    "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

                foreach (ManagementObject mo in sr.Get())
                {
                    double total = double.Parse(mo["TotalVisibleMemorySize"].ToString());
                    double libre = double.Parse(mo["FreePhysicalMemory"].ToString());

                    ramPct = (int)((total - libre) / total * 100);
                }
            }
            catch { }

            int tempMB = 0;

            try
            {
                string temp = System.IO.Path.GetTempPath();
                long bytes = 0;

                foreach (var f in System.IO.Directory.GetFiles(
                    temp,
                    "*",
                    System.IO.SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        bytes += new System.IO.FileInfo(f).Length;
                    }
                    catch { }
                }

                tempMB = (int)(bytes / (1024 * 1024));
            }
            catch { }

            return (ramPct, tempMB);
        }
    }
}