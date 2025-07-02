using CSharp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<TimeEntryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();
