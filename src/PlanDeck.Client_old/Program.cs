using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using PlanDeck.Client.Services;

namespace PlanDeck.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddFluentUIComponents();

        var path = builder.HostEnvironment.BaseAddress;
        builder.Services.AddScoped<GrpcChannel>(services => GrpcChannel.ForAddress(path, new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
        }));
        builder.Services.AddTransient<RoomService>();

        await builder.Build().RunAsync();
    }
}