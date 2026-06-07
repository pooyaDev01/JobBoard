using FluentValidation;
using FluentValidation.AspNetCore;
using JobBoard.API.Contracts.Errors;
using JobBoard.API.Middleware;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Mapping;
using JobBoard.Application.Services;
using JobBoard.Application.Validators.Jobs;
using JobBoard.Infrastructure.Data;
using JobBoard.Infrastructure.Persistence;
using JobBoard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<JobBoardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JobBoardConnectionString")));

var licenseKey = builder.Configuration["AutoMapper:LicenseKey"];
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = licenseKey;
}, 
typeof(CompanyProfile).Assembly,
typeof(JobProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateJobDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateJobDtoValidater>();

builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
options.InvalidModelStateResponseFactory = context =>
{
    var errors = context.ModelState
    .Where(e => e.Value?.Errors.Count > 0)
    .ToDictionary(k => k.Key,
    k => k.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

    var response = new ApiErrorResponse
    {
        TraceId = context.HttpContext.TraceIdentifier,
        Status = StatusCodes.Status400BadRequest,
        Title = "One or more validation errors occured",
        Detail = "Please refer to the errors property for additional details.",
        Errors = errors
    };

    return new BadRequestObjectResult(response);

});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
