using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo
{
    public class Empleado
    {
        private int _clave, _asentamiento, _municipio, _estatus, _sexo;
        private String _nombre, _apellidos, _numeroCelular, _calleNumero, _direccion, _curp, _fecha;

        public int Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }


        public int Estatus
        {
            get { return _estatus; }
            set { _estatus = value; }
        }

        public String CURP
        {
            get { return _curp; }
            set { _curp = value; }
        }
        public String Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public String Apellidos
        {
            get { return _apellidos; }
            set { _apellidos = value; }
        }

        public int Sexo
        {
            get { return _sexo; }
            set { _sexo = value; }
        }

        public String Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }
        public String Numero
        {
            get { return _numeroCelular; }
            set { _numeroCelular = value; }
        }

        public String CalleNumero
        {
            get { return _calleNumero; }
            set { _calleNumero = value; }
        }


        public String Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }

        public int Asentamiento
        {
            get { return _asentamiento; }
            set { _asentamiento = value; }
        }
        public int Municipio
        {
            get { return _municipio; }
            set { _municipio = value; }
        }
    }
}
