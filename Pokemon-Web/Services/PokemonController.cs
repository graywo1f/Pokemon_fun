using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokemon_Api;
using Pokemon_Api.Models;
using Pokemon_Data.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {

        private readonly ILogger _logger;

        public PokemonController(ILogger<PokemonController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public JsonResult Get(int id = 0)
        {
            GenericResponce<NamedApiResourceList<Pokemon>> result = new GenericResponce<NamedApiResourceList<Pokemon>>();
            try
            {
                PokeApiClient client = new PokeApiClient(_logger);
                var task = client.GetNamedResourcePageAsync<Pokemon>(20, id);
                task.Wait();
                result.Responce = task.Result;
                result.ResponceOK = true;
            }
            catch (Exception _e)
            {
                result.Error = _e.ToString();
                _logger.LogError("Exception {0} {1} {2}", _e.ToString(), _e.StackTrace?.ToString(), _e.InnerException?.Message);
            }
            return Json(result);
        }

    }
}
