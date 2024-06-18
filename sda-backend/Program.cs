using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/stations", (Context db) => db.Stations.ToListAsync())
.WithName("Stations")
.WithOpenApi();

app.MapGet("/stations/{id}",(int id, Context db) => db.Stations.FindAsync(id))
.WithName("Station")
.WithOpenApi();

app.MapGet("/stations/{id}/depatures",(int id, Context db) => db.Journeys.Where(j => j.Departure_Station_Id == id).ToListAsync())
.WithName("Station depatures")
.WithOpenApi();

app.MapGet("/stations/{id}/returns",(int id, Context db) => db.Journeys.Where(j => j.Return_Station_Id == id).ToListAsync())
.WithName("Station returns")
.WithOpenApi();

app.MapGet("/journeys", (Context db) => db.Journeys.ToListAsync())
.WithName("Journeys")
.WithOpenApi();

app.MapGet("/journey/{id}",(int id, Context db) => db.Journeys.FindAsync(id))
.WithName("Journey")
.WithOpenApi();


app.Run();