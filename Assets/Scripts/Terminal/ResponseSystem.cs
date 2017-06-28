
public class ResponseSystem
{
    public FileSystem FileSystem;

    public ResponseSystem()
    {
        FileSystem = new FileSystem();
    }

    public string MakeCommand(string command)
    {
        return "No commands available. Try again later.";
    }
}