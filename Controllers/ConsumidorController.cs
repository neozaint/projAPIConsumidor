﻿using apiConsumidor.BackgroundTask;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace apiConsumidor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumidorController : ControllerBase
    {

        private IBackgroundTask _backgroundTask;
        private IConfiguration _configuration;
        private IActionResult _response;

        public ConsumidorController(IConfiguration configuration, IBackgroundTask backgroundTask)
        {
            _backgroundTask = backgroundTask;
            _configuration = configuration;
        }
        public string GetExtraValue(IConfiguration configuration)
        {
            return configuration.GetValue<string>("ExtraSettingNotInSettingsFile");
        }

        [HttpGet("Saludar")]
        public IActionResult Saludar()
        {
            Log.Information("Se incovó el método: " + nameof(Saludar));

            string mensaje = "Hola soy: " + System.Reflection.Assembly.GetExecutingAssembly().FullName
                + " Variable Ambiente: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                + " ExtraSettingNotInSettingsFile: "+GetExtraValue(_configuration); 
            return Ok(mensaje);
        }

        [HttpGet("SaludarEscalador")]
        public async Task<IActionResult> SaludarEscalador()
        {
            try
            {

                Log.Information("Se incovó el método: " + nameof(SaludarEscalador));
                string mensaje = _backgroundTask.SaludarEscalador(new System.Threading.CancellationToken()).Result;

                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Inicia la obtención de mensajes y publicación de mensajes de el activeMQ. [No lanza excepcion]
        /// </summary>
        /// <returns></returns>
        [HttpGet("IniciarOperacion")]
        public async Task<IActionResult> IniciarOperacionAsync()
        {
            try
            {
                Log.Information("Se incovó el método: " + nameof(IniciarOperacionAsync));

                await _backgroundTask.StartAsync(_configuration);
                _response = Ok(System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                _response = StatusCode(HttpStatusCode.InternalServerError.GetHashCode(),
                    ex.Message);
            }
            return _response;
        }

    }
}
