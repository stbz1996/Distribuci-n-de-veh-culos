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
        private int numGenerations = 15;
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
            // cruce
            // mutacion 
        }


        /****************************************************
        Asigna los vehiculos de una poblacion a las lineas  *
        ****************************************************/
        public void AsignarVehiculosALineas(Poblacion poblacion)
        {
            // Asigna el valor inicial a cada linea 
            foreach (Linea l in poblacion.GetLineas())
            {
                l.RestablecerTiemporestante();
            }

            // Ordena la lista de menor a mayor segun las probabilidades del vehiculo 
            IEnumerable<vehiculo> sorted   = poblacion.GetVehiculos().OrderBy(x => x.GetProbAsignado());
            List<vehiculo> tempListaEspera = new List<vehiculo>();
            foreach (vehiculo v in sorted)
            {
                tempListaEspera.Add(v);
            }

            for(int i = 0; i < tempListaEspera.Count; i++)
            {
                // Aleatorio segun la probabilidad del vehiculo
                int rand = rnd.Next(0, 101);
                if ((rand < tempListaEspera.ElementAt(i).GetProbAsignado()))
                {
                    vehiculo tempVehiculo = tempListaEspera.ElementAt(i);

                    // Busco las lineas en que puede entrar
                    List<Linea> tempLineas = new List<Linea>();
                    for (int j = 0; j < poblacion.GetLineas().Count; j++)
                    {
                        if (poblacion.GetLineas().ElementAt(j).GetEstaActiva())
                        {
                            List<char> opciones = poblacion.GetLineas().ElementAt(j).getTiposVehiculos();
                            if (opciones.Contains(tempVehiculo.GetTipo()))
                            {
                                if ((poblacion.GetLineas().ElementAt(j).GetTiempoRestante() - tempVehiculo.GetTiempo()) >= 0)
                                {
                                    tempLineas.Add(poblacion.GetLineas().ElementAt(j));
                                }
                            }
                        }
                    }

                    // Lista de lineas donde el vehiculo puede entrar
                    if(tempLineas.Count > 0)
                    {
                        // ver cual está vacia
                        Linea tempMenorLinea = tempLineas.ElementAt(0);
                        bool asignado = false;
                        foreach (Linea l in tempLineas)
                        {
                            if (l.GetTiempoAtencion() == l.GetTiempoRestante())
                            {
                                tempVehiculo.SetLineaAsignada(l);  
                                l.RestarTiempo(tempVehiculo.GetTiempo());
                                l.IncrementarVehiculos();
                                asignado = true;
                                break;
                            }
                            if (tempMenorLinea.GetTiempoRestante() < l.GetTiempoRestante())
                            {
                                tempMenorLinea = l;
                            }
                        }
                        // si no, ver la que tenga menor carga
                        if (asignado == false)
                        {
                            tempVehiculo.SetLineaAsignada(tempMenorLinea); // coloca la linea al vehiculo
                            tempMenorLinea.RestarTiempo(tempVehiculo.GetTiempo()); // resto el tiempo del vehiculo de la linea
                            tempMenorLinea.IncrementarVehiculos();
                        }
                    }
                    
                    /*
                    Console.WriteLine("El vehiculo: " + tempVehiculo.GetId());
                    Console.WriteLine("Puede entrar en las lineas: ");
                    foreach (Linea l in tempLineas)
                    {
                        Console.WriteLine("Tiempo Atención--> " + l.GetTiempoAtencion());
                        Console.WriteLine("Tiempo restante--> " + l.GetTiempoRestante());
                        Console.WriteLine("#");
                    }
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("--------------------------------------");
                    */
                }
            }
        }


        /****************************************************************
        Evalua si la población es buena o no                            *
        Retorna True si la población cumple con las caracteristicas     *
        Retorna False si la población no cumple con las caracteristicas *
        ****************************************************************/
        public bool Fitness(Poblacion poblacion, int generation)
        {
            // Ver si se acepta o no y se le da una puntuación


            /*  
            500  |  300  |  30   | 100 | 50  | Tiempo total    (tt)
            400  |  200  |  0    | 30  | 10  | Tiempo restante (tr)
            110  |  100  |  30   | 70  | 40  | tt - tr
            --------------------------------------------------------
            20%  |  33%  |  100% | 70% | 80% | % de que tan llena quedó la linea 


            CONDICIONES
                - o se acaban los vehiculos
                - Si la linea con menor capacidad es < que 120 
                    - Esa lineas debe estar llena o no deben haber más vehiculos para esa linea
                    - El maximo para las demás lineas es de 120 o el maximo de la linea si es < a 120
                    - El minimo de las demás lineas será la capacidad de la peor linea 
                - Si no
                    - El maximo de las demas lineas será la capacidad de la peor linea mas un rango (20)
        


            OPCION 2 
            - El máximo de asignado a cada linea debe ser igual, mayor o menor a la capacidad
              de la linea más pequeña mas un rango de 20. 
            - Si no
                - Se debe ver si la linea tiene mas de un vehiculo. 
                    - si tiene mas de un vehoculo entonces se rechaza 
            - Si se cumple lo anterior
                - la linea mas pequeña debe estar llena o casi llena (-10 del valor total)
                - Si no
                    - se debe verificar si hay vehiculos que pudieron entrar ahi
                        - si hay vehuclos que pudieron entrar entonces se rechaza 
            - Si se cumple todo lo anterior, es una solución válida.


             */

            if (generation == this.numGenerations - 1)
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