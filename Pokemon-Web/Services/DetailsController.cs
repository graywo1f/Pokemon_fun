using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Api;
using Pokemon_Api.Models;
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
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            PokeApiClient client = new PokeApiClient();

            // act
            var results = client.GetResourceAsync<Pokemon>(id).Result;

            return Json(results);
        }
    }
}
