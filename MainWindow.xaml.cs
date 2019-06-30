using System;
using System.Collections.Generic;
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
//Agregar los namespaces necesarios para SQL Server
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace AdministradosZoologicoJacki
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Variable miembro
        SqlConnection sqlConnection;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Coonection String
            string conectionString = ConfigurationManager.ConnectionStrings["AdministradosZoologicoJacki.Properties.Settings.ZoologicoConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(conectionString);

            // Llenar el ListBox de Zoologicos
            MostrarZoologicos();
        }

        // Método para poder llenar el primer ListBox
        private void MostrarZoologicos()
        {
            try
            {
                // El query a realizar en la base de datos
                string query = "SELECT * from Zoo.Zoologico";
                // SQLDataAdapter es una interfaz donde las tablas y los objetos de c#
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    // Objeto en C# que refleja una tabla de una BD
                    DataTable tablaZoologico = new DataTable();

                    // Llenar el objeto de tipo DataTable
                    sqlDataAdapter.Fill(tablaZoologico);

                    // Llenar el ListBox
                    // ¿Cúal información de la tabla en el DataTable debería ser desplegada en el ListBox?
                    lbZoologicos.DisplayMemberPath = "ciudad";
                    // ¿Qué valor debe ser entregado cuando un elemento de nuestro ListBox es seleccionado?
                    lbZoologicos.SelectedValuePath = "id";
                    //¿Quién es la referencia de los datos para el ListBox (popular)?
                    lbZoologicos.ItemsSource = tablaZoologico.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }
        
       
    }
}
