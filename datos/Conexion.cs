using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace datos
{
    public class Conexion
    {
        static SqlConnection cnn;
        public static void abrirConexion()
        {
            try
            {
                String conexion = "Data Source=IVAN-PC;Initial Catalog=Papeleria;User ID=sa;Password=root";
                cnn = new SqlConnection(conexion);

                cnn.Open();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void cerrarConexion()
        {
            try
            {
                if((cnn != null) && (cnn.State != ConnectionState.Closed)){
                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public static Boolean ejecutarSQL(String sql)
        {
            try
            {
                SqlCommand comando = new SqlCommand();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;
                comando.Connection = cnn;
                comando.ExecuteNonQuery();
                comando.Dispose();

                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public static DataTable ejecutarStoredProcedure(String sql)
        {
            try
            {
                SqlCommand comando = new SqlCommand();
                SqlDataReader dr = null;
                
                comando.CommandText = sql;
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@nombreTabla","estado");
                comando.Parameters.AddWithValue("@busqueda", "");
                comando.Parameters.AddWithValue("@pagina", 2);
                comando.Connection = cnn;
                dr = comando.ExecuteReader();
                comando.Dispose();

                DataTable dt = new DataTable();
                dt.Load(dr);

                return dt;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new DataTable();
            }

           
        }

        public static DataTable ejecutarSQLSelect(String sql)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(sql, cnn);

                DataSet ds = new DataSet();

                adaptador.Fill(ds,"Informacion");

                return ds.Tables["Informacion"];

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new DataTable();
            }
        }
    }
}
