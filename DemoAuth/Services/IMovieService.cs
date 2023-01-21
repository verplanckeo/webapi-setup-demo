using DemoAuth.Models;

namespace DemoAuth.Services
{
    /// <summary>
    /// Movies service.
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Retrieve list of movies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Movie> GetMovies();
    }
}
