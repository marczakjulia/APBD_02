using System;
using System.Text.RegularExpressions;

namespace APBD_2
{
    public class ED : Device
    {
        private string _ip = string.Empty;
        private string _network = string.Empty;

        // took from the internet, unsure how to properlly format the regrex for it 
        private static readonly Regex _regex = new Regex(
            @"^((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$",
            RegexOptions.Compiled);

        public string IP
        {
            get => _ip;
            set
            {
                if (!_regex.IsMatch(value))
                    throw new ArgumentException($"Invalid IP address format: {value}");
                
                _ip = value;
            }
        }

        public string Network
        {
            get => _network;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Network name cannot be empty.");
                
                _network = value;
            }
        }

        public ED(int id, string name, string ip, string network) : base(id, name)
        {
            IP = ip;         // Uses the property setter for validation
            Network = network;
        }

        public void Connect()
        {
            if (!Network.Contains("MD Ltd."))
                throw new ConnectionException($"{Name} cannot connect. Invalid network name!");

            Console.WriteLine($"{Name} successfully connected to {Network}.");
        }

        public override void TurnOn()
        {
            Connect();
            base.TurnOn();
        }

        public override string ToString()
        {
            return base.ToString() + $", IP: {IP}, Network: {Network}";
        }
    }

    public class ConnectionException : Exception
    {
        public ConnectionException(string message) : base(message) { }
    }
}