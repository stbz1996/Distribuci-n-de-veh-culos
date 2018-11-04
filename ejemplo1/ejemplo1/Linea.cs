using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejemplo1
{
    class Linea
    {
        private int tiempoAtencion;
        private int tiempoRestante;
        private List<char> tiposVehiculos;
        private bool activa;

        public Linea(int pTiempoAtencion, List<char> pTiposVehiculos, bool pActiva)
        {
            this.tiempoAtencion = pTiempoAtencion;
            this.tiposVehiculos = pTiposVehiculos;
            this.activa         = pActiva;
            this.tiempoRestante = this.tiempoAtencion;
        }

        public List<char> getTiposVehiculos()
        {
            return this.tiposVehiculos;
        }

        public void RestarTiempo(int pTiempo)
        {
            this.tiempoRestante -= pTiempo;
        }

        public int GetTiempoRestante()
        {
            return this.tiempoRestante;
        }

        public int GetTiempoAtencion()
        {
            return this.tiempoAtencion;
        }

        public bool GetEstaActiva()
        {
            return this.activa;
        }

        public void SetActiva(bool val)
        {
            this.activa = val;
        }

        public void RestablecerTiemporestante()
        {
            this.tiempoRestante = this.tiempoAtencion;
        }
    }
}
