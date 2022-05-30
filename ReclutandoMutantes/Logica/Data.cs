using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReclutandoMutantes.Logica
{
    public class Data
    {
        private static int N;
        private static string[,] matriz;
        private static List<string> diagonales;
        private static List<string> horizontales;
        private static List<string> verticales;
        public static string error { get; set; }

        #region Métodos Públicos
        public static bool isMutant(string[] dna)
        {
            DataBase db = new DataBase();
            N = dna.Length;

            if (!CrearMatriz(dna))
            {
                error = "Error al crear la matriz";
                return false;
            }

            Data.diagonales = new List<string>();
            //Recorrido de la matriz en varias direcciones
            if (!RecorreDiagonales_IzqArrib())
            {
                error = "Error recorriendo la matriz";
                return false;
            }

            if (!RecorreDiagonales_DerArrib())
            {
                error = "Error recorriendo la matriz";
                return false;
            }

            Data.horizontales = new List<string>();
            if (!RecorrerMatrizHorizontalmente())
            {
                error = "Error recorriendo la matriz";
                return false;
            }

            Data.verticales = new List<string>();
            if (!RecorrerMatrizVerticalmente())
            {
                error = "Error recorriendo la matriz";
                return false;
            }

            //Si fue exitoso el recorrido de la matriz
            //Se validan las secuencias guardadas en las listas
            int diagonales = ValidarSecuencia("diagonales");
            int horizontales = ValidarSecuencia("horizontales");
            int verticales = ValidarSecuencia("verticales");
            int totalSecuencias = diagonales + horizontales + verticales;

            //Si hay mas de una diagonal con una secuencia de 4 caracteres iguales, es mutante
            if (totalSecuencias > 1)
            {
                //Si es mutante, guardo la secuencia de ADN en la BD en la colección de ADNs mutantes
                Dictionary<string, string[]> listaMutantes = db.ListaADN_Mutantes();
                int id = 1;
                if (listaMutantes != null)
                    id = listaMutantes.Count + 1;

                if(!db.GuardaADN_Mutantes(id, dna))
                {
                    error = "Error guardando en la BD";
                    return false;
                }
                return true;
            }
            else
            {
                //Si NO es mutante, guardo la secuencia de ADN en la BD en la colección de ADNs humanos
                Dictionary<string, string[]> listaHumanos = db.ListaADN_Humanos();
                int id = 1;
                if (listaHumanos != null)
                    id = listaHumanos.Count + 1;

                if(!db.GuardaADN_Humano(id, dna))
                {
                    error = "Error guardando en la BD";
                    return false;
                }

                return false;
            }
        }

        public static bool Validacion(string[] array)
        {
            return ValidarDatos(array);
        }
        #endregion

        #region Métodos Privados
        private static bool ValidarDatos(string[] array)
        {
            string patron = @"[^ATCG]";
            try
            {
                foreach(string elemento in array)
                {
                    //Si encuentra cualquier caracter diferente al patron, entonces la secuencia no es válida
                    if(Regex.IsMatch(elemento, patron))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (RegexMatchTimeoutException e)
            {
                Console.WriteLine("Timeout after {0} seconds matching {1}.", e.MatchTimeout, e.Input);
                return false;
            }
        }

        private static bool CrearMatriz(string[] array)
        {
            try
            {
                matriz = new string[N,N];

                int i = 0;
                foreach(string fila in array)
                {
                    char[] temp = fila.ToCharArray();

                    for (int j = 0; j < N; j++)
                    {
                        matriz[i, j] = temp[j].ToString();
                    }
                    i++;
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + "\n" + ex.StackTrace;
                return false;
            }
        }

        private static bool RecorreDiagonales_IzqArrib()
        {
            try
            {
                for (int k = 0; k < N * 2; k++)
                {
                    string caracteres = "";
                    for (int j = 0; j <= k; j++)
                    {
                        int i = k - j;

                        if (i < N && j < N)
                        {
                            caracteres += matriz[i, j];
                        }
                    }

                    //Cada diagonal que tenga entre 4 y 6 caracteres la añadimos a la lista
                    if (caracteres.Length == 4 || caracteres.Length == 5 || caracteres.Length == 6)
                        diagonales.Add(caracteres);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + "\n" + ex.StackTrace;
                return false;
            }
        }

        private static bool RecorreDiagonales_DerArrib()
        {
            try
            {
                int fila = N - 1; //ultima fila
                int col = 0;
                int aux = 1;

                while (col < N)
                {
                    string res = "";
                    for (int i = fila; i < N && col < N; i++, col++)
                    {
                        res = res + matriz[i, col];
                    }
                    if (res.Length == 4 || res.Length == 5 || res.Length == 6)
                        diagonales.Add(res);

                    //Cuando acabo una diagonal actualizo las variables
                    fila--;
                    if (fila < 0) //si se sale de los limites de la matriz
                    {
                        fila = 0;
                        col = aux;
                        aux++;
                    }
                    else //si la fila no se ha salido
                    {
                        col = 0;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + "\n" + ex.StackTrace;
                return false;
            }
        }

        private static bool RecorrerMatrizHorizontalmente()
        {
            try
            {
                for (int i = 0; i < N; i++)
                {
                    string caracteres = "";
                    for (int j = 0; j < N; j++)
                    {
                        caracteres += matriz[i, j];
                    }
                    
                    if (caracteres.Length == 4 || caracteres.Length == 5 || caracteres.Length == 6)
                        horizontales.Add(caracteres);
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + "\n" + ex.StackTrace;
                return false;
            }
        }

        private static bool RecorrerMatrizVerticalmente()
        {
            try
            {
                for (int col = 0; col < N; col++)
                {
                    string caracteres = "";
                    for (int fila = 0; fila < N; fila++)
                    {
                        caracteres += matriz[fila, col];
                    }
                    
                    if (caracteres.Length == 4 || caracteres.Length == 5 || caracteres.Length == 6)
                        verticales.Add(caracteres);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + "\n" + ex.StackTrace;
                return false;
            }
        }

        private static int ValidarSecuencia(string tipo)
        {
            int contadorOcurrencias = 0;
            List<string> listado = new List<string>();
            Console.WriteLine();

            switch (tipo)
            {
                case "diagonales":
                    listado = diagonales;
                    break;
                case "horizontales":
                    listado = horizontales;
                    break;
                case "verticales":
                    listado = verticales;
                    break;
            }

            foreach (string item in listado)
            {
                int ocurrenciasA = ContarOcurrencias(item, "AAAA");
                contadorOcurrencias += ocurrenciasA;

                int ocurrenciasT = ContarOcurrencias(item, "TTTT");
                contadorOcurrencias += ocurrenciasT;

                int ocurrenciasC = ContarOcurrencias(item, "CCCC");
                contadorOcurrencias += ocurrenciasC;

                int ocurrenciasG = ContarOcurrencias(item, "GGGG");
                contadorOcurrencias += ocurrenciasG;
            }
            return contadorOcurrencias;
        }

        private static int ContarOcurrencias(string cadena, string subCadena)
        {
            int res = 0;
            string aux = "";
            int longSubCad = subCadena.Length;

            //Mientras mi posicion sea menor o igual que
            //la longitud de la cadena menos la longitud de la subcadena
            for (int pos = 0; pos <= cadena.Length - longSubCad; pos++)
            {
                for (int i = pos; i < pos + longSubCad; i++)
                {
                    aux = aux + cadena[i];
                }
                if (subCadena.Equals(aux))
                {
                    res++;
                }
                aux = "";
            }
            return res;
        }
        #endregion
    }
}
