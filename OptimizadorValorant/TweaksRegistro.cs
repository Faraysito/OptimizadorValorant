using Microsoft.Win32;

namespace OptimizadorValorant
{
    public static class TweaksRegistro
    {
        public static void DesactivarAceleracionMouse()
        {
            using var k = Registry.CurrentUser.OpenSubKey(@"Control Panel\Mouse", true);
            k?.SetValue("MouseSpeed", "0"); k?.SetValue("MouseThreshold1", "0"); k?.SetValue("MouseThreshold2", "0");
        }

        public static void DesactivarGameBar()
        {
            using var k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\GameBar");
            k?.SetValue("UseNexusForGameBarEnabled", 0, RegistryValueKind.DWord);
            using var k2 = Registry.CurrentUser.CreateSubKey(@"System\GameConfigStore");
            k2?.SetValue("GameDVR_Enabled", 0, RegistryValueKind.DWord);
        }

        public static void DesactivarTransparencia()
        {
            using var k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            k?.SetValue("EnableTransparency", 0, RegistryValueKind.DWord);
        }

        public static void EfectosVisualesRendimiento()
        {
            using var k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects");
            k?.SetValue("VisualFXSetting", 2, RegistryValueKind.DWord);
        }

        public static void ActivarHAGS()
        {
            using var k = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers");
            k?.SetValue("HwSchMode", 2, RegistryValueKind.DWord);
        }

        public static void PrioridadGraficaValorant()
        {
            string r = @"C:\Riot Games\VALORANT\live\ShooterGame\Binaries\Win64\VALORANT-Win64-Shipping.exe";
            using var k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\DirectX\UserGpuPreferences");
            k?.SetValue(r, "GpuPreference=2;");
        }

        public static void DesactivarOptimizacionPantallaCompleta()
        {
            string r = @"C:\Riot Games\VALORANT\live\ShooterGame\Binaries\Win64\VALORANT-Win64-Shipping.exe";
            using var k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers");
            k?.SetValue(r, "~ DISABLEDXMAXIMIZEDWINDOWEDMODE");
        }

        public static void ActivarAceleracionGPU()
        {
            using var k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Avalon.Graphics");
            k?.SetValue("DisableHWAcceleration", 0, RegistryValueKind.DWord);
        }

        // sin arrastre de ventanas ni animaciones de menú
        public static void BajarGraficos()
        {
            using var k = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop");
            k?.SetValue("DragFullWindows", "0");
            k?.SetValue("UserPreferencesMask", new byte[] { 0x90, 0x12, 0x03, 0x80, 0x10, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
            using var w = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop\WindowMetrics");
            w?.SetValue("MinAnimate", "0");
        }

        public static void DesactivarAppsSegundoPlano()
        {
            using var k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications");
            k?.SetValue("GlobalUserDisabled", 1, RegistryValueKind.DWord);
            using var k2 = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Search");
            k2?.SetValue("BackgroundAppGlobalToggle", 0, RegistryValueKind.DWord);
        }

        // escalado al 100%
        public static void EscaladoOptimo()
        {
            using var k = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop");
            k?.SetValue("LogPixels", 96, RegistryValueKind.DWord); // 96 = 100%
            k?.SetValue("Win8DpiScaling", 1, RegistryValueKind.DWord);
        }
    }
}