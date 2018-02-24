using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo
{
    public class Asentamiento
    {
        private int _clave, _municipio;
        private String _nombre, _ciudad, _zona, _cp, _tipo;

        public int Clave {
            get { return _clave; }
            set { _clave = value; }
        }

        public String Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        public String Ciudad
        {
            get { return _ciudad; }
            set { _ciudad = value; }
        }
        public String Zona
        {
            get { return _zona; }
            set { _zona = value; }
        }
        public String CP
        {
            get { return _cp; }
            set { _cp = value; }
        }
        public String Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public int Municipio
        {
            get { return _municipio; }
            set { _municipio = value; }
        }
    }
}
