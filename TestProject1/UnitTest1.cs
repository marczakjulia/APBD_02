using APBD_2;
namespace TestProject1;

public class deviceManagertest
{
    private readonly string _testFilePath = "test_devices.txt";
        private DeviceManager _manager;
        private readonly string outputfile = "/Users/juliamarczak/RiderProjects/APBD_2/APBD_2/output.txt";

        public deviceManagertest()
        {
            //creating a file on which i can operate on
            CreateTestFile();
            _manager = new DeviceManager(_testFilePath);
        }

        private void CreateTestFile()
        {
            File.WriteAllLines(_testFilePath, new List<string>
            {
                "SW-1,Apple Watch SE2,true,27%",
                "P-1,LinuxPC,false,Linux Mint",
                "P-2,ThinkPad T440,false",
                "ED-1,Pi3,192.168.1.44,MD Ltd.Wifi-1"
            });
        }

        private int GetDeviceCount() => _manager._devices.Count;

        private Device GetDevice(string devicePrefix, int id)
        {
            foreach (var device in _manager._devices)
            {
                if (device.Id == id &&
                    ((devicePrefix == "SW" && device is SmartWatches) ||
                     (devicePrefix == "P" && device is PC) ||
                     (devicePrefix == "ED" && device is ED)))
                {
                    return device;
                }
            }

            throw new NotFoundException($"Device with ID {id} and type {devicePrefix} not found.");
        }

        [Fact]
        public void GetDevices()
        {
            Assert.Equal(4, GetDeviceCount());
        }

        [Fact]
        public void AddDevice()
        {
            _manager.AddDevice(new SmartWatches(2, "Garmin", 80));
            Assert.Equal(5, GetDeviceCount());
        }

        [Fact]
        public void RemoveDevice_ShouldDecreaseDeviceCount()
        {
            _manager.RemoveDevice("P", 2);
            Assert.Equal(3, GetDeviceCount());
        }

        [Fact]
        public void EditDeviceData()
        {
            _manager.EditDeviceData("P", 1, new object[] { 1, "UpdatedPC", "Ubuntu" });
            var updatedDevice = GetDevice("P", 1);
            Assert.NotNull(updatedDevice);
            Assert.Equal("UpdatedPC", updatedDevice.Name);
            Assert.Equal("Ubuntu", ((PC)updatedDevice).OS);
        }

        [Fact]
        public void TurnOnDevice()
        {
            _manager.TurnOnDevice("SW", 1);
            var device = GetDevice("SW", 1);
            Assert.NotNull(device);
            Assert.True(device.On);
        }

        [Fact]
        public void TurnOffDevice()
        {
            _manager.TurnOffDevice("SW", 1);
            var device = GetDevice("SW", 1);
            Assert.NotNull(device);
            Assert.False(device.On);
        }

        [Fact]
        public void SaveDevices()
        {
            _manager.SaveDevices(outputfile);
            Assert.True(File.Exists(outputfile));
        }
    }
