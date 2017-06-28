using UnityEngine;
using UnityEditor;

public class FileSystem
{
    public Location CurrentLocation { get; set; }

    public FileSystem()
    {
        CurrentLocation = new Location("C:", null, null, null, true);
    }
}