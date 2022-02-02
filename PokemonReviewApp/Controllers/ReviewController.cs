using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _repo;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_repo.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReview(int reviewId)
        {
            if (!_repo.ReviewExists(reviewId))
                return BadRequest(ModelState);

            var review = _mapper.Map<ReviewDto>(_repo.GetReview(reviewId));

            if (!ModelState.IsValid)
                return NotFound();
            return Ok(review);
        }

        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfSinglePokemon(int pokemonId)
        {
            var review = _mapper.Map<List<ReviewDto>>(_repo.GetReviewsOfSinglePokemon(pokemonId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

    }
}
