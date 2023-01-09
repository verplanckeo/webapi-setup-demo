using DemoAuth.Models;

namespace DemoAuth.Services
{
    public interface IMovieService
    {
        IEnumerable<Movie> GetMovies();
    }
}
