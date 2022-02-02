using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _repo;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_repo.GetOwners());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_repo.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_repo.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if(!_repo.OwnerExists(ownerId))
                return NotFound(ModelState);

            var owner = _mapper.Map<List<PokemonDto>>(_repo.GetPokemonByOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        // ______________ POST ____________________ \\

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromBody]OwnerDto ownerCreate)
        {
            if(ownerCreate == null)
            {
                return BadRequest(ModelState);
            } 

            var owners = _repo.GetOwners()
                .Where(o => o.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (owners != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            if (!_repo.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                    return StatusCode(500, ModelState); 
            }
            return Ok("Successfully created owner");
        }
    }
}
