using FluentValidation;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Validators;
using JobBoard.Infrastructure.Data;
using JobBoard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidatorsFromAssemblyContaining<CreateJobDtoValidator>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<JobBoardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JobBoardConnectionString")));



builder.Services.AddScoped<IJobRepository, JobRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
