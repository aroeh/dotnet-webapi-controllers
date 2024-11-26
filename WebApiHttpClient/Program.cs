using WebApiHttpClient.HttpClientHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// register http clients for the app
string restuarantBaseAddress = Environment.GetEnvironmentVariable("RESTUARANT_API") ?? "http://localhost";
Uri uri = new(restuarantBaseAddress);
// add a typed HttpClient
builder.Services.AddHttpClient<HttpClientHelper>(c => c.BaseAddress = uri);

// add a named IHttpClientFactory
builder.Services.AddHttpClient("restuarantClient", client =>
{
    client.BaseAddress = uri;
});
builder.Services.AddTransient<HttpFactoryHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
