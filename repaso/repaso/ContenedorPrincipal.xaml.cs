using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace repaso
{
    /// <summary>
    /// Lógica de interacción para ContenedorPrincipal.xaml
    /// </summary>
    public partial class ContenedorPrincipal : Window
    {
        //conexion linq
        DataClassesPlanillaDataContext dataContext;
        //para quitar el error alt + enter y selecciono using 
        SqlConnection conexion = new SqlConnection("Data Source = (LOCAL)\\SQLEXPRESS;Initial Catalog = PlanillaDePagoMensual; Integrated Security = True");

        //una tabla que utilizaremos
        private DataTable tabla;

        public ContenedorPrincipal()
        {
            InitializeComponent();
            //crear la conexion entre base de datos y linq
            dataContext = new DataClassesPlanillaDataContext(conexion);
            ListarUsuarios();
        }

        private void ListarUsuarios()
        {
            tabla = new DataTable();

            try
            {
                conexion.Open();
                string query = "SELECT * FROM Planilla.Usuario";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                using (adapter)
                {
                    adapter.Fill(tabla);
                    //campo que queres que muestre
                    dgUsuarios.DisplayMemberPath = "Usuarios";

                    //campo que queres que te regrese cuando seleccionas
                    dgUsuarios.SelectedValuePath = "Codigo";
                    dgUsuarios.ItemsSource = tabla.DefaultView;
                    conexion.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //para buscar usuario seleccionado
        private void DgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buscarUsuario();
        }

        //para mostrar los usuarios
        private void buscarUsuario()
        {
            if(dgUsuarios.SelectedValue == null)
            {

            }
            else
            {
                try
                {
                    int id = int.Parse(dgUsuarios.SelectedValue.ToString());

                    var User = (from u in dataContext.Usuario
                               where u.Codigo == id
                               select u).First();

                    txtUsuario.Text = User.Usuario1;
                    txtContra.Text = User.Contraseña;
                }
                catch
                {

                }
            }
        }

        //boton de agregar 
        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataContext.Usuario.InsertOnSubmit(new Usuario { Usuario1 = txtUsuario.Text, Contraseña = txtContra.Text });
                dataContext.SubmitChanges();
                MessageBox.Show("se agrego exitosamente");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ListarUsuarios();
            }
        }
    }
}
