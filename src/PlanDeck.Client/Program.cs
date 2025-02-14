using Blazored.LocalStorage;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlanDeck.Client;
using PlanDeck.Client.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var path = builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped<GrpcChannel>(services => GrpcChannel.ForAddress(path, new GrpcChannelOptions
{
    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
}));
builder.Services.AddMudServices().AddBlazoredLocalStorage();

builder.Services.AddTransient<RoomProxyService>();
builder.Services.AddScoped<IUserLocalStorageService, UserLocalStorageService>();
await builder.Build().RunAsync();
