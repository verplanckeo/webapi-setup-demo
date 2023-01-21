namespace DemoAuth.ApiClient.Models
{
    /// <summary>
    /// Movie model
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Numerical identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the movie. Ex: John Wick.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public Movie(int id, string title)
        {
            Id = id;
            Title = title;
        }

        /// <summary>
        /// Create an instance of <see cref="Movie"/>
        /// </summary>
        /// <param name="id"><see cref="Id"/></param>
        /// <param name="title"><see cref="Title"/></param>
        /// <returns></returns>
        public static Movie Create(int id, string title) => new(id, title);
    }
}
