namespace APBD_2;

public class PC: Device
{
    private string _OS = string.Empty;   //wrote because of error when i run Warning CS8618
    public string OS
    {
        get => _OS;
        set
        {
            _OS = (string.IsNullOrWhiteSpace(value)) ? "Not Installed" : value;
        }
    }

    public PC(int id, string name, string os) : base(id, name)
    {
        OS = os;
    }
    public override void TurnOn()
    {
        if (OS == "Not Installed")
            throw new EmptySystemException($"{Name} cannot be turned on. No OS installed.");

        base.TurnOn();
    }

    public override string ToString()
    {
        return base.ToString() + $", OS: {OS}";
    }
}
public class EmptySystemException : Exception
{
    public EmptySystemException(string message) : base(message) { }
}