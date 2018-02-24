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
    public class DaoCategoria:IDao<Categoria>
    {

        public List<Categoria> consultarTodos()
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            try
            {
                String sql = "SELECT * FROM categoria;";

                DataTable dtCategorias = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtCategorias.Rows)
                {
                    Categoria objCategoria = new Categoria();

                    objCategoria.Clave = Convert.ToInt32(item["cat_clave"]);
                    objCategoria.Nombre = item["cat_nombre"].ToString();

                    listaCategorias.Add(objCategoria);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaCategorias;
        }

        public Categoria consultarUno(int id)
        {
            String sql = "SELECT * FROM categoria WHERE cat_codigo = " + id;
            Categoria objCategoria = null;

            try
            {
                DataTable dtCategoria = Conexion.ejecutarSQLSelect(sql);

                if (dtCategoria != null)
                {
                    if (dtCategoria.Rows.Count > 0)
                    {
                        objCategoria = new Categoria();

                        objCategoria.Clave = Convert.ToInt32(dtCategoria.Rows[0][0]);
                        objCategoria.Nombre = dtCategoria.Rows[0][1].ToString();
                        objCategoria.Descripcion = dtCategoria.Rows[0][2].ToString();
                    }

                }
            }
            catch (Exception e)
            {

            }

            return objCategoria;
        }


        public bool consultarRepetido(string nombre)
        {
            try
            {
                String sql = "SELECT * FROM categoria where cat_nombre = '" + nombre + "'";

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
                String sql = "SELECT max(cat_codigo) + 1 FROM categoria";

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

        public bool agregar(Categoria objCategoria)
        {

            String sql = "INSERT INTO categoria (cat_codigo,cat_nombre,cat_descripcion) VALUES" +
                            "(" + objCategoria.Clave + ",'" + objCategoria.Nombre + "','" + objCategoria.Descripcion + "')";

            return Conexion.ejecutarSQL(sql);
            
        }

        public bool editar(Categoria objCategoria)
        {
            String sql = "UPDATE categoria set cat_nombre = '" + objCategoria.Nombre + "', cat_descripcion = '" + objCategoria.Descripcion + "' where cat_codigo = " +objCategoria.Clave;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(int id)
        {
            String sql = "DELETE FROM categoria WHERE cat_codigo = " + id;

            return Conexion.ejecutarSQL(sql);
        }

        public bool eliminar(string id)
        {
            throw new NotImplementedException();
        }

        public List<Categoria> buscar(string busqueda)
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            try
            {
                String sql = "SELECT * FROM categoria where cat_nombre like '%" + busqueda + "%' or cat_descripcion like '%" + busqueda + "%'";

                DataTable dtCategorias = Conexion.ejecutarSQLSelect(sql);

                foreach (DataRow item in dtCategorias.Rows)
                {
                    Categoria objCategoria = new Categoria();

                    objCategoria.Clave = Convert.ToInt32(item["cat_codigo"]);
                    objCategoria.Nombre = item["cat_nombre"].ToString();
                    objCategoria.Descripcion = item["cat_descripcion"].ToString();

                    listaCategorias.Add(objCategoria);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listaCategorias;
        }
    }
}
