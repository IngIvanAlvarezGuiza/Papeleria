using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Papeleria
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
           
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            FrmListaCategorias objCategoria = new FrmListaCategorias();
            objCategoria.ShowDialog();
        }

        private void btnMunicipios_Click(object sender, EventArgs e)
        {
            FrmListaMunicipios objMunicipio = new FrmListaMunicipios();
            objMunicipio.ShowDialog();
        }

        private void btnAsentamientos_Click(object sender, EventArgs e)
        {
            FrmListaAsentamientos objAsentamiento = new FrmListaAsentamientos();
            objAsentamiento.ShowDialog();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            FrmListaClientes objCliente = new FrmListaClientes();
            objCliente.ShowDialog();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            FrmListaEmpleados objEmpleado = new FrmListaEmpleados();
            objEmpleado.ShowDialog();
        }

        private void btnEstados_Click(object sender, EventArgs e)
        {
            FrmListaEstados objEstado = new FrmListaEstados();
            objEstado.ShowDialog();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            lblCantidadProductos objVenta = new lblCantidadProductos();
            objVenta.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmLogin obj = new FrmLogin();
            obj.ShowDialog();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            FrmListaProductos objProducto = new FrmListaProductos();
            objProducto.ShowDialog();
        }
    }
}
