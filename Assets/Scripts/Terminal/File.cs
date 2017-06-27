public enum FileType
{
    TEXT, CODE
}

// Will eventually be serialized using a proto-buffer to be transferred between file systems
public class File
{
    public FileType Type { get; set; }
    public string Name { get; set; }
    public float Size { get; set; }

    public string Content { get; set; } // String of text that can be parsed through custom "interpreter" if File is CODE

    // For creating a new file
    public File(string name, string content)
    {
        Name = name;
        Content = content;

        Serialize();
    }
    
    // For deserializing
    public File(string physicalLocation) { }

    private void Serialize() { }
}