using FluentValidation;
using FluentValidation.AspNetCore;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Services;
using JobBoard.Application.Validators;
using JobBoard.Infrastructure.Data;
using JobBoard.Infrastructure.Persistence;
using JobBoard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<JobBoardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JobBoardConnectionString")));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateJobDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateJobDtoValidater>();

builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJobService, JobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
