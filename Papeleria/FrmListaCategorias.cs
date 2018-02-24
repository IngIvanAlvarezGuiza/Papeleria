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
    public partial class FrmListaCategorias : Form,IFrmLista
    {
        DaoCategoria daoCategoria;
        List<Categoria> listaElementos;
        Boolean banderaUltimaPagina = false;

        public FrmListaCategorias()
        {
            InitializeComponent();
            cargarDatos();
        }

        public void cargarDatos(){
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
            daoCategoria = new DaoCategoria();

            List<Object> listaPaginacion = new List<Object>();

            try
            {
                Conexion.abrirConexion();
                listaElementos = daoCategoria.buscar(busqueda);
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
                    Categoria obj = new Categoria();

                    obj.Clave = listaElementos[j].Clave;
                    obj.Nombre = listaElementos[j].Nombre;
                    obj.Descripcion = listaElementos[j].Descripcion;

                    listaPaginacion.Add(obj);
                }

                tbCategorias.Columns.Clear();
                tbCategorias.Refresh();
                tbCategorias.DataSource = listaPaginacion;

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
                tbCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tbCategorias.Columns[0].Visible = false;
                tbCategorias.Columns[2].HeaderText = "Descripción";
            }
            catch (Exception e)
            {

            }
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
            if (tbCategorias.SelectedRows.Count > 0)
            {

                DialogResult dialogo = MessageBox.Show("¿Estás seguro de eliminar la categoría?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogo == DialogResult.Yes)
                {

                    daoCategoria = new DaoCategoria();

                    Conexion.abrirConexion();

                    if (daoCategoria.eliminar(Convert.ToInt32(tbCategorias.SelectedRows[0].Cells["Clave"].Value)))
                    {
                        MessageBox.Show("¡La categoría ha sido correctamente eliminada!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        cargarDatos();
                    }
                    else
                    {
                        String error = Environment.NewLine + "-> Hay productos relacionados con " + tbCategorias.SelectedRows[0].Cells["Nombre"].Value;
                        MessageBox.Show("¡No se ha podido realizar la operación!"+error, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            FrmRegCategoria objCategoria;

            if (operacion != "Agregar")
            {
                if (tbCategorias.SelectedRows.Count == 0)
                {
                    MessageBox.Show("¡Debes seleccionar una fila!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    objCategoria = new FrmRegCategoria(operacion, Convert.ToInt32(tbCategorias.SelectedRows[0].Cells["Clave"].Value));
                    objCategoria.ShowDialog();
                }
            }
            else
            {
                objCategoria = new FrmRegCategoria(operacion);
                objCategoria.ShowDialog();
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            agregar();
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
