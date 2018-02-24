using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo
{
    public class Cliente
    {
        private int _clave, _asentamiento, _municipio, _estatus;
        private String _nombre, _apellidos, _telefono, _calleNumero, _direccion, _rfc;

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

        public String RFC
        {
            get { return _rfc; }
            set { _rfc = value; }
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

        public String Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
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
