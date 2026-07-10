using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();

            CreateMap<Book, BookDto>()
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => DateTime.Now.Year - src.PublishedDate.Year));
            CreateMap<Book, BookDetailsDto>();

            CreateMap<SaveBookDTO, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore());

            CreateMap<Publisher, PublisherDTO>().ReverseMap();

            CreateMap<RegistrationDto, ApplicationUser>();

            CreateMap<LoginDto, ApplicationUser>();

            CreateMap<ApplicationUser, ProfileDto>();

            CreateMap<SaveIssueDTO, Issue>();

            CreateMap<NewReviewDTO, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
