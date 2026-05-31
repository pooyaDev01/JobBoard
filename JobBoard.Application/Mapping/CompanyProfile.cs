using AutoMapper;
using JobBoard.Application.DTOs.Companies;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Mapping
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CreateCompanyDto, Company>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAtUtc, o => o.Ignore());


            CreateMap<UpdateCompanyDto, Company>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UpdatedAtUtc, o => o.Ignore())
                .ForMember(d => d.CreatedAtUtc, o => o.Ignore());

            CreateMap<Company, CompanyDto>();
        }
    }
}
