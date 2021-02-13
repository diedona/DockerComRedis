using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContagemController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public ContagemController(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<IActionResult> Get()
        {
            string hits = await _cache.GetStringAsync("hits");
            if (string.IsNullOrEmpty(hits))
                hits = await CriarHits();

            int hitsIncrementado = int.Parse(hits);
            hitsIncrementado++;
            await _cache.SetStringAsync("hits", hitsIncrementado.ToString());

            return Ok(hitsIncrementado);
        }

        private async Task<string> CriarHits()
        {
            await _cache.SetStringAsync("hits", "0");
            return await _cache.GetStringAsync("hits");
        }
    }
}
