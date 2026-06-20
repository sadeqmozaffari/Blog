
using Blog.Common.DTOs.Post;

namespace Blog.MVC.Services.IServices
{
    public interface IPostService
    {
        Task<T?> GetAllAsync<T>();
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateAsync<T>(PostCreateDTO dto);
        Task<T?> UpdateAsync<T>(PostUpdateDTO dto);
        Task<T?> DeleteAsync<T>(int id);
    }
}
