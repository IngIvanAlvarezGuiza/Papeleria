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
    public class DaoCliente:IDao<Cliente>
    {
        public List<Cliente> consultarTodos()
        {
            throw new NotImplementedException();
        }

        public Cliente consultarUno(int id)
        {
            String sql = "SELECT * FROM cliente WHERE cte_clave = " + id;
            Cliente objCliente = null;

            try
            {
                DataTable dtCliente = Conexion.ejecutarSQLSelect(sql);

                if (dtCliente != null)
                {
                    if (dtCliente.Rows.Count > 0)
                    {
                        objCliente = new Cliente();

                        objCliente.Clave = Convert.ToInt32(dtCliente.Rows[0][0]);
                        objCliente.Estatus = Convert.ToInt32(dtCliente.Rows[0][1]);
                        objCliente.RFC = dtCliente.Rows[0][2].ToString();
                        objCliente.Nombre = dtCliente.Rows[0][3].ToString();
                        objCliente.Apellidos = dtCliente.Rows[0][4].ToString();
                        objCliente.Telefono = dtCliente.Rows[0][5].ToString();
                        objCliente.CalleNumero = dtCliente.Rows[0][6].ToString();
                        objCliente.Asentamiento = Convert.ToInt32(dtCliente.Rows[0][8]);
                        objCliente.Municipio = Convert.ToInt32(dtCliente.Rows[0][9]);
                    }

                }
            }
            catch (Exception e)
            {

            }

            return objCliente;
        }

        public bool consultarRepetido(string nombre)
        {
            try
            {
                String sql = "SELECT * FROM cliente where cte_nombre = '" + nombre + "'";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public int consultarUltimo()
        {
            int clave = 0;

            try
            {
                String sql = "SELECT max(cte_clave) + 1 FROM cliente";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt.Rows[0][0] != null)
                {
                    clave = Convert.ToInt32(dt.Rows[0][0]);
                }
                else
                {
                    clave = 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return clave;
        }

        public bool agregar(Cliente objCliente)
        {
            String sql = "INSERT INTO cliente (cte_clave,cte_estatus,cte_rfc,cte_nombre,cte_apellidos,"+
                "cte_telefono,cte_calleNumero,cte_direccion,cte_asentamiento,cte_municipio)" +
                "VALUES (" + objCliente.Clave + "," + objCliente.Estatus + ",'" + objCliente.RFC + "','"+
                         objCliente.Nombre + "','" + objCliente.Apellidos + "','" + objCliente.Telefono + "','"+
                         objCliente.CalleNumero + "','" + objCliente.Direccion + "'," + objCliente.Asentamiento + ","+
                         objCliente.Municipio + ")";

            return Conexion.ejecutarSQL(sql);
        }

        public bool editar(Cliente objCliente)
        {
            String sql = "UPDATE cliente set cte_estatus = " + objCliente.Estatus + ", cte_rfc = '" + objCliente.RFC + "', " + 
                    "cte_nombre = '" + objCliente.Nombre + "', cte_apellidos = '" + objCliente.Apellidos + "', "+
                    "cte_telefono = '" + objCliente.Telefono + "', cte_calleNumero = '" + objCliente.CalleNumero + "',"+
                    " cte_direccion = '" + objCliente.Direccion + "', cte_asentamiento = " + objCliente.Asentamiento + ", cte_municipio = " +
                      objCliente.Municipio + " where cte_clave = " + objCliente.Clave;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(int id)
        {
            String sql = "DELETE FROM cliente WHERE cte_clave = " + id;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(string id)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> buscar(string busqueda)
        {
            List<Cliente> listaClientes = new List<Cliente>();

            try
            {
                String sql = "SELECT cte_clave,cte_rfc,cte_nombre,cte_apellidos,cte_direccion FROM cliente where cte_rfc like '%" + busqueda + "%' or cte_nombre like '%" + busqueda + "%' or cte_apellidos like '%" + busqueda + "%' or cte_telefono like '%" + busqueda + "%' or cte_calleNumero like '%" + busqueda + "%' or cte_direccion like '%" + busqueda + "%'";

                DataTable dtClientes = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtClientes.Rows)
                {
                    Cliente objCliente = new Cliente();

                    objCliente.Clave = Convert.ToInt32(item["cte_clave"]);
                    //objCliente.Estatus = Convert.ToChar(item["cte_estatus"]);
                    objCliente.RFC = item["cte_rfc"].ToString();
                    objCliente.Nombre = item["cte_nombre"].ToString();
                    objCliente.Apellidos = item["cte_apellidos"].ToString();
                    //objCliente.Telefono = item["cte_telefono"].ToString();
                    //objCliente.CalleNumero = item["cte_calleNumero"].ToString();
                    //objCliente.CP = item["cte_cp"].ToString();
                    objCliente.Direccion = item["cte_direccion"].ToString();
                    //objCliente.Asentamiento = Convert.ToInt32(item["cte_asentamiento"]);
                    //objCliente.Municipio = Convert.ToInt32(item["cte_municipio"]);
                    //objCliente.Estado = Convert.ToInt32(item["cte_estado"]);
                    
                    listaClientes.Add(objCliente);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaClientes;
        }
    }
}
