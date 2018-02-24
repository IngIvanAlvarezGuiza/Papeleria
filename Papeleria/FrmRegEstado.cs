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
    public partial class FrmRegEstado : Form,IFrmReg
    {
   
        private DaoEstado daoEstado;
        private String operacion;
        private int id;
        private Estado elemento;

        public FrmRegEstado(String operacion)
        {
            inicializar(operacion);
        }

        private void inicializar(String operacion){
            InitializeComponent();
            this.operacion = operacion;
            lblTitulo.Text = operacion + " estado";
            configurarVisibilidadYEdicion();

            //Traer último est_clave cuando es Agregar
            if (operacion == "Agregar")
            {
                daoEstado = new DaoEstado();
                this.id = daoEstado.consultarUltimo();
                txtClave.Text = id.ToString();
            }
            
        }
 

        public FrmRegEstado(String operacion, int id)
        {
            inicializar(operacion);
            this.id = id;
            llenarControles();
        }

        public void guardar()
        {
            if (esCorrecto())
            {
                try
                {
                    Conexion.abrirConexion();
                    daoEstado = new DaoEstado();

                    if (operacion.Equals("Agregar"))
                    {

                        //Saber si ya hay un municipio igual en la base de datos

                        if (daoEstado.consultarRepetido(txtNombre.Text.Trim()))
                        {
                            Conexion.cerrarConexion();
                            MessageBox.Show("¡" + txtNombre.Text.Trim() + " ya existe en la base de datos!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {

                            llenarModelo();

                            if (daoEstado.agregar(elemento))
                            {
                                MessageBox.Show("¡El estado ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                        if (daoEstado.editar(elemento))
                        {
                            MessageBox.Show("¡El estado ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if(txtNombre.Text.Trim().Equals("")){
                mensaje += Environment.NewLine + "-> Nombre";
            }

            if(mensaje!=""){
                MessageBox.Show("Se debe proporcionar la siguiente información para continuar con la operación: " + mensaje,this.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void llenarModelo()
        {

            elemento = new Estado();

            elemento.Clave = Convert.ToInt32(txtClave.Text.Trim());
            elemento.Nombre = txtNombre.Text.Trim();

        }

        public void llenarControles()
        {
            daoEstado = new DaoEstado();

            Conexion.abrirConexion();

            elemento = daoEstado.consultarUno(id);

            Conexion.cerrarConexion();

            if(elemento != null)
            {
                txtClave.Text = elemento.Clave.ToString();
                txtNombre.Text = elemento.Nombre.ToString();
            }
            else
            {
                MessageBox.Show("¡Ha ocurrido un error al cargar la información!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

        }

        public void configurarVisibilidadYEdicion()
        {
            if(operacion.Equals("Mostrar"))
            {
                txtNombre.Enabled = false;
                btnGuardar.Visible = false;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }


        public void actualizarTabla()
        {
            //Actualizar grid de datos
            FrmListaEstados frmEstados = (FrmListaEstados)Application.OpenForms["FrmListaEstados"];
            frmEstados.cargarDatos();
            frmEstados.txtBusqueda.Text = "";
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

    }
}
