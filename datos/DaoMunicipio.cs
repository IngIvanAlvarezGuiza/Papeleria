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
    public class DaoMunicipio:IDao<Municipio>
    {

        public List<Municipio> consultarTodos()
        {
            List<Municipio> listaMunicipios = new List<Municipio>();

            try
            {
                String sql = "SELECT * FROM municipio order by mun_nombre;";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if(dt != null){
                    if(dt.Rows.Count > 0){
                        foreach (DataRow item in dt.Rows)
                        {
                            Municipio objMunicipio = new Municipio();

                            objMunicipio.Clave = Convert.ToInt32(item["mun_clave"]);
                            objMunicipio.Nombre = item["mun_nombre"].ToString();
                            objMunicipio.Estado = Convert.ToInt32(item["mun_estado"]);

                            listaMunicipios.Add(objMunicipio);
                        }
                    }
                }

            }catch(Exception e){
                MessageBox.Show(e.Message);
            }

            return listaMunicipios;
        }

        public List<Municipio> consultarTodosPorEstado(int id)
        {
            List<Municipio> listaMunicipios = new List<Municipio>();

            try
            {
                String sql = "SELECT * FROM municipio where mun_estado = " + id + " order by mun_nombre;";

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            Municipio objMunicipio = new Municipio();

                            objMunicipio.Clave = Convert.ToInt32(item["mun_clave"]);
                            objMunicipio.Nombre = item["mun_nombre"].ToString();
                            objMunicipio.Estado = Convert.ToInt32(item["mun_estado"]);

                            listaMunicipios.Add(objMunicipio);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaMunicipios;
        }

        public Municipio consultarUno(int id)
        {
            Municipio objMunicipio = new Municipio();

            try
            {
                String sql = "SELECT * FROM municipio where mun_clave = " + id;

                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if(dt != null){
                    if(dt.Rows.Count > 0){
                        objMunicipio.Clave = Convert.ToInt32(dt.Rows[0][0]);
                        objMunicipio.Nombre = dt.Rows[0][1].ToString();
                        objMunicipio.Estado = Convert.ToInt32(dt.Rows[0][2].ToString());
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return objMunicipio;
        }
/*
        public int consultarUnoInt(int id)
        {
            try
            {
                String sql = "SELECT mun_clave FROM municipio where mun_clave = " + id;

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
        }*/
        public int consultarUno(string nombre)
        {
            try
            {
                String sql = "SELECT mun_clave FROM municipio where mun_nombre = '" + nombre + "'";

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

        public int consultarUltimo()
        {
            int clave = 0;

            try
            {
                String sql = "SELECT max(mun_clave) + 1 FROM municipio;";
                DataTable dt = Conexion.ejecutarSQLSelect(sql);

                if (dt.Rows[0][0] != null)
                {
                    clave = Convert.ToInt32(dt.Rows[0][0]);
                }
                else
                {
                    clave = 1;
                }
                return clave;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return clave;
        }

        public bool agregar(Municipio objMunicipio)
        {
            String sql = "INSERT INTO municipio (mun_clave,mun_nombre,mun_estado) values (" + objMunicipio.Clave +
                ",'" + objMunicipio.Nombre + "'," + objMunicipio.Estado + ");";

            return Conexion.ejecutarSQL(sql);
        }

        public bool editar(Municipio objMunicipio)
        {
            String sql = "UPDATE municipio set mun_nombre = '" + objMunicipio.Nombre + "', mun_estado = " + objMunicipio.Estado + " where mun_clave = " + objMunicipio.Clave;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(int id)
        {        
            String sql = "DELETE FROM municipio WHERE mun_clave = " + id;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(string id)
        {
            throw new NotImplementedException();
        }

        public List<Municipio> buscar(string busqueda)
        {
            List<Municipio> listaMunicipios = new List<Municipio>();

            try
            {
                //String sql = "SELECT * FROM municipio where mun_nombre like '%" + busqueda + "%'";
                String sql = "SELECT m.mun_clave as mun_clave,m.mun_nombre as mun_nombre,m.mun_estado as mun_estado,e.est_nombre as est_nombre FROM municipio m join estado e on m.mun_estado = e.est_clave where m.mun_nombre like '%" + busqueda + "%' or e.est_nombre like '%" + busqueda + "%' order by mun_nombre";

                DataTable dtMunicipios = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtMunicipios.Rows)
                {
                    Municipio objMunicipio = new Municipio();

                    objMunicipio.Clave = Convert.ToInt32(item["mun_clave"]);
                    objMunicipio.Nombre = item["mun_nombre"].ToString();
                    objMunicipio.Estado = Convert.ToInt32(item["mun_estado"]);

                    listaMunicipios.Add(objMunicipio);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaMunicipios;
        }

        public Boolean consultarRepetido(string nombre)
        {
            try
            {
                String sql = "SELECT * FROM municipio where mun_nombre = '" + nombre + "'";

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
