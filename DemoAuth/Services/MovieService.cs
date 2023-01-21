using DemoAuth.Models;

namespace DemoAuth.Services
{
    /// <summary>
    /// Movie service, responsible for retrieving movies from a resource.
    /// </summary>
    public class MovieService : IMovieService
    {
        /// <summary>
        /// Return list of movies.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Movie> GetMovies()
        {
            yield return Movie.Create(1, "Black Panther");
            yield return Movie.Create(2, "John Wick");
        }
    }
}
