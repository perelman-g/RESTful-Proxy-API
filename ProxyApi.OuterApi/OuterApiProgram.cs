using ProxyApi.OuterApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IInnerApiClient>(s =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7053")
    };

    return new InnerApiClient(httpClient);
});

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

#pragma warning disable S3903 // Types should be defined in named namespaces
public class OuterApiProgram { }
#pragma warning restore S3903 // Types should be defined in named namespaces
