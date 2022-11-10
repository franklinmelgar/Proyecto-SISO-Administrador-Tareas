using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_SO_Manejador_Tareas
{
    public class Proceso
    {
        public string codigo;
        public string nombre;
        public int rafaga;
        public int llegada;
        public Proceso(string codigo, string nombre, int rafaga, int llegada)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.rafaga = rafaga;
            this.llegada = llegada;
        }
    }
}
