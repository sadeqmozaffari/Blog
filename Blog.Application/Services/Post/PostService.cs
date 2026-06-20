using Blog.Common;
using Blog.Common.DTOs.Post;
using Blog.Domain.Entities;
using Blog.Domain.UnitOfWork;
using Mapster;
using MapsterMapper;

namespace Blog.Application.Services.Post
{
	public class PostService : IPostService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PostService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApiResponse<List<PostDTO>>> GetAllAsync()
		{
			var posts = await _unitOfWork.Posts.GetAllAsync();

			var data = _mapper.Map<List<PostDTO>>(posts);

			return ApiResponse<List<PostDTO>>
				.Ok(data, "Posts retrieved successfully");
		}

		public async Task<ApiResponse<PostDTO>> GetByIdAsync(int id)
		{
			var post = await _unitOfWork.Posts.GetByIdAsync(id);

			if (post == null)
				return ApiResponse<PostDTO>.NotFound("Post not found");

			var data = _mapper.Map<PostDTO>(post);

			return ApiResponse<PostDTO>
				.Ok(data, "Post retrieved successfully");
		}

		public async Task<ApiResponse<List<PostDTO>>> GetByCategoryIdAsync(int categoryId)
		{
			var posts = await _unitOfWork.Posts.GetByCategoryIdAsync(categoryId);

			var data = _mapper.Map<List<PostDTO>>(posts);

			return ApiResponse<List<PostDTO>>
				.Ok(data, "Posts by category retrieved successfully");
		}

		public async Task<ApiResponse<PostDTO>> CreateAsync(PostCreateDTO dto)
		{
			var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

			if (category == null)
				return ApiResponse<PostDTO>.NotFound("Category not found");

			var post = dto.Adapt<Domain.Entities.Post>();

			post.CreatedDate = DateTime.UtcNow;

			await _unitOfWork.Posts.AddAsync(post);
			await _unitOfWork.SaveAsync();

			var result = _mapper.Map<PostDTO>(post);

			return ApiResponse<PostDTO>
				.CreatedAt(result, "Post created successfully");
		}

		public async Task<ApiResponse<PostDTO>> UpdateAsync(PostUpdateDTO dto)
		{
			var post = await _unitOfWork.Posts.GetByIdAsync(dto.Id);

			if (post == null)
				return ApiResponse<PostDTO>.NotFound("Post not found");

			post.Title = dto.Title;
			post.Description = dto.Description;
			post.ImageUrl = dto.ImageUrl;
			post.CategoryId = dto.CategoryId;
			post.UpdatedDate = DateTime.UtcNow;

			_unitOfWork.Posts.Update(post);
			await _unitOfWork.SaveAsync();

			var result = _mapper.Map<PostDTO>(post);

			return ApiResponse<PostDTO>
				.Ok(result, "Post updated successfully");
		}

		public async Task<ApiResponse<object>> DeleteAsync(int id)
		{
			var post = await _unitOfWork.Posts.GetByIdAsync(id);

			if (post == null)
				return ApiResponse<object>.NotFound("Post not found");

			_unitOfWork.Posts.Delete(post);
			await _unitOfWork.SaveAsync();

			return ApiResponse<object>
				.NoContent("Post deleted successfully");
		}
	}
}