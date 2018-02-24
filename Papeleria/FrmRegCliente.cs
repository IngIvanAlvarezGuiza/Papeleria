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
    public partial class FrmRegCliente : Form,IFrmReg
    {
        private DaoCliente daoCliente;
        private String operacion;
        private int id;
        private Cliente elemento;
        DaoAsentamiento daoAsentamiento;
        DaoMunicipio daoMunicipio;
        DaoEstado daoEstado;
        String direccion = "";

        public FrmRegCliente(String operacion)
        {
            inicializar(operacion);
        }

        private void inicializar(String operacion)
        {
            InitializeComponent();
            this.operacion = operacion;
            lblTitulo.Text = operacion + " cliente";
            configurarVisibilidadYEdicion();

            //Traer último est_clave cuando es Agregar
            if (operacion == "Agregar")
            {
                daoCliente = new DaoCliente();
                this.id = daoCliente.consultarUltimo();
                txtClave.Text = id.ToString();

                rdActivo.Checked = true;
            }

            cargarEstados();
        }

        public FrmRegCliente(String operacion, int id)
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

                daoEstado = new DaoEstado();
                listaEstados = daoEstado.consultarTodos();
                Conexion.cerrarConexion();

                if (listaEstados.Count > 0)
                {
                    cmbEstado.Items.Add("SELECCIONA UNA OPCIÓN");

                    foreach (var item in listaEstados)
                    {
                        cmbEstado.Items.Add(item.Nombre);
                    }
                }
                else
                {
                    MessageBox.Show("¡No hay estados registrados!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                cmbEstado.SelectedIndex = 0;

            }
            catch (Exception e)
            {

            }
        }

        private void cargarMunicipios(int clave, String estado, String op)
        {
            if (clave > 0)
            {
                List<Municipio> listaMunicipios = new List<Municipio>();

                try
                {
                    Conexion.abrirConexion();

                    daoMunicipio = new DaoMunicipio();
                    listaMunicipios = daoMunicipio.consultarTodosPorEstado(clave);
                    Conexion.cerrarConexion();

                    cmbMunicipio.Items.Clear();
                    cmbMunicipio.Refresh();
                    
                    if (listaMunicipios.Count > 0)
                    {
                        cmbMunicipio.Items.Add("SELECCIONA UNA OPCIÓN");

                        foreach (var item in listaMunicipios)
                        {
                            cmbMunicipio.Items.Add(item.Nombre);
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡" + estado + " no tiene municipios!",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }

                    cmbMunicipio.SelectedIndex = 0;
                    
                }
                catch (Exception e)
                {

                }
            }
        }

        private void cargarAsentamientos(String municipio)
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            if(!municipio.Equals("SELECCIONA UNA OPCIÓN")){
                try
                {
                    Conexion.abrirConexion();

                    daoAsentamiento = new DaoAsentamiento();
                    daoMunicipio = new DaoMunicipio();

                    //Obtener la clave del municipio
                    int clave = daoMunicipio.consultarUno(municipio);

                    listaAsentamientos = daoAsentamiento.consultarTodosPorAsentamiento(clave);
                    Conexion.cerrarConexion();

                    cmbAsentamiento.Items.Clear();
                    cmbAsentamiento.Refresh();

                    if (listaAsentamientos.Count > 0)
                    {
                        cmbAsentamiento.Items.Add("SELECCIONA UNA OPCIÓN");

                        foreach (var item in listaAsentamientos)
                        {
                            cmbAsentamiento.Items.Add(item.Nombre);
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡" + municipio + " no tiene asentamientos!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    cmbAsentamiento.SelectedIndex = 0;

                }
                catch (Exception e)
                {

                }
            }
        }

        

        public void guardar()
        {
            if(esCorrecto()){
                try
                {
                    
                    daoCliente = new DaoCliente();

                    if (operacion.Equals("Agregar"))
                    {

                        //Saber si ya hay un cliente igual en la base de datos

                        if (daoCliente.consultarRepetido(txtNombre.Text.Trim()))
                        {
                            Conexion.cerrarConexion();
                            MessageBox.Show("¡" + txtNombre.Text.Trim() + " ya existe en la base de datos!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {

                            llenarModelo();
                            Conexion.abrirConexion();

                            if (daoCliente.agregar(elemento))
                            {
                                MessageBox.Show("¡El cliente ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        Conexion.abrirConexion();
                        //Conexion.abrirConexion();
                        if (daoCliente.editar(elemento))
                        {
                            MessageBox.Show("¡El cliente ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (!txtRFC.Text.Trim().Equals(""))
            {
                if (txtRFC.Text.Trim().Length < 13)
                {
                    mensaje += Environment.NewLine + "-> RFC: la longitud debe ser igual a 13 caracteres";
                }
            }
            

            if (txtNombre.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Nombre";
            }

            if (txtApellidos.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Apellidos";
            }

            if (txtCalleNumero.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Calle y número";
            }

            if (cmbEstado.SelectedIndex == 0 || cmbEstado.Items.Count == 0)
            {
                mensaje += Environment.NewLine + "-> Estado";
            }

            if (cmbMunicipio.SelectedIndex == 0 || cmbMunicipio.Items.Count == 0)
            {
                mensaje += Environment.NewLine + "-> Municipio";
            }

            if (cmbAsentamiento.SelectedIndex == 0 || cmbAsentamiento.Items.Count == 0)
            {
                mensaje += Environment.NewLine + "-> Asentamiento";
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
            //Obtener dirección completa
            generarDireccion();

            Conexion.abrirConexion();

            //Obtener cte_municipio
            Object oMunicipio = cmbMunicipio.SelectedItem;
            int mun_clave = daoMunicipio.consultarUno(oMunicipio.ToString());

            //Obtener cte_asentamiento
            Asentamiento elementoAsentamiento = new Asentamiento();
            Object oAsentamiento = cmbAsentamiento.SelectedItem;
            elementoAsentamiento = daoAsentamiento.consultarUno(oAsentamiento.ToString());
            Conexion.cerrarConexion();

            elemento = new Cliente();
            elemento.Clave = Convert.ToInt32(txtClave.Text.Trim());

            if(rdActivo.Checked){
                elemento.Estatus = 1;
            }else{
                elemento.Estatus = 0;
            }

            elemento.RFC = txtRFC.Text.Trim();
            elemento.Nombre = txtNombre.Text.Trim();
            elemento.Apellidos = txtApellidos.Text.Trim();
            elemento.Telefono = txtTelefono.Text.Trim();
            elemento.CalleNumero = txtCalleNumero.Text.Trim();
            elemento.Direccion = direccion;
            elemento.Municipio = mun_clave;
            elemento.Asentamiento = elementoAsentamiento.Clave;
        }

        public void llenarControles()
        {
            daoCliente = new DaoCliente();
            daoEstado = new DaoEstado();
            elemento = new Cliente();
            Municipio elementoMunicipio = new Municipio();
            Asentamiento elementoAsentamiento = new Asentamiento();

            Conexion.abrirConexion();
            elemento = daoCliente.consultarUno(id);
            

            if (elemento != null)
            {
                txtClave.Text = elemento.Clave.ToString();
               // elemento.Estatus = Convert.ToChar(tx);
                if (elemento.Estatus == 1)
                {
                    rdActivo.Checked = true;
                }
                else
                {
                    rdInactivo.Checked = true;
                }

                txtRFC.Text = elemento.RFC.ToString();
                txtNombre.Text = elemento.Nombre.ToString();
                txtApellidos.Text = elemento.Apellidos.ToString();
                txtTelefono.Text = elemento.Telefono.ToString();
                txtCalleNumero.Text = elemento.CalleNumero.ToString();

                //Obtener el estado y municipio
                daoEstado = new DaoEstado();
                daoMunicipio = new DaoMunicipio();
                daoAsentamiento = new DaoAsentamiento();

                elementoMunicipio = daoMunicipio.consultarUno(Convert.ToInt32(elemento.Municipio));

                cmbEstado.SelectedIndex = elementoMunicipio.Estado;

                cargarMunicipios(Convert.ToInt32(elementoMunicipio.Estado), "","Editar");

                cmbMunicipio.SelectedItem = elementoMunicipio.Nombre;

                //Obtener el asentamiento
                cargarAsentamientos(elementoMunicipio.Nombre);

                elementoAsentamiento = daoAsentamiento.consultarUno(elemento.Asentamiento);
                cmbAsentamiento.SelectedItem = elementoAsentamiento.Nombre;
            }

            Conexion.cerrarConexion();
        }

        public void configurarVisibilidadYEdicion()
        {
            if (operacion.Equals("Mostrar"))
            {
                txtNombre.Enabled = false;
                rdActivo.Enabled = false;
                rdInactivo.Enabled = false;
                txtRFC.Enabled = false;
                txtApellidos.Enabled = false;
                txtTelefono.Enabled = false;
                txtCalleNumero.Enabled = false;
                cmbAsentamiento.Enabled = false;
                cmbMunicipio.Enabled = false;
                cmbEstado.Enabled = false;
                btnGuardar.Visible = false;
            }
        }
        public void actualizarTabla()
        {
            //Actualizar grid de datos
            FrmListaClientes frmClientes = (FrmListaClientes)Application.OpenForms["FrmListaClientes"];
            frmClientes.cargarDatos();
            frmClientes.txtBusqueda.Text = "";
        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox) sender;

            int clave = combo.SelectedIndex;
            Object estado = combo.SelectedItem;

            cargarMunicipios(clave, estado.ToString(),"");
        }

        private void cmbMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Object municipio = combo.SelectedItem;

            cargarAsentamientos(municipio.ToString());
        }

        private void generarDireccion()
        {
            daoAsentamiento = new DaoAsentamiento();

            Asentamiento objAsentamiento = new Asentamiento();
            Estado objEstado = new Estado();
            Municipio objMunicipio = new Municipio();

            Conexion.abrirConexion();

            //Asentamiento seleccionado
            Object asentamiento = cmbAsentamiento.SelectedItem;
            objAsentamiento = daoAsentamiento.consultarUno(asentamiento.ToString());

            Conexion.cerrarConexion();

            //Estado seleccionado
            Object oEstado = cmbEstado.SelectedItem;

            //Municipio seleccionado
            Object oMunicipio = cmbMunicipio.SelectedItem;

            String calleNumero = txtCalleNumero.Text+" ";
            String tipoAsentamiento = objAsentamiento.Tipo+" ";
            String nombreAsentamiento = objAsentamiento.Nombre+" ";
            String cpAsentamiento = objAsentamiento.CP+" ";
            String municipio = oMunicipio.ToString()+", ";
            String estado = oEstado.ToString();
            direccion = calleNumero + tipoAsentamiento + nombreAsentamiento + cpAsentamiento + municipio + estado;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void txtRFC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
            {
                //Si es true, no deja escribirlo en el textbox
                e.Handled = true;
            }

            if (e.KeyChar == 32)
            {
                e.Handled = false;
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
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

        private void txtApellidos_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                //Si es true, no deja escribirlo en el textbox
                e.Handled = true;
            }

            if (e.KeyChar == 32)
            {
                e.Handled = false;
            }
        }

        private void FrmRegCliente_Load(object sender, EventArgs e)
        {

        }


    }
}
