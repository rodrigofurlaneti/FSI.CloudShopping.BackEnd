using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Authentication;
using FSI.CloudShopping.Domain.Entities;

namespace FSI.CloudShopping.Application.Mappings
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<Authentication, AuthenticationDTO>()
                .ConstructUsing(src => new AuthenticationDTO(
                    src.Id,
                    src.Email,
                    "", 
                    src.AuthorizedAccess, 
                    src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                ));

            CreateMap<AuthenticationDTO, Authentication>()
                .ConstructUsing(dto => new Authentication(
                    dto.Id,
                    dto.Email,
                    dto.isAuthenticationAccess,
                    DateTime.Parse(dto.CreatedAt)
                ));
        }
    }
}