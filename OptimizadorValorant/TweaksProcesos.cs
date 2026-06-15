using System.Diagnostics;

namespace OptimizadorValorant
{
    public static class TweaksProcesos
    {
        // --- Cerrar overlays (SEGURO: se hace ANTES de abrir Valorant) ---
        public static void CerrarOverlays()
        {
            string[] overlays = { "Discord", "Steam", "Overwolf", "GameOverlayUI", "EpicGamesLauncher" };
            foreach (var nombre in overlays)
                foreach (var p in Process.GetProcessesByName(nombre))
                    try { p.Kill(); } catch { }
        }
    }
}