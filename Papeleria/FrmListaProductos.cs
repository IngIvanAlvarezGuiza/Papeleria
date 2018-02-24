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
    public partial class FrmListaProductos : Form, IFrmLista
    {
        DaoProducto daoProducto;
        List<Producto> listaElementos;
        Boolean banderaUltimaPagina = false;

        public FrmListaProductos()
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
            daoProducto = new DaoProducto();

            List<Producto> listaPaginacion = new List<Producto>();

            try
            {
                Conexion.abrirConexion();
                listaElementos = daoProducto.buscar(busqueda);
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
                    Producto obj = new Producto();

                    obj.Barras = listaElementos[j].Barras;
                    obj.Nombre = listaElementos[j].Nombre;
                    obj.PrecioCompra = listaElementos[j].PrecioCompra;
                    obj.IVA = listaElementos[j].IVA;
                    obj.TipoIVA = listaElementos[j].TipoIVA;
                    obj.Utilidad = listaElementos[j].Utilidad;
                    obj.PrecioNeto = listaElementos[j].PrecioNeto;
                    obj.PrecioVenta = listaElementos[j].PrecioVenta;
                    obj.Unidad = listaElementos[j].Unidad;
                    obj.CantidadUnidad = listaElementos[j].CantidadUnidad;
                    obj.Insuficiencia = listaElementos[j].Insuficiencia;
                    obj.Descripcion = listaElementos[j].Descripcion;

                    listaPaginacion.Add(obj);
                }

                tbProductos.Columns.Clear();
                tbProductos.Refresh();
                tbProductos.DataSource = listaPaginacion;

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
                tbProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tbProductos.Columns[0].Visible = false;
                tbProductos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                tbProductos.Columns[3].Visible = false;
                tbProductos.Columns[4].Visible = false;
                tbProductos.Columns[5].Visible = false;
                tbProductos.Columns[6].Visible = false;
                tbProductos.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                tbProductos.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                tbProductos.Columns[8].Visible = false;
                tbProductos.Columns[10].Visible = false;
                tbProductos.Columns[11].Visible = false;
                tbProductos.Columns[12].Visible = false;
            }
            catch (Exception e)
            {

            }
        }

        public void agregar()
        {
            throw new NotImplementedException();
        }

        public void editar()
        {
            throw new NotImplementedException();
        }

        public void mostrar()
        {
            throw new NotImplementedException();
        }

        public void eliminar()
        {
            throw new NotImplementedException();
        }

        public void muestraFormularioRegistro(string operacion)
        {
            throw new NotImplementedException();
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
    }
}
