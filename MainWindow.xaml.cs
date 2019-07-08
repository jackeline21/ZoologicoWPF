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

            // Llenar el listbox de Animales
            MostrarAnimales();
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

        private void MostrarAnimalesZoologico()
        {
            try
            {
                // El query a realizar en la BD
                string query = @"SELECT * FROM Zoo.Animal a INNER JOIN Zoo.AnimalZoologico z
                                ON a.id = z.idAnimal WHERE z.idZoologico = @zooId";

                // Commando sql
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Sql Adapter es una interfaz entre las tablas y los objetos utilizables
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    // Remplazar el valor del parámetro del query con su valor correspondiente 
                    sqlCommand.Parameters.AddWithValue("@zooId", Convert.ToInt32(lbZoologicos.SelectedValue));

                    // Objeto en C# que refleja una tabla de una BD
                    DataTable tablaAnimalZoologico = new DataTable();

                    // llenar el objeto de tipo DataTable
                    sqlDataAdapter.Fill(tablaAnimalZoologico);

                    // Mostrar la información que el usuario debe ver
                    lbAnimalesZoologico.DisplayMemberPath = "nombre";
                    // Valor que será entregado cuando un elemento del listbox sea seleccionado
                    lbAnimalesZoologico.SelectedValuePath = "id";

                    // la referencia de los datos del lsitbox
                    lbAnimalesZoologico.ItemsSource = tablaAnimalZoologico.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void LbZoologicos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Morstras los animales de cada zoológico
            MostrarAnimalesZoologico();
        }

        private void MostrarAnimales()
        {
            try
            {
                // eL QUERY QUE SE VA A REALIZAR EN LA BD
                string query = "SELECT * FROM Zoo.Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable tablaAnimal = new DataTable();
                    sqlDataAdapter.Fill(tablaAnimal);

                    lbAnimales.DisplayMemberPath = "nombre";
                    lbAnimales.SelectedValuePath = "id";
                    lbAnimales.ItemsSource = tablaAnimal.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        // Eliminar un zoológico
        private void BtnEliminarZoologico_Click(object sender, RoutedEventArgs e)
        {
            // Controllar si el usuario aún  no ha seleccionado un  zoológico
            if (lbZoologicos.SelectedValue == null)
                MessageBox.Show("Debes Seleccionar un zoológico");
            else
            {
                try
                {
                    // Crear la cadena de conexión 
                    string query = "DELETE FROM Zoo.Zoologico WHERE id = @zooId";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    // Abrir la conexión
                    sqlConnection.Open();

                    // Agregar el parámetro
                    sqlCommand.Parameters.AddWithValue("@zooId", Convert.ToInt32(lbZoologicos.SelectedValue));

                    // Ejecutar un query scalar
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    MostrarZoologicos();
                }
            }
        }

        private void BtnAgregarZoologico_Click(object sender, RoutedEventArgs e)
        {
            // Crear la cadena de conexión 
            string query = "INSERT INTO Zoo.Zoologico(ciudad) VALUES(@ciudad)";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            try
            {
                // Abrir la conexión
                sqlConnection.Open();

                // Reemplazar el parámetro con su valor respectivo
                sqlCommand.Parameters.AddWithValue("@ciudad", txtInformacion.Text);

                // Ejecutar el query de inserción 
                sqlCommand.ExecuteNonQuery();

                // Limpiar el valor del texto 
                txtInformacion.Text = String.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close(); ;
                // Actualizar el listbox de zoologicos para que muestre el nuevo soológico
                MostrarZoologicos();
            }
        }

        private void BtnRemoverAnimalZoo_Click(object sender, RoutedEventArgs e)
        {
            if (lbZoologicos.SelectedItem == null && lbAnimalesZoologico.SelectedItem == null)
                MessageBox.Show("Debes selceccionar el zoológico y el animal que deceas remover");
            else
            {
                try
                {
                    string query = "DELETE FROM Zoo.AnimalZoologico WHERE idZoologico = @idZoologico AND idAnimal = @idAnimal";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    // Abrir la conexión
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@idZoologico", lbZoologicos.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@idAnimal", Convert.ToInt32(lbAnimalesZoologico.SelectedValue));

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    MostrarAnimalesZoologico();
                }
            }

        }

        private void BtnActualizarAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (txtInformacion.Text == string.Empty)
            {
                MessageBox.Show("Debes ingresar el nuevo nombre del animal en la caja de texto");
                txtInformacion.Focus();
            }
            else
            {
                try
                {
                    string query = "UPDATE Zoo.Animal SET nombre = @nombre WHERE id = @idAnimal";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@nombre", txtInformacion.Text);
                    sqlCommand.Parameters.AddWithValue("@idAnimal", Convert.ToInt32(lbAnimalesZoologico.SelectedValue));
                    sqlCommand.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
                finally
                {

                    sqlConnection.Close();
                    MostrarAnimales();
                }
            }


        }

    }
}
