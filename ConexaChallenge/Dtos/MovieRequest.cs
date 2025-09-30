
namespace ConexaChallenge.Dtos
{
    public class MovieRequest 
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }
    }
}
