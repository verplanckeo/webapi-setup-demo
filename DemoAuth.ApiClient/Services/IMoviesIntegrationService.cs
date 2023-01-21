using DemoAuth.ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAuth.ApiClient.Services
{
    public interface IMoviesIntegrationService
    {
        /// <summary>
        /// Get list of all movies.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Remove one given movie from the db.
        /// </summary>
        /// <param name="id">Id of the movie.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> RemoveOneMovieAsync(string id, CancellationToken cancellationToken);
    }
}
