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
    public class DaoAsentamiento:IDao<Asentamiento>
    {

        public List<Asentamiento> consultarTodos()
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            try
            {
                String sql = "SELECT * FROM asentamiento;";

                DataTable dtAsentamientos = Conexion.ejecutarSQLSelect(sql);

                foreach(DataRow item in dtAsentamientos.Rows){
                    Asentamiento objAsentamiento = new Asentamiento();

                    objAsentamiento.Clave = Convert.ToInt32(item["ase_clave"]);
                    objAsentamiento.Nombre = item["ase_nombre"].ToString();

                    listaAsentamientos.Add(objAsentamiento);
                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaAsentamientos;
        }

        public List<Asentamiento> consultarTodosPorAsentamiento(int id)
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            try
            {
                String sql = "SELECT * FROM asentamiento where ase_mun = " + id + " order by ase_nombre;";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            Asentamiento objAsentamiento = new Asentamiento();

                            objAsentamiento.Clave = Convert.ToInt32(item["ase_clave"]);
                            objAsentamiento.Nombre = item["ase_nombre"].ToString();

                            listaAsentamientos.Add(objAsentamiento);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaAsentamientos;
        }

        public List<Asentamiento> consultarTodosPorZona()
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            try
            {
                String sql = "SELECT DISTINCT ase_zona FROM asentamiento where ase_zona not like 'null' ORDER BY ase_zona;";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            Asentamiento objAsentamiento = new Asentamiento();

                            objAsentamiento.Zona = item["ase_zona"].ToString();

                            listaAsentamientos.Add(objAsentamiento);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaAsentamientos;
        }

        public List<Asentamiento> consultarTodosPorCiudad()
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            try
            {
                String sql = "SELECT DISTINCT ase_ciudad FROM asentamiento where ase_ciudad not like 'null' ORDER BY ase_ciudad;";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            Asentamiento objAsentamiento = new Asentamiento();

                            objAsentamiento.Ciudad = item["ase_ciudad"].ToString();

                            listaAsentamientos.Add(objAsentamiento);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaAsentamientos;
        }

        public List<Asentamiento> consultarTodosPorCP()
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            try
            {
                String sql = "SELECT DISTINCT ase_cp FROM asentamiento where ase_cp not like 'null' ORDER BY ase_cp;";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            Asentamiento objAsentamiento = new Asentamiento();

                            objAsentamiento.CP = item["ase_cp"].ToString();

                            listaAsentamientos.Add(objAsentamiento);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaAsentamientos;
        }

        public List<Asentamiento> consultarTodosPorTipo()
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            try
            {
                String sql = "SELECT DISTINCT ase_tipo FROM asentamiento where ase_tipo not like 'null' ORDER BY ase_tipo;";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            Asentamiento objAsentamiento = new Asentamiento();

                            objAsentamiento.Tipo = item["ase_tipo"].ToString();

                            listaAsentamientos.Add(objAsentamiento);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaAsentamientos;
        }

        public Asentamiento consultarUno(int id)
        {
            Asentamiento objAsentamiento = new Asentamiento();

            try
            {
                String sql = "SELECT * FROM asentamiento where ase_clave = " + id;

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        objAsentamiento.Clave = Convert.ToInt32(dt.Rows[0][0]);
                        objAsentamiento.Nombre = dt.Rows[0][1].ToString();
                        objAsentamiento.Ciudad = dt.Rows[0][2].ToString();
                        objAsentamiento.Zona = dt.Rows[0][3].ToString();
                        objAsentamiento.CP = dt.Rows[0][4].ToString();
                        objAsentamiento.Tipo = dt.Rows[0][5].ToString();
                        objAsentamiento.Municipio = Convert.ToInt32(dt.Rows[0][6]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return objAsentamiento;
        }

        public Asentamiento consultarUno(string nombre)
        {
            String sql = "SELECT * FROM asentamiento WHERE ase_nombre = '" + nombre + "'";
            Asentamiento objAsentamiento = null;

            try
            {
                DataTable dtAsentamiento = Conexion.ejecutarSQLSelect(sql);

                if (dtAsentamiento != null)
                {
                    if (dtAsentamiento.Rows.Count > 0)
                    {
                        objAsentamiento = new Asentamiento();

                        objAsentamiento.Clave = Convert.ToInt32(dtAsentamiento.Rows[0][0]);
                        objAsentamiento.Nombre = dtAsentamiento.Rows[0][1].ToString();
                        objAsentamiento.CP = dtAsentamiento.Rows[0][4].ToString();
                        objAsentamiento.Tipo = dtAsentamiento.Rows[0][5].ToString();
                    }

                }
            }
            catch (Exception e)
            {

            }

            return objAsentamiento;
        }



        /*
        public int consultarUnoInt(string nombre)
        {
            try
            {
                String sql = "SELECT ase_clave FROM asentamiento where ase_nombre = '" + nombre + "'";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        */
        public int consultarUltimo()
        {
            int clave = 0;

            try
            {
                String sql = "SELECT max(ase_clave) + 1 FROM asentamiento";

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

        public bool agregar(Asentamiento objAsentamiento)
        {
            String sql = "INSERT INTO asentamiento (ase_clave,ase_nombre,ase_ciudad,ase_zona,ase_cp,ase_tipo,ase_mun) VALUES" +
                            "(" + objAsentamiento.Clave + ",'" + objAsentamiento.Nombre + "','" + objAsentamiento.Ciudad + "','"+
                            objAsentamiento.Zona + "','" + objAsentamiento.CP +"','" + objAsentamiento.Tipo + "'," + objAsentamiento.Municipio + ")";

            return Conexion.ejecutarSQL(sql);
        }

        public bool editar(Asentamiento objAsentamiento)
        {
            String sql = "UPDATE asentamiento set ase_nombre = '" + objAsentamiento.Nombre + "', ase_ciudad = '" + objAsentamiento.Ciudad + "', ase_zona = '" + objAsentamiento.Zona + "', ase_cp = '" + objAsentamiento.CP + "',ase_tipo = '" + objAsentamiento.Tipo + "',ase_mun = " + objAsentamiento.Municipio + " where ase_clave = " + objAsentamiento.Clave;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(int id)
        {
            String sql = "DELETE FROM asentamiento WHERE ase_clave = " + id;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(string id)
        {
            throw new NotImplementedException();
        }

        public List<Asentamiento> buscar(string busqueda)
        {
            List<Asentamiento> listaAsentamientos = new List<Asentamiento>();

            try
            {
                String sql = "SELECT * FROM asentamiento where ase_nombre like '%" + busqueda + "%' or ase_ciudad like '%" + busqueda + "%' or ase_zona like '%" + busqueda + "%' or ase_cp like '%" + busqueda + "%' or ase_tipo like '%" + busqueda + "%'";

                DataTable dtAsentamientos = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtAsentamientos.Rows)
                {
                    Asentamiento objAsentamiento = new Asentamiento();

                    objAsentamiento.Clave = Convert.ToInt32(item["ase_clave"]);
                    objAsentamiento.Nombre = item["ase_nombre"].ToString();
                    objAsentamiento.Ciudad = item["ase_ciudad"].ToString();
                    objAsentamiento.Zona = item["ase_zona"].ToString();
                    objAsentamiento.CP = item["ase_cp"].ToString();
                    objAsentamiento.Tipo = item["ase_tipo"].ToString();

                    listaAsentamientos.Add(objAsentamiento);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaAsentamientos;
        }


        public bool consultarRepetido(string nombre)
        {
            try
            {
                String sql = "SELECT * FROM asentamiento where ase_nombre = '" + nombre + "'";

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
