using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modelo;
using System.Data;
using System.Windows.Forms;

namespace datos
{
    public class DaoProducto:IDao<Producto>
    {
        public List<Producto> consultarTodos()
        {
            throw new NotImplementedException();
        }

        public Producto consultarUno(int id)
        {
            throw new NotImplementedException();
        }

        public bool consultarRepetido(string id)
        {
            throw new NotImplementedException();
        }

        public int consultarUltimo()
        {
            throw new NotImplementedException();
        }

        public bool agregar(Producto t)
        {
            throw new NotImplementedException();
        }

        public bool editar(Producto t)
        {
            throw new NotImplementedException();
        }

        public bool eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public bool eliminar(string id)
        {
            throw new NotImplementedException();
        }

        public List<Producto> buscar(string busqueda)
        {
            List<Producto> listaProductos = new List<Producto>();

            try
            {
                String sql = "SELECT * FROM producto where pto_nombre like '%" + busqueda + "%' or pto_precioCompra like '%" + busqueda + "%'  or pto_iva like '%" + busqueda + "%'  or pto_tipoIva like '%" + busqueda + "%'  or pto_utilidad like '%" + busqueda + "%'  or pto_neto like '%" + busqueda + "%'  or pto_precioVenta like '%" + busqueda + "%'  or pto_unidad like '%" + busqueda + "%'  or pto_cantUnidad like '%" + busqueda + "%'   or pto_insuficiencia like '%" + busqueda + "%'   or pto_descripcion like '%" + busqueda + "%'";

                DataTable dtProductos = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtProductos.Rows)
                {
                    Producto objProducto = new Producto();

                    objProducto.Barras = item["pto_barras"].ToString();
                    objProducto.Nombre = item["pto_nombre"].ToString();
                    objProducto.PrecioCompra = Convert.ToDecimal(item["pto_precioCompra"].ToString());
                    objProducto.IVA = Convert.ToDecimal(item["pto_iva"].ToString());
                    //objProducto.TipoIVA = Convert.ToInt32(item["pto_tipoIva"].ToString());
                    objProducto.Utilidad = Convert.ToDecimal(item["pto_utilidad"].ToString());
                    objProducto.PrecioNeto = Convert.ToDecimal(item["pto_neto"].ToString());
                    objProducto.PrecioVenta = Convert.ToDecimal(item["pto_precioVenta"].ToString());
                    objProducto.Unidad = Convert.ToInt32(item["pto_unidad"].ToString());
                    objProducto.CantidadUnidad = Convert.ToDecimal(item["pto_cantUnidad"].ToString());
                    objProducto.Insuficiencia = Convert.ToDecimal(item["pto_insuficiencia"].ToString());
                    objProducto.Descripcion = item["pto_descripcion"].ToString();

                    listaProductos.Add(objProducto);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
            return listaProductos;
        }
    }
}
