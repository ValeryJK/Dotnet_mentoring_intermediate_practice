using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.DAL.Entities;
using EventBookSystem.Data.Entities;

namespace EventBookSystem.Core.Service.MappingProfile
{
    public class MappingCoreProfile : Profile
    {
        public MappingCoreProfile()
        {
            CreateMap<UserForRegistrationDto, User>();

            CreateMap<Venue, VenueDTO>();
            CreateMap<Section, SectionDto>();

            CreateMap<Event, EventDto>();
            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.SeatId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new SeatStatusDto
                {
                    Id = (int)src.Status,
                    Name = src.Status.ToString()
                }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new PriceDto
                {
                    Id = src.Price.Id,
                    Name = src.Price.Name
                }));

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new PaymentStatusDto
                {
                    Id = (int)src.Status,
                    Name = src.Status.ToString()
                }));

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.CartItems.Sum(x => x.Seat.Price.Amount)));

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Seat.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Seat.Price.Amount));

            CreateMap<EventForCreationDto, Event>();
            CreateMap<EventForUpdateDto, Event>();

            CreateMap<VenueForCreationDto, Venue>();
            CreateMap<VenueForUpdateDto, Venue>();
        }
    }
}
