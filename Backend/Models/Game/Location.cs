using System.Collections.Generic;

namespace Backend.Models.Game;

public class Location
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<string> ConnectedLocationIds { get; set; } = new();}
