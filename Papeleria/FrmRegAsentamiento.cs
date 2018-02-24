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
    public partial class FrmRegAsentamiento : Form,IFrmReg
    {
        private DaoAsentamiento daoAsentamiento;
        private DaoMunicipio daoMunicipio;
        private DaoEstado daoEstado;
        private String operacion;
        private int id;
        private Asentamiento elemento;
        int txtCiudadLength = 0;
        int txtNombreLength = 0;

        public FrmRegAsentamiento(String operacion)
        {
            inicializar(operacion);
        }

        public FrmRegAsentamiento(String operacion, int id)
        {
            inicializar(operacion);
            this.id = id;
            llenarControles();
        }

        private void inicializar(String operacion)
        {
            InitializeComponent();
            this.operacion = operacion;
            lblTitulo.Text = operacion + " asentamiento";
            configurarVisibilidadYEdicion();

            //Traer último est_clave cuando es Agregar
            if (operacion == "Agregar")
            {
                daoAsentamiento = new DaoAsentamiento();
                this.id = daoAsentamiento.consultarUltimo();
                txtClave.Text = id.ToString();
            }

            cargarZonas();
            cargarCiudades();
            cargarCP();
            cargarTipos();
            cargarEstados();
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
        private void cargarCiudades()
        {
            List<Asentamiento> listaCiudades = new List<Asentamiento>();

            try
            {
                Conexion.abrirConexion();

                DaoAsentamiento daoAsentamiento = new DaoAsentamiento();
                listaCiudades = daoAsentamiento.consultarTodosPorCiudad();

                Conexion.cerrarConexion();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();

                foreach (var item in listaCiudades)
                {
                    coleccion.Add(item.Ciudad);
                }

                //txtCiudad.AutoCompleteCustomSource = coleccion;
                
                
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
                        MessageBox.Show("¡" + estado + " no tiene municipios!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    cmbMunicipio.SelectedIndex = 0;
                    

                }
                catch (Exception e)
                {

                }
            }
        }

        private void cargarCP()
        {
            List<Asentamiento> listaCP = new List<Asentamiento>();

            try
            {
                Conexion.abrirConexion();

                DaoAsentamiento daoAsentamiento = new DaoAsentamiento();
                listaCP = daoAsentamiento.consultarTodosPorCP();

                Conexion.cerrarConexion();
                txtCP.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();

                foreach (var item in listaCP)
                {
                    coleccion.Add(item.CP);
                }

                txtCP.AutoCompleteCustomSource = coleccion;
            }
            catch (Exception e)
            {

            }
        }
        private void cargarZonas()
        {
            List<Asentamiento> listaZonas = new List<Asentamiento>();

            try
            {
                Conexion.abrirConexion();

                DaoAsentamiento daoAsentamiento = new DaoAsentamiento();
                listaZonas = daoAsentamiento.consultarTodosPorZona();

                Conexion.cerrarConexion();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();

                foreach (var item in listaZonas)
                {
                    coleccion.Add(item.Zona);
                }

                txtZona.AutoCompleteCustomSource = coleccion;
            }
            catch (Exception e)
            {

            }
        }

        private void cargarTipos()
        {
            List<Asentamiento> listaTipos = new List<Asentamiento>();

            try
            {
                Conexion.abrirConexion();

                DaoAsentamiento daoAsentamiento = new DaoAsentamiento();
                listaTipos = daoAsentamiento.consultarTodosPorTipo();

                Conexion.cerrarConexion();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();

                foreach (var item in listaTipos)
                {
                    coleccion.Add(item.Tipo);
                }

                txtTipo.AutoCompleteCustomSource = coleccion;
            }
            catch (Exception e)
            {

            }
        }

        public void guardar()
        {
            if (esCorrecto())
            {
                try
                {

                    daoAsentamiento = new DaoAsentamiento();

                    if (operacion.Equals("Agregar"))
                    {

                        //Saber si ya hay un asentamiento igual en la base de datos

                        if (daoAsentamiento.consultarRepetido(txtNombre.Text.Trim()))
                        {
                            Conexion.cerrarConexion();
                            MessageBox.Show("¡" + txtNombre.Text.Trim() + " ya existe en la base de datos!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {

                            llenarModelo();
                            Conexion.abrirConexion();

                            if (daoAsentamiento.agregar(elemento))
                            {
                                MessageBox.Show("¡El asentamiento ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (daoAsentamiento.editar(elemento))
                        {
                            MessageBox.Show("¡El asentamiento ha sido correctamente guardado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (txtCiudad.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Ciudad";
            }

            if (txtZona.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Zona";
            }

            if (txtCP.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Código Postal";
            }

            if (txtTipo.Text.Trim().Equals(""))
            {
                mensaje += Environment.NewLine + "-> Tipo";
            }

            if(cmbEstado.SelectedIndex == 0 || cmbEstado.Items.Count == 0){
                mensaje += Environment.NewLine + "-> Estado";
            }

            if (cmbMunicipio.SelectedIndex == 0 || cmbMunicipio.Items.Count == 0)
            {
                mensaje += Environment.NewLine + "-> Municipio";
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
            Conexion.abrirConexion();
            //Obtener cte_municipio
            Object oMunicipio = cmbMunicipio.SelectedItem;
            int mun_clave = daoMunicipio.consultarUno(oMunicipio.ToString());
            Conexion.cerrarConexion();

            elemento = new Asentamiento();
            elemento.Clave = Convert.ToInt32(txtClave.Text);
            elemento.Nombre = txtNombre.Text;
            elemento.Ciudad = txtCiudad.Text;
            elemento.Zona = txtZona.Text;
            elemento.CP = txtCP.Text;
            elemento.Tipo = txtTipo.Text;
            elemento.Municipio = mun_clave;
        }

        public void llenarControles()
        {
            daoAsentamiento = new DaoAsentamiento();
            Municipio elementoMunicipio = new Municipio();

            Conexion.abrirConexion();

            elemento = daoAsentamiento.consultarUno(id);

            Conexion.cerrarConexion();

            if (elemento != null)
            {
                txtClave.Text = elemento.Clave.ToString();
                txtNombre.Text = elemento.Nombre;
                txtCiudad.Text = elemento.Ciudad;
                txtZona.Text = elemento.Zona;
                txtCP.Text = elemento.CP;
                txtTipo.Text = elemento.Tipo;

                //Obtener el estado y municipio
                daoEstado = new DaoEstado();
                daoMunicipio = new DaoMunicipio();
                daoAsentamiento = new DaoAsentamiento();

                elementoMunicipio = daoMunicipio.consultarUno(Convert.ToInt32(elemento.Municipio));

                cmbEstado.SelectedIndex = elementoMunicipio.Estado;

                cargarMunicipios(Convert.ToInt32(elementoMunicipio.Estado), "", "Editar");

                cmbMunicipio.SelectedItem = elementoMunicipio.Nombre;
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
                txtCiudad.Enabled = false;
                txtZona.Enabled = false;
                txtCP.Enabled = false;
                txtTipo.Enabled = false;
                cmbMunicipio.Enabled = false;
                cmbEstado.Enabled = false;
                btnGuardar.Visible = false;
            }
        }

        public void actualizarTabla()
        {
            //Actualizar grid de datos
            FrmListaAsentamientos frmAsentamientos = (FrmListaAsentamientos)Application.OpenForms["FrmListaAsentamientos"];
            frmAsentamientos.cargarDatos();
            frmAsentamientos.txtBusqueda.Text = "";
        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            int clave = combo.SelectedIndex;
            Object estado = combo.SelectedItem;

            cargarMunicipios(clave, estado.ToString(), "");
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

            if (txtNombreLength > 100)
            {
                MessageBox.Show("¡Límite de lontigud!",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                e.Handled = true;
            }

            txtNombreLength += 1;


        }

        private void txtCiudad_KeyPress(object sender, KeyPressEventArgs e)
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

            if (txtCiudadLength>100)
            {
                MessageBox.Show("¡Límite de lontigud!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Handled = true;
            }

            txtCiudadLength += 1;
        }

        private void txtZona_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTipo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCP_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Verificar si no escribió números
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                //Si es true, no deja escribirlo en el textbox
                e.Handled = true;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void lblZona_Click(object sender, EventArgs e)
        {

        }
    }
}
