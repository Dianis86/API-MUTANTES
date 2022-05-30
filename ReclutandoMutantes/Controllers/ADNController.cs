using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ReclutandoMutantes.Logica;
using ReclutandoMutantes.Models;

namespace ReclutandoMutantes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ADNController : Controller
    {
        

        [HttpPost]
        [Route("/mutant")]
        public IActionResult Mutante([FromBody] ADN cadena)
        {           
            string[] dna = cadena.dna;
            try
            {
                string msg = "";
                int statusCode = 0;

                if (dna != null)
                {
                    msg = string.Empty;
                    statusCode = 0;

                    //Valido que la cadena tenga caracteres permitidos
                    if (Data.Validacion(dna))
                    {
                        //Valido la secuencia de ADN para saber si es mutante
                        if (Data.isMutant(dna))
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


        [HttpGet]
        [Route("/stats")]
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
