namespace PokemonReviewApp.Models
{
    public class PokemonCategory
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Category Category { get; set; }

        // Many to many
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
