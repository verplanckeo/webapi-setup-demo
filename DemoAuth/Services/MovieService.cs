using DemoAuth.Models;

namespace DemoAuth.Services
{
    public class MovieService : IMovieService
    {
        public IEnumerable<Movie> GetMovies()
        {
            yield return Movie.Create(1, "Black Panther");
            yield return Movie.Create(2, "John Wick");
        }
    }
}
