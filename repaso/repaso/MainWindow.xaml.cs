using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace repaso
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClassesPlanillaDataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();
            //el string de conexion
            //en aap.config esta el nombre que va entre [] lo que esta en comillas lo copio y pego//
            string connectionString = ConfigurationManager.ConnectionStrings["repaso.Properties.Settings.PlanillaDePagoMensualConnectionString"].ConnectionString;
            //si tira error se arregla en la bombilla system.configuration
            
            //conexion de linq con el string de conexion
            dataContext = new DataClassesPlanillaDataContext(connectionString);

        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text != "" ||  pwbContra.Password != "")
            {
                //evitar errores que no se cierre el programa al haberlos
                try
                {
                    //TE LLENA UNA LISTA CON LOS DATOS DE ADENTRO
                    //busca el usuario
                    var User = (from u in dataContext.Usuario where u.Usuario1 == txtUser.Text select u).ToList();
                    //busca la contraseña
                    var Pass = (from u in dataContext.Usuario where u.Contraseña == txtUser.Text select u).ToList();
                    //valida que esten llenos y vexistan
                    if (User.Count > 0 && Pass.Count > 0)
                    {
                        //abre la pantalla principal
                        ContenedorPrincipal ventana = new ContenedorPrincipal();
                        ventana.Show();//muestra el lobby
                        this.Close();//cierra el loggin
                    }
                    else
                    {
                        MessageBox.Show("los datos son incorrectos");
                    }
                }
                catch (Exception ex)
                {
                    //para que muestre el error
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                    //validara esten llenos los campos
                    MessageBox.Show("debe validar todos los campos esten llenos");

            }
            
        }
    }
}
