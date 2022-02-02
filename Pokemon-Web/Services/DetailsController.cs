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

namespace Pokemon_Web.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : Controller
    {
        private readonly ILogger _logger;


        public DetailsController(ILogger<DetailsController> logger)
        {
            _logger = logger;
        }
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            GenericResponce<Pokemon> result = new GenericResponce<Pokemon>();
            try
            {
                PokeApiClient client = new PokeApiClient(_logger);
                var task = client.GetResourceAsync<Pokemon>(id);
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
