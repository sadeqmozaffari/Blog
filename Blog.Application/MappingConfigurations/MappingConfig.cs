using Blog.Common.DTOs.Post;
using Blog.Domain.Entities;
using Mapster;


namespace Blog.Application.MappingConfigurations
{
	public static class MappingConfig
	{
		public static void Register()
		{
			var config = TypeAdapterConfig.GlobalSettings;

			config.NewConfig<Post, PostDTO>()
				.Map(dest => dest.CategoryName, src => src.Category.Title);
		}
	}
}
