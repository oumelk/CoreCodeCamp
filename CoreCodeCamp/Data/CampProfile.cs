using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {

        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>();
        }
    }
}
