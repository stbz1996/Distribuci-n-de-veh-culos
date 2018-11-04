using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejemplo1
{
    class Poblacion
    {
        private List<vehiculo> vehiculos;
        private List<Linea> lineas;

        public Poblacion(List<vehiculo> pVehiculos, List<Linea> pLineas)
        {
            this.vehiculos = pVehiculos;
            this.lineas = pLineas;
        }

        public List<vehiculo> GetVehiculos()
        {
            return this.vehiculos;
        }

        public List<Linea> GetLineas()
        {
            return this.lineas;
        }

        public void SetVehiculos(List<vehiculo> list)
        {
            this.vehiculos = list;
        }


    }
}