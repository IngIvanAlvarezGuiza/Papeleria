using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo
{
    public class Municipio
    {
        private int _clave,_estado;
        private String _nombre ;

        public int Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }

        public String Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public int Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }
    }
}
