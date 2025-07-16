using AutoMapper;
//using Trips.Data.Models;
using Reizen.Data.Models;
using Reizen.CommonClasses.DTOs;

namespace ReizenApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<ClientDAL, ClientDTO>();
            CreateMap<ClientDTO, ClientDAL> ();

            CreateMap<CountryDAL, CountryDTO> ();
            CreateMap<CountryDTO, CountryDAL> ();

            CreateMap<DestinationDAL, DestinationDTO> ();
            CreateMap<DestinationDTO, DestinationDAL> ();

            CreateMap<ContinentDAL, ContinentDTO> ();
            CreateMap<ContinentDTO, ContinentDAL> ();

            CreateMap<BookingDAL, BookingDTO> ();
            CreateMap<BookingDTO, BookingDAL> ();

            CreateMap<TripDAL, TripDTO> ();
            CreateMap<TripDTO, TripDAL> ();

            CreateMap<ResidenceDAL, ResidenceDTO> ();
            CreateMap<ResidenceDTO, ResidenceDAL> ();
        }
    }
}
