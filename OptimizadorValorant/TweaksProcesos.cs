using System.Diagnostics;

namespace OptimizadorValorant
{
    public static class TweaksProcesos
    {
        // llamarlo antes de abrir valorant
        public static void CerrarOverlays()
        {
            string[] overlays = { "Discord", "Steam", "Overwolf", "GameOverlayUI", "EpicGamesLauncher" };
            foreach (var nombre in overlays)
                foreach (var p in Process.GetProcessesByName(nombre))
                    try { p.Kill(); } catch { }
        }
    }
}