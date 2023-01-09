namespace DemoAuth.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Movie(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public static Movie Create(int id, string title) => new(id, title);
    }
}
