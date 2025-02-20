using Blazored.LocalStorage;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlanDeck.Client;
using PlanDeck.Client.Services;
using MudBlazor.Services;
using MudBlazor;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string path = builder.HostEnvironment.BaseAddress;
builder.Services.AddSingleton(services => GrpcChannel.ForAddress(path, new GrpcChannelOptions
{
    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()),    
}));
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
}).AddBlazoredLocalStorage();

builder.Services.AddTransient<RoomProxyService>();
builder.Services.AddScoped<IUserLocalStorageService, UserLocalStorageService>();
builder.Services.AddScoped<IPlanningRoomService, PlanningRoomService>();
builder.Services.AddSingleton<IConnectionProxyService, ConnectionProxyService>();
await builder.Build().RunAsync();
