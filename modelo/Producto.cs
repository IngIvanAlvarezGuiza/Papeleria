using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo
{
    public class Producto
    {
        private String _barras, _nombre, _descripcion;
        private Decimal _precioCompra, _iva, _utilidad, _precioNeto, _precioVenta, _cantidadUnidad, _insuficiencia;
        private int _tipoIva, _unidad, _categoria;

        public String Barras
        {
            get { return _barras; }
            set { _barras = value; }
        }

        public String Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public Decimal PrecioCompra
        {
            get { return _precioCompra; }
            set { _precioCompra = value; }
        }

        public Decimal IVA
        {
            get { return _iva; }
            set { _iva = value; }
        }

        public int TipoIVA
        {
            get { return _tipoIva; }
            set { _tipoIva = value; }
        }

        public Decimal Utilidad
        {
            get { return _utilidad; }
            set { _utilidad = value; }
        }

        public Decimal PrecioNeto
        {
            get { return _precioNeto; }
            set { _precioNeto = value; }
        }

        public Decimal PrecioVenta
        {
            get { return _precioVenta; }
            set { _precioVenta = value; }
        }
        public int Unidad
        {
            get { return _unidad; }
            set { _unidad = value; }
        }

        public Decimal CantidadUnidad
        {
            get { return _cantidadUnidad; }
            set { _cantidadUnidad = value; }
        }

        public Decimal Insuficiencia
        {
            get { return _insuficiencia; }
            set { _insuficiencia = value; }
        }

        public String Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public int Categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }
    }
}
