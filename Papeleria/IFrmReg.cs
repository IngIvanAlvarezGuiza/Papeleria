using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria
{
    interface IFrmReg
    {
        void guardar();
        Boolean esCorrecto();
        void llenarModelo();
        void llenarControles();
        void configurarVisibilidadYEdicion();
        void actualizarTabla();
    }
}
