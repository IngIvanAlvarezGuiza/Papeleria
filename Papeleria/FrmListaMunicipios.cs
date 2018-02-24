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
using System.Collections;

namespace Papeleria
{
    public partial class FrmListaMunicipios : Form,IFrmLista
    {
        DaoMunicipio daoMunicipio;
        List<Municipio> listaElementos;
        List<Estado> listaEstados;
        Boolean banderaUltimaPagina = false;

        public FrmListaMunicipios()
        {
            InitializeComponent();
            cargarDatos();
        }

        public void cargarDatos()
        {
            try
            {
                cargarEstados();
                algoritmoBusqueda("", 1);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un errror al cargar los datos");
            }
        }

        private void cargarEstados()
        {
            listaEstados = new List<Estado>();
            DaoEstado daoEstado = new DaoEstado();
            List<Estado> list = new List<Estado>();
           
            Conexion.abrirConexion();
            list = daoEstado.consultarTodos();
            Conexion.cerrarConexion();
            
            foreach(var item in list){
                Estado objEstado = new Estado();
                objEstado.Clave = item.Clave;
                objEstado.Nombre = item.Nombre;
                listaEstados.Add(objEstado);
            }
        }

        public void algoritmoBusqueda(String busqueda, int pagina)
        {
            daoMunicipio = new DaoMunicipio();

            List<Object> listaPaginacion = new List<Object>();

            try
            {
                Conexion.abrirConexion();
                listaElementos = daoMunicipio.buscar(busqueda);
                Conexion.cerrarConexion();

                //Si la consulta tuvo más de un registro, se hace la paginación
                double totalPaginacion = Convert.ToDouble(listaElementos.Count) / 10.0;

                int i = (((pagina) - 1) * (10)) + 1;
                int f = 0;
                int paginaS = pagina + 1;

                totalPaginacion = totalPaginacion + 1;
                String[] auxiliar = totalPaginacion.ToString().Split('.');
                lblTotalPaginas.Text = auxiliar[0];

                if (banderaUltimaPagina)
                {
                    f = listaElementos.Count;
                    banderaUltimaPagina = false;
                }
                else if (listaElementos.Count < 10)
                {
                    f = listaElementos.Count;
                    banderaUltimaPagina = false;
                }
                else
                {
                    f = ((paginaS) - 1) * 10;
                }

                totalPaginacion = totalPaginacion - 1;

                String Estado = "";

                for (int j = i - 1; j < f; j++)
                {
                    Municipio obj = new Municipio();

                    obj.Clave = listaElementos[j].Clave;
                    obj.Nombre = listaElementos[j].Nombre;
                    obj.Estado = listaElementos[j].Estado;

                    foreach(var item in listaEstados){
                        if (item.Clave == obj.Estado)
                        {
                            Estado = item.Nombre;
                            Object objeto = new { obj.Clave, obj.Nombre, Estado };
                            listaPaginacion.Add(objeto);
                            
                        }

                        Estado = "";
                    }
                }

                tbMunicipios.Columns.Clear();
                tbMunicipios.Refresh();
                tbMunicipios.DataSource = listaPaginacion;

                //Total de registros totales en la tabla
                lblTotalRegistros.Text = listaElementos.Count.ToString();
                lblRegistrosActual.Text = listaPaginacion.Count.ToString();

                //Obtener el total de páginas
                if (totalPaginacion == 0)
                {
                    int aux = 1;
                    lblTotalPaginas.Text = aux.ToString();
                }
                else if (totalPaginacion.ToString().Contains('.'))
                {
                    totalPaginacion = totalPaginacion + 1;
                    String[] auxiliar2 = totalPaginacion.ToString().Split('.');
                    lblTotalPaginas.Text = auxiliar2[0];
                }
                else
                {
                    lblTotalPaginas.Text = totalPaginacion.ToString();
                }

                lblPaginasActual.Text = pagina.ToString();

                lblTotalRegistros.Text = listaElementos.Count.ToString();

                if (listaElementos.Count == 0)
                {
                    lblRegistrosActual.Text = "0";
                    lblTotalRegistros.Text = "0";
                    lblPaginasActual.Text = "0";
                    lblTotalPaginas.Text = "0";
                }
                tbMunicipios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tbMunicipios.Columns[0].Visible = false;
            }
            catch (Exception e)
            {

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            agregar();
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtBusqueda.Text.Length) == 0)
            {
                cargarDatos();
            }
            else
            {
                algoritmoBusqueda(txtBusqueda.Text.Trim(), 1);
            }
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
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

