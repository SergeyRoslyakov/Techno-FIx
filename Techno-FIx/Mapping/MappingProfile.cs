using AutoMapper;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Mapping
{
    /// <summary>
    /// Профиль маппинга между сущностями и DTO объектами
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Client mappings
            CreateMap<Client, ClientDTO>()
                .ForMember(dest => dest.DevicesCount,
                    opt => opt.MapFrom(src => src.Devices.Count));
            CreateMap<CreateClientDTO, Client>();
            CreateMap<UpdateClientDTO, Client>();

            // Device mappings
            CreateMap<Device, DeviceDTO>()
                .ForMember(dest => dest.ClientName,
                    opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"));
            CreateMap<CreateDeviceDTO, Device>();

            // Service mappings
            CreateMap<Service, ServiceDTO>()
                .ForMember(dest => dest.OrdersCount,
                    opt => opt.MapFrom(src => src.RepairOrders.Count));
            CreateMap<CreateServiceDTO, Service>();

            // Technician mappings
            CreateMap<Technician, TechnicianDTO>()
                .ForMember(dest => dest.ActiveOrdersCount,
                    opt => opt.MapFrom(src => src.RepairOrders.Count));
            CreateMap<CreateTechnicianDTO, Technician>();
            CreateMap<UpdateTechnicianDTO, Technician>();

            // RepairOrder mappings
            CreateMap<RepairOrder, RepairOrderDTO>()
                .ForMember(dest => dest.DeviceInfo,
                    opt => opt.MapFrom(src => $"{src.Device.Brand} {src.Device.Model}"))
                .ForMember(dest => dest.ServiceName,
                    opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.TechnicianName,
                    opt => opt.MapFrom(src => $"{src.Technician.FirstName} {src.Technician.LastName}"))
                .ForMember(dest => dest.ClientName,
                    opt => opt.MapFrom(src => $"{src.Device.Client.FirstName} {src.Device.Client.LastName}"));
            CreateMap<CreateRepairOrderDTO, RepairOrder>();
            CreateMap<UpdateRepairOrderDTO, RepairOrder>();
        }
    }
}