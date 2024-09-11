using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Server.DTOs;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Server.Repositories
{
    public interface IPostRepository
    {
        Task DeletePostAsync(int postId);
        Task<PostDto> GetPostAsync(int postId);
        Task<IEnumerable<PostDto>> GetPostsAsync();
        Task<PostDto> InsertPostAsync(NewPostDto newPost);
    }

    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _context;
        private readonly IMapper _mapper;

        public PostRepository(BlogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> GetPostsAsync()
        {
            return await _context.Posts
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PostDto> GetPostAsync(int postId)
        {
            return await _context.Posts
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.ID == postId)
                ?? throw new Exception($"Post not found with ID {postId}!");
        }

        public async Task<PostDto> InsertPostAsync(NewPostDto newPost)
        {
            var efPost = _mapper.Map<Post>(newPost);
            _context.Posts.Add(efPost);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return await GetPostAsync(efPost.ID);
        }

        public async Task DeletePostAsync(int postId)
        {
            _context.Posts.Remove(new Post { ID = postId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!await _context.Posts.AnyAsync(p => p.ID == postId))
                    throw new Exception($"Post not found with ID {postId}!");
                else
                    throw;
            }
        }
    }
}
