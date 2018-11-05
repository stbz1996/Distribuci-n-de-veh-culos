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
        Busca la linea con menor capacidad                  *
        *****************************************************/
        private Linea GetLineaConMenoCapacidad()
        {
            IEnumerable<Linea> sorted = this.poblacion.GetLineas().OrderBy(x => (x.GetTiempoAtencion() - x.GetTiempoRestante()));
            List<Linea> temp = new List<Linea>();
            return sorted.ElementAt(0);
        }


        /****************************************************
        Obtiene el tiempo de atencion menor en las lineas   *
        ****************************************************/
        private int GetTiempoatencionLineaMasPequena()
        {
            IEnumerable<Linea> sorted = this.poblacion.GetLineas().OrderBy(x => x.GetTiempoAtencion());
            List<Linea> temp = new List<Linea>();
            return sorted.ElementAt(0).GetTiempoAtencion();
        }


        /****************************************************
        Busca si alguna linea se pasa del rango             *
        ****************************************************/
        private bool LineasFueraDeRango(int rango)
        {
            foreach (Linea l in this.poblacion.GetLineas())
            {
                // Si la linea se pasa del rango 
                int tiempoAsignado = l.GetTiempoAtencion() - l.GetTiempoRestante();
                if (tiempoAsignado > rango)
                {
                    // Hay que buscar el porqué
                    if (l.GetNumVehiculosAsignados() > 1)
                    {
                        // No puede darse este caso, hay que poner la penalización 
                        Console.WriteLine("La linea está muy llena y tiene mas de un vehiculo");
                        return false;
                    }
                }
            }
            return true;
        }


        /****************************************************
        Verifica que cada lineas esté en el rango           *
        ****************************************************/
        private bool LineasEnRango(int rango)
        {
            foreach (Linea l in this.poblacion.GetLineas())
            {
                foreach (vehiculo v in this.listaEspera)
                {
                    // Si el vehiculo no fue asignado
                    if (v.GetLineaAsignada() == null)
                    {
                        int nuevotiempoTotal = (l.GetTiempoAtencion() - l.GetTiempoRestante()) + v.GetTiempo();
                        if (nuevotiempoTotal <= rango)
                        {
                            // si no se sale del rango, hay que penalizar 
                            Console.WriteLine("Se pudo haber ingresado el vehiculo: " + v.GetId());
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        /****************************************************
        Indica si la poblacion es equivalente en las lineas *
        ****************************************************/
        private bool VerificarEquivalencia(int mayorCargaPosible)
        {
            foreach (Linea l in this.lineas)
            {
                int cargaAsignada = (l.GetTiempoAtencion() - l.GetTiempoRestante());
                if (cargaAsignada > mayorCargaPosible)
                {
                    Console.WriteLine("La linea: " + l.GetTiempoAtencion() + " se pasó de la mayor carga posible que era: " + mayorCargaPosible);
                    return false;
                }
            }
            return true;
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
            foreach(vehiculo v in this.listaEspera)
            {
                v.SetLineaAsignada(null);
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
                            int tiempoAsignado = tempMenorLinea.GetTiempoAtencion() - tempMenorLinea.GetTiempoRestante();
                            if (tiempoAsignado > (l.GetTiempoAtencion() - l.GetTiempoRestante()))
                            {
                                tempMenorLinea = l;
                            }
                        }
                        // si no, ver la que tenga menor carga y que no sobrepase el limite 

                        if (asignado == false)
                        {
                            int futuroValorLinea = (tempMenorLinea.GetTiempoAtencion() - tempMenorLinea.GetTiempoRestante()) + tempVehiculo.GetTiempo();
                            int maximoConsumo = this.GetTiempoatencionLineaMasPequena() + 20;
                            if (futuroValorLinea <= maximoConsumo)
                            {
                                tempVehiculo.SetLineaAsignada(tempMenorLinea); 
                                tempMenorLinea.RestarTiempo(tempVehiculo.GetTiempo());
                                tempMenorLinea.IncrementarVehiculos();
                            }
                        }
                    }
                    
                    
                    Console.WriteLine("El vehiculo: " + tempVehiculo.GetId());
                    if (tempVehiculo.GetLineaAsignada() != null)
                    {
                        Console.WriteLine("Fué asignado en la linea: " + tempVehiculo.GetLineaAsignada().GetTiempoAtencion());
                        Console.WriteLine("#");
                        Console.WriteLine("--------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("- - - > No se asignó el vehiculo ");
                    }
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
            // Busca la linea con menor capacidad
            Linea lineaConMenoCapacidad = this.GetLineaConMenoCapacidad();
            int maximoValorPorLinea = lineaConMenoCapacidad.GetTiempoAtencion();


            // Busca si alguna linea se pasa del rango 
            int rango = maximoValorPorLinea + 20;
            if (this.LineasFueraDeRango(rango) == false)
            {
                return false;
            }
            

            // Verifica que cada lineas esté en el rango
            if(this.LineasEnRango(rango) == false)
            {
                return false;
            }


            // Verifico la equivalencia final 
            int mayorCargaPosible = lineaConMenoCapacidad.GetTiempoAtencion() - lineaConMenoCapacidad.GetTiempoRestante() + 20;
            if (this.VerificarEquivalencia(mayorCargaPosible))
            {
                // Si llega aqui, es una solucion valida
                return true;
            }


            // Si se acabaron las generaciones 
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
                //this.PrintPoblacion();

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
            this.PrintPoblacion();
            return this.poblacion;
        }
















        // IMPRIME UNA SOLUCION en consola
        public void PrintPoblacion()
        {
            Poblacion pob = this.poblacion;
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