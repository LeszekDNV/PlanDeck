using Blazored.LocalStorage;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlanDeck.Client;
using PlanDeck.Client.Services;
using MudBlazor.Services;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var path = builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped<GrpcChannel>(services => GrpcChannel.ForAddress(path, new GrpcChannelOptions
{
    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
}));
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
}).AddBlazoredLocalStorage();

builder.Services.AddTransient<RoomProxyService>();
builder.Services.AddScoped<IUserLocalStorageService, UserLocalStorageService>();
builder.Services.AddScoped<IPlanningRoomService, PlanningRoomService>();
await builder.Build().RunAsync();
