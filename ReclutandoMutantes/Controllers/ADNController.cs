using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ReclutandoMutantes.Logica;
using ReclutandoMutantes.Models;
using Microsoft.AspNetCore.Authorization;

namespace ReclutandoMutantes.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ADNController : Controller
    {
        /// <summary>
        /// Verifica que la secuencia de ADN sea váldida,
        /// si la secuencia ingresada es mutante o humano, y
        /// guarda en la BD los registros de las secuencias
        /// </summary>
        /// <param name="dna"></param>
        /// <returns>Devuelve codigo http y un mensaje</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /mutant
        ///     {
        ///        "dna": ["ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG"]
        ///     }
        ///
        /// </remarks>
        /// /// <response code="200">Devuelve como respuesta OK --> Es mutante</response>
        /// <response code="403">Forbidden --> Devuelve como respuesta NO es mutante</response>
        /// <response code="500">InternalServerError --> Si hay algún error creando o recorriendo la matriz</response>
        /// <response code="400">BadRequest --> Tiene caracteres no permitidos ó no se proporcionó ningún dato</response>
        [HttpPost]
        [Route("/mutant")]
        [AllowAnonymous]
        public IActionResult Mutante([FromBody] ADN dna)
        {
            string[] secuencia = dna.dna;
            try
            {
                string msg = "";
                int statusCode = 0;

                if (secuencia != null)
                {
                    msg = string.Empty;
                    statusCode = 0;

                    //Valido que la cadena tenga caracteres permitidos
                    if (Data.Validacion(secuencia))
                    {
                        //Valido la secuencia de ADN para saber si es mutante
                        if (Data.isMutant(secuencia))
                        {
                            statusCode = StatusCodes.Status200OK;
                            msg = "Es mutante";  
                        }
                        else
                        {
                            //Si hay algún error creando o recorriendo la matriz
                            if (Data.error != null)
                            {
                                statusCode = StatusCodes.Status500InternalServerError;
                                msg = Data.error;
                            }

                            //Si la secuencia es de un humano
                            statusCode = StatusCodes.Status403Forbidden;
                            msg = "NO es mutante";
                        }
                    }
                    else
                    {
                        statusCode = StatusCodes.Status400BadRequest;
                        msg = "Cadena de ADN no válida, tiene caracteres no permitidos!";
                    }
                }
                else
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    msg = "No se proporcionó ningún dato";
                }
                return StatusCode(statusCode, new { mensaje = msg });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        /// <summary>
        /// Método que comprueba la cantidad de adn mutantes y humanos para sacar un ratio
        /// </summary>
        /// <returns>Devuelve un Json con las estadíticas de las verificaciones</returns>
        [HttpGet]
        [Route("/stats")]
        [AllowAnonymous]
        public IActionResult Estados()
        {       
            try
            {
                DataBase db = new DataBase();
                string rpta = db.Estadisticas();

                if(rpta == string.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "No hay datos en la BD" });
                }
                return Ok(rpta);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
