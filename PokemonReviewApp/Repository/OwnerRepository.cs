using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Linq;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
       

        public OwnerRepository(DataContext context)
        {
            _context = context;
            
        }        

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners
                .Where(o => o.Id == ownerId)  
                .FirstOrDefault();
               
        }

        public ICollection<Owner> GetOwnerOfPokemon(int pokemonId)
        {
            return _context.PokemonOwners
                // Inspects Pokemon to ensure int pokemonId matches
                .Where(p => p.Pokemon.Id == pokemonId)
                // Selects the owner of the pokemon chosen
                .Select(o => o.Owner)
                .ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners
                .Where(o => o.Owner.Id == ownerId)
                .Select(p => p.Pokemon)
                .ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        // ________________ POST _________________ \\


        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
