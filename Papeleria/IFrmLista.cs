﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria
{
    interface IFrmLista
    {
        void agregar();
        void editar();
        void mostrar();
        void eliminar();
        void muestraFormularioRegistro(String operacion);
    }
}
