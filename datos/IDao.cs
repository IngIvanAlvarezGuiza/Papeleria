using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datos
{
    interface IDao<T>
    {
        List<T> consultarTodos();
        T consultarUno(int id);
        Boolean consultarRepetido(String id);
        int consultarUltimo();
        Boolean agregar(T t);
        Boolean editar(T t);
        Boolean eliminar(int id);
        Boolean eliminar(String id);
        List<T> buscar(String busqueda);
    }
}
