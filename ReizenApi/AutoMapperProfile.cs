using AutoMapper;
using Reizen.Data.Models;
using ReizenApi.Models;

namespace ReizenApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        { 
            CreateMap<Klant, KlantDTO>();
            CreateMap<KlantDTO, Klant> ();

            CreateMap<Land, LandDTO> ();
            CreateMap<LandDTO, Land> ();

            CreateMap<Bestemming, BestemmingDTO> ();
            CreateMap<BestemmingDTO, Bestemming> ();

            CreateMap<Werelddeel, WerelddeelDTO> ();
            CreateMap<WerelddeelDTO, Werelddeel> ();

            CreateMap<Boeking, BoekingDTO> ();
            CreateMap<BoekingDTO, Boeking> ();

            CreateMap<Reis, ReisDTO> ();
            CreateMap<ReisDTO, Reis> ();

            CreateMap<Woonplaats, WoonplaatsDTO> ();
            CreateMap<WoonplaatsDTO, Woonplaats> ();
        }
    }
}
