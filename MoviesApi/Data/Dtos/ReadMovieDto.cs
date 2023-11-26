namespace MoviesApi.Data.Dtos
{
    public class ReadMovieDto
    {
        public string Title { get; set; }

        public string Genre { get; set; }

        public int Duration { get; set; }

        public int Year { get; set; }

        public DateTime CheckedAt { get; set; } = DateTime.Now;
    }
}
