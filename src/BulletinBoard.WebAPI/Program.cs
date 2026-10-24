using System.Text.Encodings.Web;
using System.Text.Unicode;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BulletinBoard.Infrastructure;
using BulletinBoard.WebAPI.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// DI: контейнер Autofac замість стандартного. Рівні реєструються модулями з Infrastructure,
// тому PL не створює жодного об'єкта нижніх рівнів сам.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new DataAccessModule(builder.Configuration.GetConnectionString("Board")));
    container.RegisterModule(new BusinessLogicModule());
});

builder.Services
    .AddControllers(options => options.Filters.Add<BusinessExceptionFilter>())
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
