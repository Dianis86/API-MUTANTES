using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using ReclutandoMutantes.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace ReclutandoMutantes.Logica
{
    public class DataBase
    {
        IFirebaseClient cliente;
        IFirebaseConfig config;

        public DataBase()
        {
            config = new FirebaseConfig
            {
                AuthSecret = "4SlxJBEPtTrSAhhLRANnNNjEn8Yer78xYkqtgoLy",
                BasePath = "https://fb-api-1bfe9-default-rtdb.firebaseio.com/"
            };
        }

        #region Métodos Privados
        private Dictionary<string, string[]> ListarADN_Mutantes()
        {
            string lista = string.Empty;
            cliente = new FirebaseClient(config);
            FirebaseResponse responseGet = cliente.Get("Secuencias_Mutantes");

            if (responseGet.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return responseGet.ResultAs<Dictionary<string, string[]>>();
            }
            else
                return null;
        }

        private Dictionary<string, string[]> ListarADN_Humanos()
        {
            string lista = string.Empty;
            cliente = new FirebaseClient(config);
            FirebaseResponse responseGet = cliente.Get("Secuencias_Humanos");

            if (responseGet.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return responseGet.ResultAs<Dictionary<string, string[]>>();
            }
            else
                return null;

        }

        private bool GuardarADN_Mutante(int id, string[] dna)
        {
            cliente = new FirebaseClient(config);
            SetResponse responseSet = cliente.Set("Secuencias_Mutantes/" + "dna" + id, dna);

            if (responseSet.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        private bool GuardarADN_Humano(int id, string[] dna)
        {
            cliente = new FirebaseClient(config);
            SetResponse responseSet = cliente.Set("Secuencias_Humanos/" + "dna" + id, dna);

            if (responseSet.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        private string Estadistica()
        {
            var listaMutantes = ListarADN_Mutantes();
            var listarHumanos = ListarADN_Humanos();

            if (listaMutantes == null || listarHumanos == null)
            {
                return string.Empty;
            }

            int cantidadMutantes = listaMutantes.Count();
            int cantidadHumanos = listarHumanos.Count();
            double ratio = ((double)cantidadMutantes / cantidadHumanos);

            return "{\"count_mutant_dna\":" + cantidadMutantes 
                + ", \"count_human_dna\":" + cantidadHumanos + ", \"ratio\":" + ratio + "}";
        }
        #endregion

        #region Métodos Públicos
        public Dictionary<string, string[]> ListaADN_Mutantes()
        {
            return ListarADN_Mutantes();
        }

        public Dictionary<string, string[]> ListaADN_Humanos()
        {
            return ListarADN_Humanos();
        }

        public bool GuardaADN_Mutantes(int id, string[] dna)
        {
            return GuardarADN_Mutante(id, dna);
        }

        public bool GuardaADN_Humano(int id, string[] dna)
        {
            return GuardarADN_Humano(id, dna);
        }

        public string Estadisticas()
        {
            return Estadistica();
        }
        #endregion
    }
}
