using Menza.WebApi.Middleware;
using Menza.WebApi.Repository;
using Menza.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<OsijekMenzeReader>();
builder.Services.AddScoped<DataNormalizer>();
builder.Services.AddScoped<MenzeRespository>();
builder.Services.AddScoped<MenuRepository>();

builder.Services.AddHttpClient<OsijekMenzeReader>((serviceProvider, client) =>
{
    client.BaseAddress = new Uri("http://www.stucos.unios.hr/upload");
});

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
