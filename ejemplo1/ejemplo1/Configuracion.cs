using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejemplo1
{
    class Configuracion
    {
        private List<vehiculo> listaEspera;
        private List<Linea> lineas;
        private Poblacion poblacion;
        private Poblacion mejorPoblacion;
        private int numGenerations = 10;
        private Random rnd = new Random();

        /************
        Constructor *
        ************/
        public Configuracion(List<vehiculo> pListaEspera, List<Linea> pLineas)
        {
            this.listaEspera = pListaEspera;
            this.lineas      = pLineas;
        }
        public Configuracion()
        {
            this.listaEspera = new List<vehiculo>();
            this.lineas = new List<Linea>();
        }


        /****************************************
         Agrega una linea a la lista de lineas  *
        ****************************************/
        public void AgregarLinea(int pTiempoAtencion, List<char> pTiposVehiculos, bool pActiva)
        {
            this.lineas.Add(new Linea(pTiempoAtencion, pTiposVehiculos, pActiva));
        }


        /******************************************
         Agrega un vehiculo a la lista de espera  *
        ******************************************/
        public void AgregarVehiculoEnEspera(int pId, char pTipo, int pTiempo)
        {
            this.listaEspera.Add(new vehiculo(pId, pTipo, pTiempo));
        }

        
        /**************************************************************************
        Calcula la probabilidad de un vehiculo de ser escogido con base en        *
        la cantidad de lineas que pueden atender ese vehiculo mas un # aleatorio  *
        entre -10 y 10                                                            *
        **************************************************************************/
        private double GetProbVehiculo(vehiculo v, List<Linea> lineas)
        {
            int countOfLines = lineas.Count;
            double countOfCoincidences = 0.0;
            for (int i = 0; i < countOfLines; i++)
            {
                List<char> tiposVehiculos = lineas.ElementAt(i).getTiposVehiculos();
                for(int j = 0; j < tiposVehiculos.Count; j++)
                {
                    if(tiposVehiculos.ElementAt(j) == v.GetTipo())
                    {
                        countOfCoincidences++;
                    }
                }
            }
            // Aplica lo aleatorio
            int rand   = rnd.Next(-10, 0);
            double res = 100 * (countOfCoincidences / countOfLines) + rand;
            return res;
        }


        /**************************************************************************
        Genera una población apartir de la lista de espera y la lista de lineas   *
        **************************************************************************/
        public void GenerarPoblacionInicial()
        {
            for (int i = 0; i < this.listaEspera.Count; i++)
            {
                double prob = GetProbVehiculo(listaEspera.ElementAt(i), this.lineas);
                listaEspera.ElementAt(i).SetProbAsignado(prob);
            }
            this.poblacion = new Poblacion(this.listaEspera, this.lineas);
        }


        /***************************************
        Ejecuta una mutacion en this.poblacion * 
        ***************************************/
        public void AplicarOperadoresGeneticos()
        {
            // debe limpiar las lineas y volver a las originales 
            // Seleccion
            // cruze
            // mutacion 
        }


        /****************************************************
        Asigna los vehiculos de una poblacion a las lineas  *
        ****************************************************/
        public void AsignarVehiculosALineas(Poblacion poblacion)
        {
            // debe sacar una copia de la lista de lineas para no editar las lineas originales 
            poblacion.GetVehiculos().OrderBy(o => o.GetProbAsignado()).ToList();
        }


        /****************************************************************
        Evalua si la población es buena o no                            *
        Retorna True si la población cumple con las caracteristicas     *
        Retorna False si la población no cumple con las caracteristicas *
        ****************************************************************/
        public bool Fitness(Poblacion poblacion, int generation)
        {
            if (generation == 2)
            {
                return true;
            }
            if (generation == this.numGenerations)
            {
                return true;
            }
            return false;
        }


        /******************************************************
        Inicia el algoritmo genetico con base en la poblacion *
        ******************************************************/
        public void IniciarGenetico()
        {
            Console.WriteLine("genetico iniciado");

            // Si no se ha generado la población 
            if (this.poblacion == null)
            {
                Console.WriteLine("genetico NO iniciado, no hay una población");
                return;
            }

            // Si la población fue generada correctamente
            for(int i = 0; i < this.numGenerations; i++)
            {
                Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");
                Console.WriteLine("GENERACION: " + i);

                // Asigna los vehiculos a las lineas  
                this.AsignarVehiculosALineas(this.poblacion);
                this.PrintPoblacion(this.poblacion);

                // Se calcula el fitness. Si es una solucion valida, debo parar porque ya tenemos solucion 
                bool validSolution = this.Fitness(this.poblacion, i);
                if (validSolution == true)
                {
                    this.mejorPoblacion = this.poblacion;
                    return;
                }

                // Genera una nueva población 
                this.AplicarOperadoresGeneticos();
            }
            Console.WriteLine("genetico terminado");
        }


        






        /**************************
        Retorna la mejor solucion *
        **************************/
        public Poblacion GetMejorPoblacion()
        {
            Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");
            Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");
            Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");
            Console.WriteLine("Retorné la mejor solución");
            this.PrintPoblacion(this.mejorPoblacion);
            return this.poblacion;
        }










        // IMPRIME UNA SOLUCION en consola
        public void PrintPoblacion(Poblacion pob)
        {
            List<vehiculo> listaEsperaTemp = pob.GetVehiculos();
            List<Linea> lineasTemp = pob.GetLineas();
            Console.WriteLine("##############################################################");
            Console.WriteLine("##############################################################");
            Console.WriteLine("LINEAS");
            for (int i = 0; i < lineasTemp.Count; i++)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Linea: " + i);
                Console.WriteLine("Tiempo de atencion: " + lineasTemp.ElementAt(i).GetTiempoAtencion());
                Console.WriteLine("Tiempo restante: "    + lineasTemp.ElementAt(i).GetTiempoRestante());
                Console.WriteLine("¿Está activa?: "      + lineasTemp.ElementAt(i).GetEstaActiva());
                Console.WriteLine("---------------------------------------------------------------");
            }

            Console.WriteLine(""); Console.WriteLine("");
            Console.WriteLine("VEHICULOS");
            for (int i = 0; i < listaEsperaTemp.Count; i++)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Vehiculo: " + listaEsperaTemp.ElementAt(i).GetId());
                Console.WriteLine("Tipo: " + listaEsperaTemp.ElementAt(i).GetTipo());
                Console.WriteLine("Tiempo total: " + listaEsperaTemp.ElementAt(i).GetTiempo());
                Console.WriteLine("Probabilidad: " + listaEsperaTemp.ElementAt(i).GetProbAsignado());
                Linea linea = listaEsperaTemp.ElementAt(i).GetLineaAsignada();
                if(linea != null)
                {
                    Console.WriteLine("Tiempo restante de linea asignada: " + linea.GetTiempoRestante());
                }
                Console.WriteLine("---------------------------------------------------------------");
            }
            Console.WriteLine("##############################################################");
            Console.WriteLine("##############################################################");
        }


    }
}