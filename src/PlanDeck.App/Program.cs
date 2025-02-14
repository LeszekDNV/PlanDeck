using Microsoft.EntityFrameworkCore;
using PlanDeck.Application.Interfaces;
using PlanDeck.Infrastructure.Persistence;
using PlanDeck.Infrastructure.Persistence.Repositories;
using ProtoBuf.Grpc.Server;
using PlanDeck.App.GrpcServices;

namespace PlanDeck.App;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging().EnableDetailedErrors());

        builder.Services.AddScoped<DbContext, AppDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped(typeof(IKeyRepository<,>), typeof(KeyRepository<,>));

        builder.Services.AddCodeFirstGrpc(config =>
        {
            config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();


        app.MapFallbackToFile("index.html");


        // Configure the HTTP request pipeline.
        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
        app.MapGrpcService<GrpcRoomService>();
        //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        using (AppDbContext? dbContext = app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>())
        {
            dbContext?.Database.Migrate();
        }

        app.Run();
    }
}