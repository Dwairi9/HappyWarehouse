using AutoMapper;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.BusinessLogic.Services.IServices;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Repositories.IRepsitories;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.BusinessLogic.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CountryDto>> GetCountries()
        {
            var countries = await _unitOfWork.Repository<Country>().Entities.ToListAsync();

            return _mapper.Map<List<Country>, List<CountryDto>>(countries);
        }
    }
}
