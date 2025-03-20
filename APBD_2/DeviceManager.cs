using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace APBD_2
{
    public class DeviceManager
    {
        public List<Device> _devices = new List<Device>();
        private string _filePath;
        private int _maxStorage = 15;

        public DeviceManager(string filePath)
        {
            _filePath = filePath;
            GetDevices();
        }
        public void GetDevices()
{
    if (!File.Exists(_filePath))
    {
        throw new FileNotFoundException($"File not found: {_filePath}");
    }

    var lines = File.ReadAllLines(_filePath);
    foreach (var line in lines)
    {
        if (string.IsNullOrWhiteSpace(line)) continue; 

        try
        {
            string trimmedLine = line.Trim();
            int firstComma = trimmedLine.IndexOf(',');
            if (firstComma == -1) continue; 
            
            string prefix = trimmedLine.Substring(0, firstComma).Trim();
            string data = trimmedLine.Substring(firstComma + 1).Trim();

            Device device;

            if (prefix.StartsWith("SW-"))
            {
                int id = int.Parse(prefix.Substring(3));
                string[] parts = data.Split(',');
                if (parts.Length < 3) continue; 

                string name = parts[0].Trim();
                bool isOn = bool.Parse(parts[1].Trim());
                int battery = int.Parse(parts[2].TrimEnd('%'));

                device = new SmartWatches(id, name, battery);
                if (isOn) device.TurnOn();
            }
            else if (prefix.StartsWith("P-"))
            {
                int id = int.Parse(prefix.Substring(2));
                string[] parts = data.Split(',');
                if (parts.Length < 2) continue;

                string name = parts[0].Trim();
                string os = (parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2].Trim())) 
                    ? parts[2].Trim() 
                    : "Not Installed"; 

                device = new PC(id, name, os);
            }
            else if (prefix.StartsWith("ED-"))
            {
                int id = int.Parse(prefix.Substring(3));
                string[] parts = data.Split(',');
                if (parts.Length < 3) continue;
                string name = parts[0].Trim();
                string ip = parts[1].Trim();
                string network = parts[2].Trim();

                device = new ED(id, name, ip, network);
            }
            else
            {
                Console.WriteLine($"Invalid entry: {line}");
                continue;
            }

            if (_devices.Any(d => d.Id == device.Id && d.GetType() == device.GetType()))
            {
                Console.WriteLine("Duplicate ID");
                continue;
            }

            if (_devices.Count < _maxStorage)
                _devices.Add(device);
            else
                Console.WriteLine("Cannot add more devices.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in line: {line} - {ex.Message}");
        }
    }
}
        public void AddDevice(Device device)
        {
            if (_devices.Count >= _maxStorage)
                throw new InvalidOperationException("Cannot add more devices.");

            _devices.Add(device);
        }
        public void RemoveDevice(string devicePrefix, int id)
        {
            foreach (var device in _devices)
            {
                if (device.Id == id &&
                    ((devicePrefix == "SW" && device is SmartWatches) ||
                     (devicePrefix == "P" && device is PC) ||
                     (devicePrefix == "ED" && device is ED)))
                {
                    _devices.Remove(device);
                    return;
                }
            }
        }

        public void ShowDevices()
        {
            foreach (var device in _devices)
                Console.WriteLine(device);
        }

        public void SaveDevices(String outputPath)
        {
            try
            {
                File.WriteAllLines(outputPath, _devices.Select(d => d.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //i had to use object[] since the values are different like int or string so array wouldnt d0
        public void EditDeviceData(string devicePrefix, int id, object[] newValues)
        {
            foreach (var device in _devices)
            {
                if (device.Id == id &&
                    ((devicePrefix == "SW" && device is SmartWatches) ||
                     (devicePrefix == "P" && device is PC) ||
                     (devicePrefix == "ED" && device is ED)))
                {
                    try
                    {
                        switch (device)
                        {
                            case SmartWatches sw:
                                if (newValues.Length < 3) throw new UpdateException("Invalid number of values.");
                                sw.Name = (string)newValues[1];
                                sw.Battery = (int)newValues[2];
                                break;

                            case PC pc:
                                if (newValues.Length < 3) throw new UpdateException("Invalid number of values.");
                                pc.Name = (string)newValues[1];
                                pc.OS = (string)newValues[2];
                                break;

                            case ED ed:
                                if (newValues.Length < 4) throw new UpdateException("Invalid number of values.");
                                ed.Name = (string)newValues[1];
                                ed.IP = (string)newValues[2];
                                ed.Network = (string)newValues[3];
                                break;

                            default:
                                throw new UpdateException("Unknown device type.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new UpdateException($"Failed to update device: {ex.Message}");
                    }
                }
            }
        }

        public void TurnOnDevice(string devicePrefix, int id)
        {
            foreach (var device in _devices)
            {
                if (device.Id == id &&
                    ((devicePrefix == "SW" && device is SmartWatches) ||
                     (devicePrefix == "P" && device is PC) ||
                     (devicePrefix == "ED" && device is ED)))
                {
                    device.TurnOn();
                    return; 
                }
            }
            throw new NotFoundException("Device not found.");
        }

    public void TurnOffDevice(string devicePrefix, int id)
        {
            foreach (var device in _devices)
            {
                if (device.Id == id &&
                    ((devicePrefix == "SW" && device is SmartWatches) ||
                     (devicePrefix == "P" && device is PC) ||
                     (devicePrefix == "ED" && device is ED)))
                {
                    device.TurnOff();
                    return; 
                }
            }
            throw new NotFoundException("Device not found.");
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UpdateException : Exception
    {
        public UpdateException(string message) : base(message) { }
    }
}

