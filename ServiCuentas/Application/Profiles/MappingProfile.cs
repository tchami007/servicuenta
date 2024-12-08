using AutoMapper;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Model;

namespace ServiCuentas.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Cuenta, CuentaDTO>().ReverseMap();
            CreateMap<Movimiento, MovimientoDTO>().ReverseMap();
        }
    }
}
