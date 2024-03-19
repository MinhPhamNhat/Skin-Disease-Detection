using Cropper.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using SkinDiseaseDetectionApp;
using SkinDiseaseDetectionApp.HttpClients;
using SkinDiseaseDetectionApp.HttpClients.Interfaces;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddScoped<ISkinDetectionClient, SkinDetectionClient>();

builder.Services.AddHttpClient("SkinDetectionClient", httpClient =>
{
    httpClient.BaseAddress =new Uri("http://localhost:5037/");
});
builder.Services.AddCropper();
await builder.Build().RunAsync();
