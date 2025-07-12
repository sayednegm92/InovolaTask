using InovolaTask.Application.Helper;
using InovolaTask.Infrastructure.Context;
using InovolaTask.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    #region Connect To SQL Server
    var connectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(opions => opions.UseSqlServer(connectionString));
    #endregion
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMemoryCache();

    #region Extension Methods
    builder.Services.AddInfrastuctureDependencies(builder.Configuration);
    builder.Services.AddApplicationDependencies();
    #endregion


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
}
