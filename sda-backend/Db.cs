using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

class Context : DbContext
{
    public DbSet<Station> Stations { get; set; }
    public DbSet<Journey> Journeys { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var username = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PW");

        Console.WriteLine("DBHOST: " + host);
        
        optionsBuilder.UseNpgsql($"Host={host};Port={port};Database=citybike;Username={username};Password={password}");
    }
}
[Table("station")]
public class Station
{
    [Column("id")]
    public int Id { get; set; }
    [Column("station_name")]
    public string? Station_name { get; set; }
    [Column("station_address")]
    public string? Station_address { get; set; }
    [Column("coordinate_x")]
    public string? Coordinate_x { get; set; }
    [Column("coordinate_y")]
    public string? Coordinate_y { get; set; }
}
[Table("journey")]
public class Journey
{
    [Column("id")]
    public int Id { get; set; }
    [Column("departure_date_time")]
    public DateTime? Departure_date_time { get; set; }
    [Column("return_date_time")]
    public DateTime? Return_date_time { get; set; }
    [Column("departure_station_id")]
    public int? Departure_station_id { get; set; }
    [Column("return_station_id")]
    public int? Return_station_id { get; set; }
    [Column("distance")]
    public int? Distance { get; set; }
    [Column("duration")]
    public int? Duration { get; set; }
}