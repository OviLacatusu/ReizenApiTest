using AutoMapper;
//using Reizen.Data.Models;
using Reizen.Data.Models;
using Reizen.Domain.DTOs;

namespace ReizenApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<KlantDAL, KlantDTO>();
            CreateMap<KlantDTO, KlantDAL> ();

            CreateMap<LandDAL, LandDTO> ();
            CreateMap<LandDTO, LandDAL> ();

            CreateMap<BestemmingDAL, BestemmingDTO> ();
            CreateMap<BestemmingDTO, BestemmingDAL> ();

            CreateMap<WerelddeelDAL, WerelddeelDTO> ();
            CreateMap<WerelddeelDTO, WerelddeelDAL> ();

            CreateMap<BoekingDAL, BoekingDTO> ();
            CreateMap<BoekingDTO, BoekingDAL> ();

            CreateMap<ReisDAL, ReisDTO> ();
            CreateMap<ReisDTO, ReisDAL> ();

            CreateMap<WoonplaatsDAL, WoonplaatsDTO> ();
            CreateMap<WoonplaatsDTO, WoonplaatsDAL> ();
        }
    }
}
