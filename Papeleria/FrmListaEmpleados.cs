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
    public partial class FrmListaEmpleados : Form, IFrmLista
    {
        DaoEmpleado daoEmpleado;
        List<Empleado> listaElementos;
        Boolean banderaUltimaPagina = false;

        public FrmListaEmpleados()
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
            daoEmpleado = new DaoEmpleado();

            List<Object> listaPaginacion = new List<Object>();

            try
            {
                Conexion.abrirConexion();
                listaElementos = daoEmpleado.buscar(busqueda);
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
                    Empleado obj = new Empleado();

                    obj.Clave = listaElementos[j].Clave;
                    obj.CURP = listaElementos[j].CURP;
                    obj.Nombre = listaElementos[j].Nombre;
                    obj.Apellidos = listaElementos[j].Apellidos;
                    obj.Direccion = listaElementos[j].Direccion;

                    listaPaginacion.Add(obj);
                }

                tbEmpleados.Columns.Clear();
                tbEmpleados.Refresh();
                tbEmpleados.DataSource = listaPaginacion;

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
                tbEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                tbEmpleados.Columns[0].Visible = false;
                tbEmpleados.Columns[1].Visible = false;
                tbEmpleados.Columns[5].Visible = false;
                tbEmpleados.Columns[6].Visible = false;
                tbEmpleados.Columns[7].Visible = false;
                tbEmpleados.Columns[8].Visible = false;
                tbEmpleados.Columns[9].HeaderText = "Dirección";
                tbEmpleados.Columns[10].Visible = false;
                tbEmpleados.Columns[11].Visible = false;
                tbEmpleados.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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


        private void btnAgregar_Click(object sender, EventArgs e)
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
            if (tbEmpleados.SelectedRows.Count > 0)
            {

                DialogResult resultado = MessageBox.Show("¿Estás seguro de eliminar el empleado?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (resultado == DialogResult.Yes)
                {

                    Conexion.abrirConexion();
                    daoEmpleado = new DaoEmpleado();

                    if (daoEmpleado.eliminar(Convert.ToInt32(tbEmpleados.SelectedRows[0].Cells["Clave"].Value)))
                    {
                        MessageBox.Show("¡El empleado ha sido correctamente eliminado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        cargarDatos();
                    }
                    else
                    {
                        String error = Environment.NewLine + "-> Hay ventas relacionadas con " + tbEmpleados.SelectedRows[0].Cells["Nombre"].Value;
                        MessageBox.Show("¡No se ha podido realizar la operación!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            FrmRegEmpleado objEmpleado;

            if (operacion != "Agregar")
            {
                if (tbEmpleados.SelectedRows.Count == 0)
                {
                    MessageBox.Show("¡Debes seleccionar una fila!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    objEmpleado = new FrmRegEmpleado(operacion, Convert.ToInt32(tbEmpleados.SelectedRows[0].Cells["Clave"].Value));
                    objEmpleado.ShowDialog();
                }
            }
            else
            {
                objEmpleado = new FrmRegEmpleado(operacion);
                objEmpleado.ShowDialog();
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
            //auxPaginaAnterior = (Convert.ToInt32(lblTotalPaginas.Text)-1).ToString();
            banderaUltimaPagina = true;
            algoritmoBusqueda(txtBusqueda.Text.Trim(), Convert.ToInt32(lblTotalPaginas.Text));
            banderaUltimaPagina = false;
        }


        private void btnAgregar_Click_2(object sender, EventArgs e)
        {
            agregar();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            editar();
        }

        private void btnMostrar_Click_1(object sender, EventArgs e)
        {
            mostrar();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            eliminar();
        }
    }
}
