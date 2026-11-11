using System.Windows;
using System.Windows.Controls;

namespace OptimizadorValorant
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ActualizarEstado();
        }

        // barra de título
        void Min_Click(object s, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        void Max_Click(object s, RoutedEventArgs e) =>
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        void Close_Click(object s, RoutedEventArgs e) => Close();

        // navegación
        void Nav_Checked(object sender, RoutedEventArgs e)
        {
            if (pageInicio == null) return;
            pageInicio.Visibility = Visibility.Collapsed;
            pageGraficos.Visibility = Visibility.Collapsed;
            pageSistema.Visibility = Visibility.Collapsed;
            pageMonitoreo.Visibility = Visibility.Collapsed;
            pagePerfiles.Visibility = Visibility.Collapsed;

            if (navInicio.IsChecked == true) pageInicio.Visibility = Visibility.Visible;
            else if (navGraficos.IsChecked == true) pageGraficos.Visibility = Visibility.Visible;
            else if (navSistema.IsChecked == true) pageSistema.Visibility = Visibility.Visible;
            else if (navMonitoreo.IsChecked == true) pageMonitoreo.Visibility = Visibility.Visible;
            else if (navPerfiles.IsChecked == true) pagePerfiles.Visibility = Visibility.Visible;
        }

        void ActualizarEstado()
        {
            var (ram, temp) = Monitor.EstadoRapido();
            lblRam.Text = ram + "%";
            lblTemp.Text = temp + " MB";
        }

        void OptimizarTodo_Click(object s, RoutedEventArgs e)
        {
            TweaksRegistro.DesactivarAceleracionMouse();
            TweaksRegistro.DesactivarGameBar();
            TweaksRegistro.DesactivarTransparencia();
            TweaksRegistro.EfectosVisualesRendimiento();
            TweaksRegistro.DesactivarAppsSegundoPlano();
            TweaksSistema.PlanMaximoRendimiento();
            TweaksSistema.LimpiarTemporales();
            TweaksProcesos.CerrarOverlays();
            ActualizarEstado();
            MessageBox.Show("Optimizacion rapida aplicada.", "Listo");
        }

        void Backup_Click(object s, RoutedEventArgs e) => Backup.CrearBackup();

        void Mouse_Click(object s, RoutedEventArgs e) { TweaksRegistro.DesactivarAceleracionMouse(); Aviso("Mouse: aceleracion desactivada."); }
        void GameBar_Click(object s, RoutedEventArgs e) { TweaksRegistro.DesactivarGameBar(); Aviso("Game Bar desactivada."); }
        void Transp_Click(object s, RoutedEventArgs e) { TweaksRegistro.DesactivarTransparencia(); Aviso("Transparencia desactivada."); }
        void Efectos_Click(object s, RoutedEventArgs e) { TweaksRegistro.EfectosVisualesRendimiento(); Aviso("Efectos visuales: rendimiento."); }
        void BajarGraf_Click(object s, RoutedEventArgs e) { TweaksRegistro.BajarGraficos(); Aviso("Graficos del PC bajados."); }
        void AcelGpu_Click(object s, RoutedEventArgs e) { TweaksRegistro.ActivarAceleracionGPU(); Aviso("Aceleracion de GPU activada."); }
        void Hags_Click(object s, RoutedEventArgs e) { TweaksRegistro.ActivarHAGS(); Aviso("HAGS activado. Reinicia Windows."); }
        void PrioGpu_Click(object s, RoutedEventArgs e) { TweaksRegistro.PrioridadGraficaValorant(); Aviso("Prioridad grafica alta a Valorant."); }
        void PantallaCompleta_Click(object s, RoutedEventArgs e) { TweaksRegistro.DesactivarOptimizacionPantallaCompleta(); Aviso("Optim. pantalla completa desactivada."); }
        void Apps2do_Click(object s, RoutedEventArgs e) { TweaksRegistro.DesactivarAppsSegundoPlano(); Aviso("Apps en 2do plano desactivadas."); }
        void Escalado_Click(object s, RoutedEventArgs e) { TweaksRegistro.EscaladoOptimo(); Aviso("Escalado en 100%. Cierra sesion para aplicar."); }

        void Energia_Click(object s, RoutedEventArgs e) { TweaksSistema.PlanMaximoRendimiento(); Aviso("Plan de energia: maximo."); }
        void Hpet_Click(object s, RoutedEventArgs e) { TweaksSistema.DesactivarHPET(); Aviso("HPET desactivado. Reinicia Windows."); }
        void Temp_Click(object s, RoutedEventArgs e) { TweaksSistema.LimpiarTemporales(); ActualizarEstado(); Aviso("Temporales limpiados."); }
        void Pagina_Click(object s, RoutedEventArgs e) { TweaksSistema.AjustarPaginacion(8192); Aviso("Paginacion 8GB. Reinicia Windows."); }
        void Overlays_Click(object s, RoutedEventArgs e) { TweaksProcesos.CerrarOverlays(); Aviso("Overlays cerrados."); }

        void VerInicio_Click(object s, RoutedEventArgs e) => Salida.Text = TweaksSistema.ListarProgramasInicio();
        void QuitarInicio_Click(object s, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NombreInicio.Text))
            { TweaksSistema.DesactivarProgramaInicio(NombreInicio.Text.Trim()); Aviso($"Inicio '{NombreInicio.Text.Trim()}' desactivado (si estaba en Run)."); }
        }

        void VerTemp_Click(object s, RoutedEventArgs e) => Salida.Text = Monitor.LeerTemperaturas();
        void VerDriver_Click(object s, RoutedEventArgs e) => Salida.Text = Monitor.VersionDriverGPU();
        void VerCuello_Click(object s, RoutedEventArgs e) => Salida.Text = Monitor.DetectarCuelloBotella();

        void GuardarPerfil_Click(object s, RoutedEventArgs e)
        {
            var p = new Perfil
            {
                Nombre = NombrePerfil.Text.Trim(),
                DesactivarMouseAccel = cbMouse.IsChecked == true,
                DesactivarGameBar = cbGameBar.IsChecked == true,
                DesactivarTransparencia = cbTransp.IsChecked == true,
                PlanMaximoRendimiento = cbEnergia.IsChecked == true,
                LimpiarTemporales = cbTemp.IsChecked == true
            };
            Perfiles.Guardar(p);
            MessageBox.Show($"Perfil '{p.Nombre}' guardado.", "Perfiles");
        }

        Perfil perfilCargado;
        void CargarPerfil_Click(object s, RoutedEventArgs e)
        {
            perfilCargado = Perfiles.Cargar(NombrePerfil.Text.Trim());
            cbMouse.IsChecked = perfilCargado.DesactivarMouseAccel;
            cbGameBar.IsChecked = perfilCargado.DesactivarGameBar;
            cbTransp.IsChecked = perfilCargado.DesactivarTransparencia;
            cbEnergia.IsChecked = perfilCargado.PlanMaximoRendimiento;
            cbTemp.IsChecked = perfilCargado.LimpiarTemporales;
            MessageBox.Show($"Perfil '{perfilCargado.Nombre}' cargado.", "Perfiles");
        }

        void AplicarPerfil_Click(object s, RoutedEventArgs e)
        {
            if (perfilCargado == null) { MessageBox.Show("Primero carga un perfil."); return; }
            if (perfilCargado.DesactivarMouseAccel) TweaksRegistro.DesactivarAceleracionMouse();
            if (perfilCargado.DesactivarGameBar) TweaksRegistro.DesactivarGameBar();
            if (perfilCargado.DesactivarTransparencia) TweaksRegistro.DesactivarTransparencia();
            if (perfilCargado.PlanMaximoRendimiento) TweaksSistema.PlanMaximoRendimiento();
            if (perfilCargado.LimpiarTemporales) TweaksSistema.LimpiarTemporales();
            MessageBox.Show($"Perfil '{perfilCargado.Nombre}' aplicado.", "Perfiles");
        }

        void Aviso(string t) { if (Salida != null) Salida.Text = t; }
    }
}