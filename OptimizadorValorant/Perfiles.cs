using System.IO;
using System.Text.Json;

namespace OptimizadorValorant
{
    // Esta clase representa qué optimizaciones están activadas en un perfil
    public class Perfil
    {
        public string Nombre { get; set; } = "Default";
        public bool DesactivarMouseAccel { get; set; }
        public bool DesactivarGameBar { get; set; }
        public bool DesactivarTransparencia { get; set; }
        public bool EfectosVisualesRendimiento { get; set; }
        public bool PlanMaximoRendimiento { get; set; }
        public bool LimpiarTemporales { get; set; }
        public bool CerrarOverlays { get; set; }
        public bool ActivarHAGS { get; set; }
        // Podés agregar más opciones acá en el futuro
    }

    public static class Perfiles
    {
        static string Carpeta = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
            "OptimizadorValorant_Perfiles");

        // Guardar un perfil en un archivo JSON
        public static void Guardar(Perfil p)
        {
            Directory.CreateDirectory(Carpeta);
            string json = JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(Carpeta, p.Nombre + ".json"), json);
        }

        // Cargar un perfil desde un archivo JSON
        public static Perfil Cargar(string nombre)
        {
            string ruta = Path.Combine(Carpeta, nombre + ".json");
            if (!File.Exists(ruta)) return new Perfil();
            return JsonSerializer.Deserialize<Perfil>(File.ReadAllText(ruta)) ?? new Perfil();
        }
    }
}