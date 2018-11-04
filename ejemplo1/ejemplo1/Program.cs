using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejemplo1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creo la configuración 
            Configuracion config = new Configuracion();

            // Creo las lineas 
            List<char> l1 = new List<char>();
            l1.Add('a');
            l1.Add('b');
            l1.Add('c');
            List<char> l2 = new List<char>();
            l2.Add('a');
            List<char> l3 = new List<char>();
            l3.Add('a');
            l3.Add('b');
            config.AgregarLinea(200, l1, true);
            config.AgregarLinea(100, l2, true);
            config.AgregarLinea(300, l3, true);

            // Creo los vehiculos 
            config.AgregarVehiculoEnEspera(2, 'b', 10);
            config.AgregarVehiculoEnEspera(1, 'a', 10);
            config.AgregarVehiculoEnEspera(3, 'c', 15);
            config.AgregarVehiculoEnEspera(4, 'a', 20);
            config.AgregarVehiculoEnEspera(5, 'a', 30);


            // Inicio la asignación
            config.GenerarPoblacionInicial();
            config.IniciarGenetico();
            Poblacion poblacion = config.GetMejorPoblacion();

            // Aqui deberia tomar sol y usarlo en la interfaz 




            String fin = Console.ReadLine();
        }
    }
}
