using AutoMapper;
using Blog.Server.DTOs;
using Data.Models;

namespace Blog.Server
{
    public class ApiProfile: Profile
    {
        public ApiProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Post, NewPostDto>().ReverseMap();
            CreateMap<Post, NewPostIdeaDto>().ReverseMap();
            CreateMap<Post, PostSuggestionDto>().ReverseMap();
        }
    }
}
