using System.Collections.Generic;

public class Location
{
    public string Name { get; set; }
    public Location Parent { get; set; }

    public List<Location> ChildLocations { get; set; }    
    public List<File> Files { get; set; }

    public Location(string name, Location parent, List<Location> childLocations, List<File> files)
    {
        Name = name;
        Parent = parent;
        ChildLocations = childLocations;
        Files = files;
    }
}