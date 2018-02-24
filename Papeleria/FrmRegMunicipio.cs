using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using modelo;
using datos;

namespace Papeleria
{
    public partial class FrmRegMunicipio : Form,IFrmReg
    {
        private int id;
        private String operacion;
        DaoMunicipio daoMunicipio;
        Municipio elemento;

        public FrmRegMunicipio(String operacion)
        {
            inicializar(operacion);
        }

        private void inicializar(String operacion){
            InitializeComponent();
            this.operacion = operacion;
            configurarVisibilidadYEdicion();
            lblTitulo.Text = operacion + " municipio";
            
            if (operacion == "Agregar")
            {
                daoMunicipio = new DaoMunicipio();
                this.id = daoMunicipio.consultarUltimo();
                txtClave.Text = id.ToString();
            }

            cargarEstados();
        }

        public FrmRegMunicipio(String operacion, int id)
        {
            inicializar(operacion);
            this.id = id;
            llenarControles();
        }

        private void cargarEstados()
        {
            List<Estado> listaEstados = new List<Estado>();

            try
            {
                Conexion.abrirConexion();

                DaoEstado daoEstado = new DaoEstado();
                daoEstado = new DaoEstado();
                listaEstados = daoEstado.consultarTodos();

                Conexion.cerrarConexion();

                cmbEstado.Items.Add("SELECCIONA UNA OPCIÓN");

                foreach(var item in listaEstados){
                    cmbEstado.Items.Add(item.Nombre);
                }

                cmbEstado.SelectedIndex = 0;
            }
            catch(Exception e)
            {

            }
        }

       

        public void guardar()
        {
            if(esCorrecto()){
             //   try
               // {
                    Conexion.abrirConexion();
                    daoMunicipio = new DaoMunicipio();

                    if(operacion.Equals("Agregar")){

                        //Saber si ya hay un municipio igual en la base de datos

                        if (daoMunicipio.consultarRepetido(txtNombre.Text.Trim()))
                        {
                            Conexion.cerrarConexion();
                            MessageBox.Show("¡" + txtNombre.Text.Trim() + " ya existe en la base de datos!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            
                            llenarModelo();

                            if(daoMunicipio.agregar(elemento)){
                                MessageBox.Show("¡El municipio ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }else{
                                MessageBox.Show("¡Ha ocurrido un error al intentar guardar!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                    }else{

                        llenarModelo();

                        if (daoMunicipio.editar(elemento))
                        {
                            MessageBox.Show("¡El municipio ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            /*  catch(Exception e)
              {
                  Conexion.cerrarConexion();
              }
          }*/
        }

        public bool esCorrecto()
        {
            String mensaje = "";

            if(txtNombre.Text.Trim().Equals("")){
                mensaje += Environment.NewLine + "-> Nombre";
            }
            if(cmbEstado.SelectedIndex == 0){
                mensaje += Environment.NewLine + "-> Estado";
            }

            if(!mensaje.Equals("")){

                MessageBox.Show("Se debe proporcionar la siguiente información para continuar con la operación: " + mensaje, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }else{
                return true;
            }
        }

        public void llenarModelo()
        {
            elemento = new Municipio();

            elemento.Clave = Convert.ToInt32(txtClave.Text);
            elemento.Nombre = txtNombre.Text;
            elemento.Estado = cmbEstado.SelectedIndex;
        }

        public void llenarControles()
        {
            daoMunicipio = new DaoMunicipio();
            Conexion.abrirConexion();
            elemento = new Municipio();
            elemento = daoMunicipio.consultarUno(id);
            Conexion.cerrarConexion();

            if (elemento != null)
            {
                txtClave.Text = elemento.Clave.ToString();
                txtNombre.Text = elemento.Nombre;
                cmbEstado.SelectedIndex = elemento.Estado;
            }
        }

        public void configurarVisibilidadYEdicion()
        {
            if(operacion=="Mostrar"){
                txtNombre.Enabled = false;
                btnGuardar.Visible = false;
                cmbEstado.Enabled = false;
            }
        }

        public void actualizarTabla()
        {
            //Actualizar grid de datos
            FrmListaMunicipios frmMunicipios = (FrmListaMunicipios)Application.OpenForms["FrmListaMunicipios"];
            frmMunicipios.cargarDatos();
            frmMunicipios.txtBusqueda.Text = "";
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar)){
                e.Handled = true;
            }
            if(e.KeyChar == 32){
                e.Handled = false;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }
    }
}
