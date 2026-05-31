using AutoMapper;
using JobBoard.Application.DTOs.Jobs;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JobBoard.Application.Mapping
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<CreateJobDto, Job>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAtUtc, o => o.Ignore())
                .ForMember(d => d.UpdatedAtUtc, o => o.Ignore())
                .ForMember(d => d.IsActive, o => o.Ignore());

            CreateMap<UpdateJobDto, Job>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAtUtc, o => o.Ignore())
                .ForMember(d => d.UpdatedAtUtc, o => o.Ignore());

            CreateMap<Job, JobDto>()
                .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.Name));
        }
    }
}
