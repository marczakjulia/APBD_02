namespace APBD_2
{
    public abstract class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool On { get; set; }

        public Device(int id, string name)
        {
            Id = id;
            Name = name;
            On = false;
        }

        public virtual void TurnOn()
        {
            On = true;
            Console.WriteLine($"{Name} is ON.");
        }

        public void TurnOff()
        {
            On = false;
            Console.WriteLine($"{Name} is OFF.");
        }

        public override string ToString()
        {
            return $"Device [ID: {Id}, Name: {Name}, Is Turned On: {On}]";
        }
    }
}