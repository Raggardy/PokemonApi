using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _repo;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ------------------------------------------------------------------------- \\


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetPokemons()
        {
            var countries = _mapper.Map<List<CountryDto>>(_repo.GetCountries());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);   
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]

        public IActionResult GetCountry(int countryId)
        {
            if (!_repo.CountryExists(countryId))
                return NotFound();

            var countries = _mapper.Map<CountryDto>(_repo.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }
        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(
                _repo.GetCountryByOwner(ownerId));

            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        // ______________ POST ____________________ \\

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if(countryCreate == null)
                return BadRequest(ModelState);

            var country = _repo.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault(); 
            
            if(country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(402, ModelState);
            }
            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_repo.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Oooops......Save failed");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created entity");
        }
    }
}
