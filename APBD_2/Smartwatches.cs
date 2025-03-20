namespace APBD_2;
public class SmartWatches: Device, IPowerNotifier
{
    private int _battery;
    public int Battery
    {
        get => _battery;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException();
            _battery = value;
            if (_battery < 20)
            {
                LowPower();
            }
        }
    }
    public SmartWatches(int id, string name, int battery) : base(id, name)
    {
        Battery = battery;
    }
    public void LowPower()
    {
        Console.WriteLine("Low Power, you need to charge your battery");
    }
   
    public override void TurnOn()
    {
        if (Battery < 11)
            throw new EmptyBatteryException($"{Name} cannot be turned on. Battery is too low ({Battery}%).");

        if (!On)
        {
            base.TurnOn();
            Battery -= 10;
        }
    }
    public override string ToString()
    {
        return base.ToString() + $", Battery: {_battery}";
    }
}
public class EmptyBatteryException : Exception
{
    public EmptyBatteryException(string message) : base(message) { }
}