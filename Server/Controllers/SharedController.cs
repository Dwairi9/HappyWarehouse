using HappyWarehouse.BusinessLogic.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Server.Controllers
{
    public class SharedController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public SharedController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [Route("api/Shared/GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(await _countryService.GetCountries());
        }
    }
}
