var builder = WebApplication.CreateBuilder(args);

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


app.MapGet("/stations/{id}",(int id) => new Station() { Id = id })
.WithName("Station")
.WithOpenApi();

app.MapGet("/stations", () => new List<Station>{})
.WithName("Stations")
.WithOpenApi();

app.MapGet("/journey/{id}",(int id) => new Journey() { Id = id })
.WithName("Journey")
.WithOpenApi();

app.MapGet("/journeys", () => new List<Journey>{})
.WithName("Journeys")
.WithOpenApi();

app.Run();
class Station
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Station_Name { get; set; }
    public string? Station_Address { get; set; }
    public string? Coordinate_X { get; set; }
    public string? Coordinate_Y { get; set; }
}
class Journey
{
    public int Id { get; set; }
    public DateTime? Departure_Date_Time { get; set; }
    public DateTime? Return_Date_Time { get; set; }
    public int Departure_Station_Id { get; set; }
    public int Return_Station_Id { get; set; }
    public int? Distance { get; set; }
    public int? Duration { get; set; }
}