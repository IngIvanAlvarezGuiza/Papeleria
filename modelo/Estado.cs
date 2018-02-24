using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo
{
    public class Estado
    {
        private int _clave;
        private String _nombre;

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
    }
}
