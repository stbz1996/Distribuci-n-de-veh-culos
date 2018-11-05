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
            l2.Add('c');
            List<char> l3 = new List<char>();
            l3.Add('a');
            l3.Add('b');
            l3.Add('c');
            config.AgregarLinea(200, l1, true);
            config.AgregarLinea(100, l2, true);
            config.AgregarLinea(300, l3, true);
            // Creo los vehiculos 
            config.AgregarVehiculoEnEspera(1, 'b', 20);
            config.AgregarVehiculoEnEspera(2, 'a', 30);
            config.AgregarVehiculoEnEspera(3, 'c', 60);
            config.AgregarVehiculoEnEspera(4, 'a', 40);
            config.AgregarVehiculoEnEspera(5, 'a', 60);
            config.AgregarVehiculoEnEspera(6, 'b', 40);
            config.AgregarVehiculoEnEspera(7, 'c', 30);
            config.AgregarVehiculoEnEspera(8, 'c', 20);
            config.AgregarVehiculoEnEspera(9, 'b', 120);
            config.AgregarVehiculoEnEspera(10, 'a', 80);
            config.AgregarVehiculoEnEspera(11, 'a', 100);
            config.AgregarVehiculoEnEspera(12, 'c', 100);

            // Inicio la asignación
            config.GenerarPoblacionInicial();
            config.IniciarGenetico();
            Poblacion poblacion = config.GetMejorPoblacion();

            // Aqui deberia tomar sol y usarlo en la interfaz 




            String fin = Console.ReadLine();
        }
    }
}
