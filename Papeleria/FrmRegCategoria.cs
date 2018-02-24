using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using datos;
using modelo;

namespace Papeleria
{
    public partial class FrmRegCategoria : Form,IFrmReg
    {
        private DaoCategoria daoCategoria;
        private String operacion;
        private int id;
        private Categoria elemento;

        public FrmRegCategoria(String operacion)
        {
            inicializar(operacion);
        }

        public FrmRegCategoria(String operacion, int id)
        {
            inicializar(operacion);
            this.id = id;
            llenarControles();
        }

        private void inicializar(String operacion)
        {
            InitializeComponent();
            this.operacion = operacion;
            lblTitulo.Text = operacion + " categoría";
            configurarVisibilidadYEdicion();

            //Traer último est_clave cuando es Agregar
            if (operacion == "Agregar")
            {
                daoCategoria = new DaoCategoria();
                this.id = daoCategoria.consultarUltimo();
                txtClave.Text = id.ToString();
            }
        }

        public void guardar()
        {
            if (esCorrecto())
            {
                try
                {
                    Conexion.abrirConexion();
                    daoCategoria = new DaoCategoria();

                    if (operacion.Equals("Agregar"))
                    {

                        //Saber si ya hay un municipio igual en la base de datos

                        if (daoCategoria.consultarRepetido(txtNombre.Text.Trim()))
                        {
                            Conexion.cerrarConexion();
                            MessageBox.Show("¡" + txtNombre.Text.Trim() + " ya existe en la base de datos!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {

                            llenarModelo();

                            if (daoCategoria.agregar(elemento))
                            {
                                MessageBox.Show("¡La categoría ha sido correctamente guardada!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("¡Ha ocurrido un error al intentar guardar!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                    }
                    else
                    {

                        llenarModelo();

                        if (daoCategoria.editar(elemento))
                        {
                            MessageBox.Show("¡La categoría ha sido correctamente guardada!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("¡Ha ocurrido un error al intentar guardar!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    Conexion.cerrarConexion();

                    actualizarTabla();
                }
                catch (Exception e)
                {
                    Conexion.cerrarConexion();
                }
            }
        }

        public bool esCorrecto()
        {
            String mensaje = "";

            if (txtNombre.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Nombre";
            }

            if (mensaje != "")
            {
                MessageBox.Show("Se debe proporcionar la siguiente información para continuar con la operación: " + mensaje, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void llenarModelo()
        {
            elemento = new Categoria();

            elemento.Clave = Convert.ToInt32(txtClave.Text.Trim());
            elemento.Nombre = txtNombre.Text.Trim();
            elemento.Descripcion = txtDescripcion.Text.Trim();
        }

        public void llenarControles()
        {
            daoCategoria = new DaoCategoria();

            Conexion.abrirConexion();

            elemento = daoCategoria.consultarUno(id);

            Conexion.cerrarConexion();

            if (elemento != null)
            {
                txtClave.Text = elemento.Clave.ToString();
                txtNombre.Text = elemento.Nombre.ToString();
                txtDescripcion.Text = elemento.Descripcion.ToString();
            }
            else
            {
                MessageBox.Show("¡Ha ocurrido un error al cargar la información!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        public void configurarVisibilidadYEdicion()
        {
            if (operacion.Equals("Mostrar"))
            {
                txtNombre.Enabled = false;
                txtDescripcion.Enabled = false;
                btnGuardar.Visible = false;
            }
        }

        public void actualizarTabla()
        {
            //Actualizar grid de datos
            FrmListaCategorias frmCategorias = (FrmListaCategorias)Application.OpenForms["FrmListaCategorias"];
            frmCategorias.cargarDatos();
            frmCategorias.txtBusqueda.Text = "";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Verificar si no escribió números
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                //Si es true, no deja escribirlo en el textbox
                e.Handled = true;
            }

            if (e.KeyChar == 32)
            {
                e.Handled = false;
            }
        }

        private void txtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Verificar si no escribió números
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                //Si es true, no deja escribirlo en el textbox
                e.Handled = true;
            }

            if (e.KeyChar == 32)
            {
                e.Handled = false;
            }
        }

        private void FrmRegCategoria_Load(object sender, EventArgs e)
        {

        }
    }
}
