using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejemplo1
{
    class vehiculo
    {
        private int id;
        private char tipo;
        private int tiempo;

        private double probAsignado;
        private Linea lineaAsignada;

        public vehiculo(int pId, char pTipo, int pTiempo)
        {
            this.id     = pId;
            this.tipo   = pTipo;
            this.tiempo = pTiempo;
        }

        // GETTERS
        public int GetId(){return this.id;}

        public char GetTipo(){return this.tipo;}

        public int GetTiempo(){return this.tiempo;}

        public double GetProbAsignado(){return this.probAsignado;}

        public Linea GetLineaAsignada() { return this.lineaAsignada; }

        // SETTERS

        public void SetId(int pId) {this.id = pId; }

        public void SetTipo(char pTipo) {this.tipo = pTipo; }

        public void SetTiempo(int pTiempo) {this.tiempo = pTiempo;}

        public void SetProbAsignado(double pProb) {this.probAsignado = pProb; }

        public void SetLineaAsignada(Linea pLinea) {this.lineaAsignada = pLinea; }   
    }
}