        public void agregar()
        {
            muestraFormularioRegistro("Agregar");
        }

        public void editar()
        {
            muestraFormularioRegistro("Editar");
        }

        public void mostrar()
        {
            muestraFormularioRegistro("Mostrar");
        }

        public void eliminar()
        {

            if(tbMunicipios.SelectedRows.Count > 0){

                DialogResult resultado = MessageBox.Show("¿Estás seguro de eliminar el municipio?",this.Text,MessageBoxButtons.YesNo,MessageBoxIcon.Information);

                if(resultado == DialogResult.Yes){

                    Conexion.abrirConexion();
                    daoMunicipio = new DaoMunicipio();

                    if (daoMunicipio.eliminar(Convert.ToInt32(tbMunicipios.SelectedRows[0].Cells["Clave"].Value)))
                    {
                        MessageBox.Show("¡El municipio ha sido correctamente eliminado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        cargarDatos();
                    }
                    else
                    {
                        MessageBox.Show("¡No se ha podido realizar la operación!" , this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    Conexion.cerrarConexion();

                }

            }else{
                MessageBox.Show("¡Debes seleccionar una fila!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtBusqueda.Text = "";
        }

        public void muestraFormularioRegistro(string operacion)
        {
            FrmRegMunicipio objMunicipio = null;

            if(operacion != "Agregar"){

                if (tbMunicipios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debes seleccionar una fila", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }else{
                    objMunicipio = new FrmRegMunicipio(operacion,Convert.ToInt32(tbMunicipios.SelectedRows[0].Cells["Clave"].Value));
                    objMunicipio.ShowDialog();
                }

            }else{
                objMunicipio = new FrmRegMunicipio(operacion);
                objMunicipio.ShowDialog();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            editar();
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            mostrar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (!lblPaginasActual.Text.Equals(""))
            {
                if (Convert.ToInt32(lblPaginasActual.Text) < Convert.ToInt32(lblTotalPaginas.Text))
                {
                    //Si es la última página para ir, ejemplo: 3 a 4
                    if ((Convert.ToInt32(lblPaginasActual.Text) + 1) == Convert.ToInt32(lblTotalPaginas.Text))
                    {
                        banderaUltimaPagina = true;
                        algoritmoBusqueda(txtBusqueda.Text.Trim(), Convert.ToInt32(lblPaginasActual.Text) + 1);
                        banderaUltimaPagina = false;
                    }
                    else
                    {
                        banderaUltimaPagina = false;
                        algoritmoBusqueda(txtBusqueda.Text.Trim(), Convert.ToInt32(lblPaginasActual.Text) + 1);
                    }
                }
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            int paginaA = Convert.ToInt32(lblPaginasActual.Text.ToString()) - 1;
            if (paginaA > 0)
            {
                algoritmoBusqueda(txtBusqueda.Text.Trim(), paginaA);
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            algoritmoBusqueda(txtBusqueda.Text.Trim().ToString(), 1);
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            //auxPaginaAnterior = (Convert.ToInt32(lblTotalPaginas.Text)-1).ToString();
            banderaUltimaPagina = true;
            algoritmoBusqueda(txtBusqueda.Text.Trim(), Convert.ToInt32(lblTotalPaginas.Text));
            banderaUltimaPagina = false;
        }

        private void FrmListaMunicipios_Load(object sender, EventArgs e)
        {

        }
        
    }
}
