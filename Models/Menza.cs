using System.Text.Json.Serialization;

public class Restaurant
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("postalCode")]
    public int PostalCode { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("workingHoursDesc")]
    public string WorkingHoursDesc { get; set; }

    [JsonPropertyName("closedHoursDesc")]
    public string ClosedHoursDesc { get; set; }

    [JsonPropertyName("hoursByDay")]
    public HoursByDay HoursByDay { get; set; }
}

public class HoursByDay
{
    [JsonPropertyName("monday")]
    public WorkingHours Monday { get; set; }

    [JsonPropertyName("tuesday")]
    public WorkingHours Tuesday { get; set; }

    [JsonPropertyName("wednesday")]
    public WorkingHours Wednesday { get; set; }

    [JsonPropertyName("thursday")]
    public WorkingHours Thursday { get; set; }

    [JsonPropertyName("friday")]
    public WorkingHours Friday { get; set; }

    [JsonPropertyName("saturday")]
    public WorkingHours Saturday { get; set; }

    [JsonPropertyName("sunday")]
    public WorkingHours Sunday { get; set; }
}

public class WorkingHours
{
    [JsonPropertyName("open")]
    public string Open { get; set; }

    [JsonPropertyName("close")]
    public string Close { get; set; }
}
