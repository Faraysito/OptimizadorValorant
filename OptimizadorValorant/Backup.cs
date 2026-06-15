using System;
using System.IO;
using System.Diagnostics;

namespace OptimizadorValorant
{
    public static class Backup
    {
        // Carpeta donde se guardan los backups (en tus Documentos)
        static string CarpetaBackup = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "OptimizadorValorant_Backups");

        public static void CrearBackup()
        {
            Directory.CreateDirectory(CarpetaBackup);
            string sello = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string carpeta = Path.Combine(CarpetaBackup, sello);
            Directory.CreateDirectory(carpeta);

            // 1. Backup de las claves de registro que vamos a tocar
            ExportarRegistro(@"HKEY_CURRENT_USER\Control Panel\Mouse",
                             Path.Combine(carpeta, "mouse.reg"));
            ExportarRegistro(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                             Path.Combine(carpeta, "explorer.reg"));

            // 2. Backup del plan de energía actual
            EjecutarCmd("cmd.exe", $"/c powercfg /getactivescheme > \"{Path.Combine(carpeta, "plan_energia.txt")}\"");

            // 3. Backup de la lista de servicios y su estado
            EjecutarCmd("cmd.exe", $"/c sc query state= all > \"{Path.Combine(carpeta, "servicios.txt")}\"");

            System.Windows.MessageBox.Show("Backup creado en:\n" + carpeta, "Backup");
        }

        static void ExportarRegistro(string clave, string archivo)
        {
            EjecutarCmd("reg", $"export \"{clave}\" \"{archivo}\" /y");
        }

        static void EjecutarCmd(string programa, string argumentos)
        {
            var p = new ProcessStartInfo
            {
                FileName = programa,
                Arguments = argumentos,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(p)?.WaitForExit();
        }
    }
}