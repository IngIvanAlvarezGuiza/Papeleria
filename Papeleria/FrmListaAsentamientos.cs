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
    public partial class FrmListaAsentamientos : Form,IFrmLista
    {
        DaoAsentamiento daoAsentamiento;
        List<Asentamiento> listaElementos;
        Boolean banderaUltimaPagina = false;

        public FrmListaAsentamientos()
        {
            InitializeComponent();
            cargarDatos();
        }

        public void cargarDatos()
        {
            try
            {
                algoritmoBusqueda("", 1);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un errror al cargar los datos");
            }
        }

       

        public void algoritmoBusqueda(String busqueda, int pagina)
        {
            daoAsentamiento = new DaoAsentamiento();

            List<Object> listaPaginacion = new List<Object>();

            try
            {
                Conexion.abrirConexion();
                listaElementos = daoAsentamiento.buscar(busqueda);
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

                for (int j = i - 1; j < f; j++)
                {
                    Asentamiento obj = new Asentamiento();

                    obj.Clave = listaElementos[j].Clave;
                    obj.Nombre = listaElementos[j].Nombre;
                    obj.Ciudad = listaElementos[j].Ciudad;
                    obj.Zona = listaElementos[j].Zona;
                    obj.CP = listaElementos[j].CP;
                    obj.Tipo = listaElementos[j].Tipo;

                    listaPaginacion.Add(obj);
                }

                tbAsentamientos.Columns.Clear();
                tbAsentamientos.Refresh();
                tbAsentamientos.DataSource = listaPaginacion;

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
                tbAsentamientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tbAsentamientos.Columns[0].Visible = false;
                tbAsentamientos.Columns[6].Visible = false;
                tbAsentamientos.Columns[4].HeaderText = "Código Postal";
            }
            catch (Exception e)
            {

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            agregar();
        }

        private void FrmListaAsentamientos_Load(object sender, EventArgs e)
        {

        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            algoritmoBusqueda(txtBusqueda.Text.Trim().ToString(), 1);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            int paginaA = Convert.ToInt32(lblPaginasActual.Text.ToString()) - 1;
            if (paginaA > 0)
            {
                algoritmoBusqueda(txtBusqueda.Text.Trim(), paginaA);
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (!lblPaginasActual.Text.Equals(""))
            {
                if (Convert.ToInt32(lblPaginasActual.Text) < Convert.ToInt32(lblTotalPaginas.Text))
                {
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

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            banderaUltimaPagina = true;
            algoritmoBusqueda(txtBusqueda.Text.Trim(), Convert.ToInt32(lblTotalPaginas.Text));
            banderaUltimaPagina = false;
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

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            agregar();
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
            if (tbAsentamientos.SelectedRows.Count > 0)
            {

                DialogResult dialogo = MessageBox.Show("¿Estás seguro de eliminar el asentamiento?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogo == DialogResult.Yes)
                {

                    daoAsentamiento = new DaoAsentamiento();

                    Conexion.abrirConexion();

                    if (daoAsentamiento.eliminar(Convert.ToInt32(tbAsentamientos.SelectedRows[0].Cells["Clave"].Value)))
                    {
                        MessageBox.Show("¡El asentamiento ha sido correctamente eliminado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        cargarDatos();
                    }
                    else
                    {
                        String error = "";
                        error += Environment.NewLine + "-> Hay clientes relacionados con " + tbAsentamientos.SelectedRows[0].Cells["Nombre"].Value;
                        error += Environment.NewLine + "-> Hay empleados relacionados con " + tbAsentamientos.SelectedRows[0].Cells["Nombre"].Value;
                        MessageBox.Show("¡No se ha podido realizar la operación!" + error, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    Conexion.cerrarConexion();
                }
            }
            else
            {
                MessageBox.Show("¡Debes seleccionar una fila!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtBusqueda.Text = "";
        }

        public void muestraFormularioRegistro(string operacion)
        {
            FrmRegAsentamiento objAsentamiento;

            if (operacion != "Agregar")
            {
                if (tbAsentamientos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("¡Debes seleccionar una fila!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    objAsentamiento = new FrmRegAsentamiento(operacion, Convert.ToInt32(tbAsentamientos.SelectedRows[0].Cells["Clave"].Value));
                    objAsentamiento.ShowDialog();
                }
            }
            else
            {
                objAsentamiento = new FrmRegAsentamiento(operacion);
                objAsentamiento.ShowDialog();
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
    }
}
