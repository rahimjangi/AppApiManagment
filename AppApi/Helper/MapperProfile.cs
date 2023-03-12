using AppApi.Models;
using AutoMapper;

namespace AppApi.Helper;

public static class MapperProfile
{
    public static Mapper Initialize()
    {
        var config = new MapperConfiguration(cnf =>
        {
            cnf.CreateMap<Users,UsersDto>().ReverseMap();
        });

        return new Mapper(config);
    }
}
