using System.ComponentModel.DataAnnotations;

namespace Blog.Server.DTOs
{
    public record PostDto
    {
        public int? ID { get; init; }
        public string? Title { get; init; }
        public string? Content { get; init; }
    }

    public record NewPostDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required!")]
        [StringLength(450, ErrorMessage = "The title have to be less than or equal to 450 characters long!")]
        public string? Title { get; init; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content is required!")]
        public string? Content { get; init; }
    }

    public record NewPostIdeaDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required!")]
        [StringLength(450, ErrorMessage = "The title have to be less than or equal to 450 characters long!")]
        public string? Title { get; init; }
    }

    public record PostSuggestionDto
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
    }
}
