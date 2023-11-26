using HappyWarehouse.BusinessLogic.DTOs;

namespace HappyWarehouse.BusinessLogic.Services.IServices
{
    public interface ICountryService
    {
        Task<List<CountryDto>> GetCountries();
    }
}
