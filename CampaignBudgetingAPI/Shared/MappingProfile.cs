using AutoMapper;
using CampaignBudgetingAPI.Api.DTOs.Requests;
using CampaignBudgetingAPI.Api.DTOs.Responses;
using CampaignBudgetingAPI.Models.DTOs;
using CampaignBudgetingAPI.Models.Entities;

namespace CampaignBudgetingAPI.Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de Requisição (DTO -> Entidade)
            CreateMap<UserCreateRequest, User>();
            CreateMap<UserUpdateRequest, User>();

            // Mapeamento de Resposta (Entidade -> DTO)
            CreateMap<User, UserResponse>();

            CreateMap<ClientCreateRequest, Client>();
            CreateMap<ClientUpdateRequest, Client>();
            CreateMap<Client, ClientResponse>();

            CreateMap<CampaignCreateRequest, Campaign>();
            CreateMap<CampaignUpdateRequest, Campaign>();
            CreateMap<Campaign, CampaignResponse>();


        }
    }
}