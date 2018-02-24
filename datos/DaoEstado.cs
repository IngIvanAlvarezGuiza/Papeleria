using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using modelo;
using System.Windows.Forms;

namespace datos
{
    public class DaoEstado:IDao<Estado>
    {
        public List<Estado> consultarTodos()
        {

            List<Estado> listaEstados = new List<Estado>();

            try
            {
                String sql = "SELECT * FROM estado;";

                DataTable dtEstados = Conexion.ejecutarSQLSelect(sql);

                foreach(DataRow item in dtEstados.Rows){
                    Estado objEstado = new Estado();

                    objEstado.Clave = Convert.ToInt32(item["est_clave"]);
                    objEstado.Nombre = item["est_nombre"].ToString();

                    listaEstados.Add(objEstado);
                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaEstados;
        }

        public Estado consultarUno(int id)
        {
            String sql = "SELECT * FROM estado WHERE est_clave = " + id;
            Estado objEstado = null;

            try
            {
                DataTable dtEstado = Conexion.ejecutarSQLSelect(sql);

                if (dtEstado != null)
                {
                    if (dtEstado.Rows.Count > 0)
                    {
                        objEstado = new Estado();

                        objEstado.Clave = Convert.ToInt32(dtEstado.Rows[0][0]);
                        objEstado.Nombre = dtEstado.Rows[0][1].ToString();
                    }

                }
            }catch(Exception e){

            }

            return objEstado;
        }

        public int consultarUltimo()
        {
            int clave = 0;

            try
            {
                String sql = "SELECT max(est_clave) + 1 FROM estado";
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

        public bool agregar(Estado objEstado)
        {

            String sql = "INSERT INTO estado (est_clave,est_nombre) VALUES" +
                            "(" + objEstado.Clave + ",'" + objEstado.Nombre + "')";

            return Conexion.ejecutarSQL(sql);
            
        }

        public bool editar(Estado objEstado)
        {
            String sql = "UPDATE estado set est_nombre = '" + objEstado.Nombre + "'" + " where est_clave = " +objEstado.Clave;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(int id)
        {
            String sql = "DELETE FROM estado WHERE est_clave = " + id;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(string id)
        {
            throw new NotImplementedException();
        }




        public List<Estado> buscar(String busqueda)
        {
            List<Estado> listaEstados = new List<Estado>();

            try
            {
                String sql = "SELECT * FROM estado where est_nombre like '%" + busqueda + "%'";

                DataTable dtEstados = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtEstados.Rows)
                {
                    Estado objEstado = new Estado();

                    objEstado.Clave = Convert.ToInt32(item["est_clave"]);
                    objEstado.Nombre = item["est_nombre"].ToString();

                    listaEstados.Add(objEstado);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaEstados;
        }


        public bool consultarRepetido(string nombre)
        {
            try
            {
                String sql = "SELECT * FROM estado where est_nombre = '" + nombre + "'";

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
    }
}
