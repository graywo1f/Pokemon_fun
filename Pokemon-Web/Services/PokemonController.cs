using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Api;
using Pokemon_Api.Models;
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
        [HttpGet]       
        public JsonResult Get()
        {
            PokeApiClient client = new PokeApiClient();

            // act
            NamedApiResourceList<Pokemon> page = client.GetNamedResourcePageAsync<Pokemon>().Result;

            return Json(page);
        }

        [HttpGet("{id}",Name = "GetDetails")]
        public JsonResult GetDetails(int id)
        {
            PokeApiClient client = new PokeApiClient();

            // act
            var results = client.GetResourceAsync<Pokemon>(id).Result;

            return Json(results);
        }
    }
}
