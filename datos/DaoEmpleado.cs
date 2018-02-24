using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modelo;
using System.Windows.Forms;
using System.Data;

namespace datos
{
    public class DaoEmpleado:IDao<Empleado>
    {

        public List<Empleado> consultarTodos()
        {
            throw new NotImplementedException();
        }

        public Empleado consultarUno(int id)
        {
            String sql = "SELECT * FROM empleado WHERE emp_cve = " + id;
            Empleado objEmpleado = null;

            try
            {
                DataTable dtEmpleado = Conexion.ejecutarSQLSelect(sql);

                if (dtEmpleado != null)
                {
                    if (dtEmpleado.Rows.Count > 0)
                    {
                        objEmpleado = new Empleado();

                        objEmpleado.Clave = Convert.ToInt32(dtEmpleado.Rows[0][0]);
                        objEmpleado.Estatus = Convert.ToInt32(dtEmpleado.Rows[0][1]);
                        objEmpleado.CURP = dtEmpleado.Rows[0][2].ToString();
                        objEmpleado.Nombre = dtEmpleado.Rows[0][3].ToString();
                        objEmpleado.Apellidos = dtEmpleado.Rows[0][4].ToString();
                        objEmpleado.Sexo = Convert.ToInt32(dtEmpleado.Rows[0][5].ToString());
                        objEmpleado.Fecha = dtEmpleado.Rows[0][6].ToString();
                        objEmpleado.Numero = dtEmpleado.Rows[0][7].ToString();
                        objEmpleado.CalleNumero = dtEmpleado.Rows[0][8].ToString();
                        objEmpleado.Asentamiento = Convert.ToInt32(dtEmpleado.Rows[0][10]);
                        objEmpleado.Municipio = Convert.ToInt32(dtEmpleado.Rows[0][11]);
                    }

                }
            }
            catch (Exception e)
            {

            }

            return objEmpleado;
        }

        public bool consultarRepetido(string nombre)
        {
            try
            {
                String sql = "SELECT * FROM empleado where emp_nombre = '" + nombre + "'";

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
                String sql = "SELECT max(emp_cve) + 1 FROM empleado";

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

        public bool agregar(Empleado objEmpleado)
        {
            String sql = "INSERT INTO empleado (emp_cve,emp_estatus,emp_curp,emp_nombre,emp_apellidos," +
                "emp_sexo,emp_fechaNacimiento,emp_numeroCelular,emp_calleNumero,emp_direccion,emp_asentamiento,emp_municipio)" +
                "VALUES (" + objEmpleado.Clave + "," + objEmpleado.Estatus + ",'" + objEmpleado.CURP + "','" +
                         objEmpleado.Nombre + "','" + objEmpleado.Apellidos + "',"+ objEmpleado.Sexo +",'" + objEmpleado.Fecha + "','"+
                         objEmpleado.Numero + "','" + objEmpleado.CalleNumero + "','" + objEmpleado.Direccion + "'," + objEmpleado.Asentamiento + "," +
                         objEmpleado.Municipio + ")";

            return Conexion.ejecutarSQL(sql);
        }

        public bool editar(Empleado objEmpleado)
        {
            String sql = "UPDATE empleado set emp_estatus = " + objEmpleado.Estatus + ", emp_curp = '" + objEmpleado.CURP + "', " +
                    "emp_nombre = '" + objEmpleado.Nombre + "', emp_apellidos = '" + objEmpleado.Apellidos + "', " +
                    "emp_sexo = " + objEmpleado.Sexo + ", emp_fechaNacimiento = '" + objEmpleado.Fecha + "', "+
                    "emp_numeroCelular = '" + objEmpleado.Numero + "', emp_calleNumero = '" + objEmpleado.CalleNumero + "'," +
                    " emp_direccion = '" + objEmpleado.Direccion + "', emp_asentamiento = " + objEmpleado.Asentamiento + ", emp_municipio = " +
                      objEmpleado.Municipio + " where emp_cve = " + objEmpleado.Clave;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(int id)
        {
            String sql = "DELETE FROM empleado WHERE emp_cve = " + id;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(string id)
        {
            throw new NotImplementedException();
        }

        public List<Empleado> buscar(string busqueda)
        {
            List<Empleado> listaEmpleados = new List<Empleado>();

            try
            {
                String sql = "SELECT emp_cve,emp_curp,emp_nombre,emp_apellidos,emp_direccion FROM empleado where emp_curp like '%" + busqueda + "%' or emp_nombre like '%" + busqueda + "%' or emp_apellidos like '%" + busqueda + "%' or emp_numeroCelular like '%" + busqueda + "%' or emp_calleNumero like '%" + busqueda + "%' or emp_direccion like '%" + busqueda + "%'";

                DataTable dtEmpleados = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtEmpleados.Rows)
                {
                    Empleado objEmpleado = new Empleado();

                    objEmpleado.Clave = Convert.ToInt32(item["emp_cve"]);
                    //objCliente.Estatus = Convert.ToChar(item["cte_estatus"]);
                    objEmpleado.CURP = item["emp_curp"].ToString();
                    objEmpleado.Nombre = item["emp_nombre"].ToString();
                    objEmpleado.Apellidos = item["emp_apellidos"].ToString();
                    //objCliente.Telefono = item["cte_telefono"].ToString();
                    //objCliente.CalleNumero = item["cte_calleNumero"].ToString();
                    //objCliente.CP = item["cte_cp"].ToString();
                    objEmpleado.Direccion = item["emp_direccion"].ToString();
                    //objCliente.Asentamiento = Convert.ToInt32(item["cte_asentamiento"]);
                    //objCliente.Municipio = Convert.ToInt32(item["cte_municipio"]);
                    //objCliente.Estado = Convert.ToInt32(item["cte_estado"]);

                    listaEmpleados.Add(objEmpleado);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaEmpleados;
        }
    }
}
