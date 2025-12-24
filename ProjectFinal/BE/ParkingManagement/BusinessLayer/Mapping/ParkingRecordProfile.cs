using AutoMapper;
using BusinessLayer.DTOs.ParkingFeeRules;
using BusinessLayer.DTOs.ParkingSlot;
using BusinessLayer.DTOs.Vehicles;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.DTOs.Parking.ParkingDTOs;

namespace BusinessLayer.Mapping
{
    public class ParkingRecordProfile : Profile
    {
        public ParkingRecordProfile()
        {
            CreateMap<ParkingRecord, ParkingRecordDto>()
                .ForMember(dest => dest.PlateNumber,
                    opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
                .ForMember(dest => dest.VehicleType,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleType.Name))
                .ForMember(dest => dest.ParkingSlotCode,
                    opt => opt.MapFrom(src => src.ParkingSlot != null
                        ? src.ParkingSlot.SlotCode
                        : null));

            CreateMap<ParkingFeeRule, ParkingFeeRuleDto>();
            CreateMap<ParkingFeeRuleCreateDto, ParkingFeeRule>();
            CreateMap<ParkingFeeRuleUpdateDto, ParkingFeeRule>();

            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleCreateDto, Vehicle>();

            CreateMap<ParkingSlot, ParkingSlotDto>();
            CreateMap<ParkingSlotCreateDto, ParkingSlot>();

            CreateMap<MonthlyPass, MonthlyPassDto>();
            CreateMap<MonthlyPassCreateDto, MonthlyPass>();

        }
    }

}
