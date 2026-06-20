using Blog.Common;


namespace Blog.MVC.Services.IServices
{
    public interface IBaseService
    {
        ApiResponse<object> ResponseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
