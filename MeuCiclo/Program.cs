using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MeuCiclo.Application.Commands;
using MeuCiclo.Domain.Interfaces;
using MeuCiclo.Infrastructure.Persistence;
using MeuCiclo.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactCors", policy =>
    {
        policy
            .WithOrigins("https://meuciclo.devporwillie.shop")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<MeuCicloDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICicloRepository, CicloRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCicloCommand).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CreateCicloCommand).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("ReactCors");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
