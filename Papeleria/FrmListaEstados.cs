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
    public partial class FrmListaEstados : Form, IFrmLista
    {
        DaoEstado daoEstado;
        List<Estado> listaElementos;
        Boolean banderaUltimaPagina = false;

        public FrmListaEstados()
        {
            InitializeComponent();
            cargarDatos();
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
            if(tbEstados.SelectedRows.Count > 0){

                DialogResult dialogo = MessageBox.Show("¿Estás seguro de eliminar el estado?",this.Text,MessageBoxButtons.YesNo,MessageBoxIcon.Information);

                if(dialogo == DialogResult.Yes){

                    daoEstado = new DaoEstado();

                    Conexion.abrirConexion();

                    if (daoEstado.eliminar(Convert.ToInt32(tbEstados.SelectedRows[0].Cells["Clave"].Value)))
                    {
                        MessageBox.Show("¡El estado ha sido correctamente eliminado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        cargarDatos();

                    }else{
                        String error = Environment.NewLine + "-> Hay municipios relacionados con " + tbEstados.SelectedRows[0].Cells["Nombre"].Value;
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
            FrmRegEstado objEstado;

            if (operacion != "Agregar")
            {
                if(tbEstados.SelectedRows.Count == 0)
                {
                    MessageBox.Show("¡Debes seleccionar una fila!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    objEstado = new FrmRegEstado(operacion,Convert.ToInt32(tbEstados.SelectedRows[0].Cells["Clave"].Value));
                    objEstado.ShowDialog();
                }
            }
            else
            {
                objEstado = new FrmRegEstado(operacion); 
                objEstado.ShowDialog();
            }
        }

        public void cargarDatos()
        {
            try
            {
                algoritmoBusqueda("", 1);
            }
            catch(Exception e)
            {
                MessageBox.Show("Ha ocurrido un errror al cargar los datos");
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


        public void algoritmoBusqueda(String busqueda, int pagina)
        {
            daoEstado = new DaoEstado();
            List<Estado> listaPaginacion = new List<Estado>();

            try
            {
                
                Conexion.abrirConexion();
                listaElementos = daoEstado.buscar(busqueda);
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
                else if (listaElementos.Count<10)
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
                    Estado obj = new Estado();
                    obj.Clave = listaElementos[j].Clave;
                    obj.Nombre = listaElementos[j].Nombre;
                    listaPaginacion.Add(obj);
                }

                tbEstados.Columns.Clear();
                tbEstados.Refresh();
                tbEstados.DataSource = listaPaginacion;

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
                tbEstados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tbEstados.Columns[0].Visible = false;
            }catch(Exception e){

            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (!lblPaginasActual.Text.Equals(""))
            {
                if (Convert.ToInt32(lblPaginasActual.Text) < Convert.ToInt32(lblTotalPaginas.Text))
                {
                    if ((Convert.ToInt32(lblPaginasActual.Text)+1) == Convert.ToInt32(lblTotalPaginas.Text))
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
            int paginaA = Convert.ToInt32(lblPaginasActual.Text.ToString())-1;
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
