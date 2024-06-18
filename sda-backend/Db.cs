using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

class Context : DbContext
{
    public DbSet<Station> Stations { get; set; }
    public DbSet<Journey> Journeys { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=citybike;Username=academy;Password=academy");
}
[Table("station")]
public class Station
{
    [Column("id")]
    public int Id { get; set; }
    [Column("station_name")]
    public string? Station_Name { get; set; }
    [Column("station_address")]
    public string? Station_Address { get; set; }
    [Column("coordinate_x")]
    public string? Coordinate_X { get; set; }
    [Column("coordinate_y")]
    public string? Coordinate_Y { get; set; }
}
[Table("journey")]
public class Journey
{
    [Column("id")]
    public int Id { get; set; }
    [Column("departure_date_time")]
    public DateTime? Departure_Date_Time { get; set; }
    [Column("return_date_time")]
    public DateTime? Return_Date_Time { get; set; }
    [Column("departure_station_id")]
    public int? Departure_Station_Id { get; set; }
    [Column("return_station_id")]
    public int? Return_Station_Id { get; set; }
    [Column("distance")]
    public int? Distance { get; set; }
    [Column("duration")]
    public int? Duration { get; set; }
}