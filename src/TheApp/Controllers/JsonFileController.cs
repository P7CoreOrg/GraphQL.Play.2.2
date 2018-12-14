﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheApp.Services;

namespace TheApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class JsonFileController : ControllerBase
    {
        private IJsonFileLoader _jsonFileLoader;
        private IHttpContextAccessor _httpContextAccessor;

        public JsonFileController(IJsonFileLoader jsonFileLoader, IHttpContextAccessor httpContextAccessor)
        {
            _jsonFileLoader = jsonFileLoader;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET api/values
        [HttpGet]
        [Route("open")]
        public async Task< ActionResult<object>> GetOpenJsonAsync(string file)
        {
            return await _jsonFileLoader.LoadAsync(file);
        } // GET api/values
        [HttpGet]
        [Authorize("Daffy Duck")]
        [Route("closed")]
        public async Task<ActionResult<object>> GetClosedJsonAsync(string file)
        {
            var query = from item in _httpContextAccessor.HttpContext.User.Claims
                where item.Type == ClaimTypes.NameIdentifier
                select item;
            var nameClaim = query.FirstOrDefault();

            return await _jsonFileLoader.LoadAsync($"{nameClaim.Value}/{file}");
        }
    }
}