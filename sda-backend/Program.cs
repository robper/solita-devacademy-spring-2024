using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/stations", (Context db) => db.Stations.OrderBy(s => s.Id).ToListAsync())
.WithName("Stations")
.WithOpenApi();

app.MapGet("/stations/{id}", (int id, Context db) => db.Stations.FindAsync(id))
.WithName("Station")
.WithOpenApi();

app.MapGet("/stations/{id}/depatures/returnstations", (int id, Context db) =>
    db.Journeys.Where(j => j.Departure_station_id == id)
    .GroupBy(s=> s.Return_station_id).Select(group => new {
        return_station = group.Key,
        count = group.Count()
    }).OrderBy(e => e.count))
.WithName("Station depatures")
.WithOpenApi();

app.MapGet("/stations/{id}/depatures/count", (int id, Context db) =>
    new { count = db.Journeys.Where(j => j.Departure_station_id == id).CountAsync().Result })
.WithName("Nr of station depatures")
.WithOpenApi();

app.MapGet("/stations/{id}/depatures/distance", (int id, Context db) =>
    new { avg = db.Journeys.Where(j => j.Departure_station_id == id).Average(j => j.Distance) })
.WithName("Average depature distance")
.WithOpenApi();

app.MapGet("/stations/{id}/depatures/duration", (int id, Context db) =>
    new { avg = db.Journeys.Where(j => j.Departure_station_id == id).Average(j => j.Duration) })
.WithName("Average depature duration")
.WithOpenApi();

app.MapGet("/stations/{id}/returns/count", (int id, Context db) =>
    new { count = db.Journeys.Where(j => j.Return_station_id == id).CountAsync().Result })
.WithName("Nr of station returns")
.WithOpenApi();

app.MapGet("/health", () =>
{
    return Results.Ok();
});

app.Run();