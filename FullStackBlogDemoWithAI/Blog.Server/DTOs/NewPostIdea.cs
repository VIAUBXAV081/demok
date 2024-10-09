using System.ComponentModel.DataAnnotations;

namespace Blog.Server.DTOs
{
    public record NewPostIdea
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required!")]
        [StringLength(450, ErrorMessage = "The title have to be less than or equal to 450 characters long!")]
        public string? Title { get; init; }
    }
}
