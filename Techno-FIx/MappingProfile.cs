using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Techno_FIx
{
    public class MappingProfile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDTO>()
                .ForMember(dest => dest.DevicesCount,
                    opt => opt.MapFrom(src => src.Devices.Count));
            CreateMap<CreateClientDTO, Client>();
            CreateMap<UpdateClientDTO, Client>();

            CreateMap<Device, DeviceDTO>()
                .ForMember(dest => dest.ClientName,
                    opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"));
            CreateMap<CreateDeviceDTO, Device>();

            CreateMap<Service, ServiceDTO>()
                .ForMember(dest => dest.OrdersCount,
                    opt => opt.MapFrom(src => src.RepairOrders.Count));
            CreateMap<CreateServiceDTO, Service>();

            CreateMap<Technician, TechnicianDTO>()
                .ForMember(dest => dest.ActiveOrdersCount,
                    opt => opt.MapFrom(src => src.RepairOrders.Count));
            CreateMap<CreateTechnicianDTO, Technician>();
            CreateMap<UpdateTechnicianDTO, Technician>();

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
