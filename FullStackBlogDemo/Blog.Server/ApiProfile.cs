using AutoMapper;
using Blog.Server.Database.Models;
using Blog.Server.DTOs;

namespace Blog.Server
{
    public class ApiProfile: Profile
    {
        public ApiProfile()
        {
            CreateMap<PostEntity, Post>().ReverseMap();
            CreateMap<PostEntity, NewPost>().ReverseMap();
        }
    }
}
