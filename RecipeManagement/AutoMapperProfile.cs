
using AutoMapper;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;
using System;


namespace RecipeManagement
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<User, UserModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
               .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive.HasValue ? src.IsActive.Value == 1 : false))               
            .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin.HasValue ? src.IsAdmin.Value == 1 : false)).ReverseMap();

            CreateMap<Recipe, RecipeModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
                .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
                .ForMember(dest => dest.CookingTimeInMins, opt => opt.MapFrom(src => src.CookingTimeInMins))                
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active.HasValue ? src.Active.Value == 1 : false))
                .ForMember (dest=> dest.RecipeImages , opt=>opt.MapFrom(src=>src.RecipeImages.Select(r=>r.Path)))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId)).ReverseMap ();

            CreateMap<Category , CategoryModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();
        }
    }
}
