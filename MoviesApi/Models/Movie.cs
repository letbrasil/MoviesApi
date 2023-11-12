using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título do filme é obrigatório")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O gênero do filme é obrigatório")]
        [StringLength(50, ErrorMessage = "O gênero não pode exceder 50 caracteres")]
        public string Genre { get; set; }

        [Required]
        [Range(1, 600, ErrorMessage = "A duração deve ter no mínimo 1 e no máximo 600 minutos")]
        public int Duration { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
