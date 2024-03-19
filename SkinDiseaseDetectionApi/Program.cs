using SkinDiseaseDetectionApi.Models;
using SkinDiseaseDetectionApi.Service;
using SkinDiseaseDetectionApi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddSingleton<JwtTokenManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDiagnoseService, DiagnoseService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("SkinDetectionClient", httpClient =>
{
    httpClient.BaseAddress = new Uri("http://127.0.0.1:5000");
});
builder.Services.AddCors(o => o.AddPolicy("test", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

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

app.UseCors("test");

app.Run();
