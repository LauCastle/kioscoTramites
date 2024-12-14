using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kioscoTramites
{
    public partial class Form1 : Form
    {
        private SqlConnection conexion;
        private string connectionString = "Server=localhost;Database=KioscoColima;Trusted_Connection=True;";

        public Form1()
        {
            InitializeComponent();
            ConfigurarConexion();
            ConfigurarInterfaz();
            CargarTramites();
        }

        private void ConfigurarConexion()
        {
            try
            {
                conexion = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar la conexión: " + ex.Message);
            }
        }

        private void ConfigurarInterfaz()
        {
            // Configurar el formulario
            this.Text = "Kiosco de Trámites - Colima";
            this.Width = 800;
            this.Height = 600;

            // Crear DataGridView para mostrar trámites
            DataGridView dgvTramites = new DataGridView();
            dgvTramites.Name = "dgvTramites";
            dgvTramites.Dock = DockStyle.Bottom;
            dgvTramites.Height = 300;
            this.Controls.Add(dgvTramites);

            // Panel para los controles
            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Height = 250;
            this.Controls.Add(panel);

            // Etiquetas y campos de texto
            Label lblNombre = new Label();
            lblNombre.Text = "Nombre del Trámite:";
            lblNombre.Location = new System.Drawing.Point(20, 20);
            panel.Controls.Add(lblNombre);

            TextBox txtNombre = new TextBox();
            txtNombre.Name = "txtNombre";
            txtNombre.Location = new System.Drawing.Point(150, 20);
            txtNombre.Width = 200;
            panel.Controls.Add(txtNombre);

            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Descripción:";
            lblDescripcion.Location = new System.Drawing.Point(20, 50);
            panel.Controls.Add(lblDescripcion);

            TextBox txtDescripcion = new TextBox();
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Location = new System.Drawing.Point(150, 50);
            txtDescripcion.Width = 200;
            txtDescripcion.Multiline = true;
            txtDescripcion.Height = 60;
            panel.Controls.Add(txtDescripcion);

            Label lblPrecio = new Label();
            lblPrecio.Text = "Precio:";
            lblPrecio.Location = new System.Drawing.Point(20, 120);
            panel.Controls.Add(lblPrecio);

            TextBox txtPrecio = new TextBox();
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Location = new System.Drawing.Point(150, 120);
            txtPrecio.Width = 100;
            panel.Controls.Add(txtPrecio);

            // Botones
            Button btnAgregar = new Button();
            btnAgregar.Text = "Agregar Trámite";
            btnAgregar.Location = new System.Drawing.Point(20, 160);
            btnAgregar.Click += new EventHandler(BtnAgregar_Click);
            panel.Controls.Add(btnAgregar);

            Button btnActualizar = new Button();
            btnActualizar.Text = "Actualizar";
            btnActualizar.Location = new System.Drawing.Point(150, 160);
            btnActualizar.Click += new EventHandler(BtnActualizar_Click);
            panel.Controls.Add(btnActualizar);

            Button btnEliminar = new Button();
            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new System.Drawing.Point(280, 160);
            btnEliminar.Click += new EventHandler(BtnEliminar_Click);
            panel.Controls.Add(btnEliminar);
        }

        private void CargarTramites()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Id, Nombre, Descripcion, Precio, FechaCreacion FROM Tramites";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    DataGridView dgv = (DataGridView)this.Controls["dgvTramites"];
                    dgv.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los trámites: " + ex.Message);
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox txtNombre = (TextBox)this.Controls.Find("txtNombre", true)[0];
                TextBox txtDescripcion = (TextBox)this.Controls.Find("txtDescripcion", true)[0];
                TextBox txtPrecio = (TextBox)this.Controls.Find("txtPrecio", true)[0];

                if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtPrecio.Text))
                {
                    MessageBox.Show("Por favor complete los campos requeridos.");
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un valor numérico válido.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Tramites (Nombre, Descripcion, Precio) VALUES (@Nombre, @Descripcion, @Precio)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("@Precio", precio);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Trámite agregado exitosamente.");
                LimpiarCampos();
                CargarTramites();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el trámite: " + ex.Message);
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)this.Controls["dgvTramites"];
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Por favor seleccione un trámite para actualizar.");
                    return;
                }

                TextBox txtNombre = (TextBox)this.Controls.Find("txtNombre", true)[0];
                TextBox txtDescripcion = (TextBox)this.Controls.Find("txtDescripcion", true)[0];
                TextBox txtPrecio = (TextBox)this.Controls.Find("txtPrecio", true)[0];

                int id = Convert.ToInt32(dgv.CurrentRow.Cells["Id"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Tramites SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Trámite actualizado exitosamente.");
                LimpiarCampos();
                CargarTramites();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el trámite: " + ex.Message);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)this.Controls["dgvTramites"];
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Por favor seleccione un trámite para eliminar.");
                    return;
                }

                if (MessageBox.Show("¿Está seguro de que desea eliminar este trámite?", "Confirmar eliminación",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                int id = Convert.ToInt32(dgv.CurrentRow.Cells["Id"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Tramites WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Trámite eliminado exitosamente.");
                LimpiarCampos();
                CargarTramites();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el trámite: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            TextBox txtNombre = (TextBox)this.Controls.Find("txtNombre", true)[0];
            TextBox txtDescripcion = (TextBox)this.Controls.Find("txtDescripcion", true)[0];
            TextBox txtPrecio = (TextBox)this.Controls.Find("txtPrecio", true)[0];

            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecio.Text = "";
        }
    }
}