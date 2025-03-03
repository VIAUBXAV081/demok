using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Server.Database;
using Blog.Server.Database.Models;
using Blog.Server.DTOs;
using Blog.Server.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Server.Repositories
{
    public interface IPostRepository
    {
        Task DeletePostAsync(int postId);
        Task<Post> GetPostAsync(int postId);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<Post> InsertPostAsync(NewPost newPost);
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

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _context.Posts
                .ProjectTo<Post>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Post> GetPostAsync(int postId)
        {
            return await _context.Posts
                .ProjectTo<Post>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.ID == postId)
                ?? throw new EntityNotFoundException($"Post not found with ID {postId}!");
        }

        public async Task<Post> InsertPostAsync(NewPost newPost)
        {
            var efPost = _mapper.Map<PostEntity>(newPost);
            _context.Posts.Add(efPost);
            await _context.SaveChangesAsync();
            return await GetPostAsync(efPost.ID);
        }

        public async Task DeletePostAsync(int postId)
        {
            _context.Posts.Remove(new PostEntity { ID = postId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!await _context.Posts.AnyAsync(p => p.ID == postId))
                    throw new EntityNotFoundException($"Post not found with ID {postId}!");
                else
                    throw;
            }
        }
    }
}
