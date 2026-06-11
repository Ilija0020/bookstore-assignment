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

            CreateMap<Publisher, PublisherDTO>().ReverseMap();
        }
    }
}
