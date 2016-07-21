using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;
using System.Threading.Tasks;

namespace CreateDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=**.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=****";
        static string deviceIdTemplate = "Device-";
        const int count = 12;

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);

            string deviceId, deviceIdVal = string.Empty;

            for (int num = 1; num < count; num++)
            {
                deviceIdVal = ("0000" + num.ToString());
                deviceIdVal = deviceIdVal.Substring(deviceIdVal.Length - 5);

                deviceId = $"{deviceIdTemplate}{deviceIdVal}";

                AddDeviceAsync(deviceId).Wait();
            }
            Console.ReadLine();
        }

        private static async Task AddDeviceAsync(string deviceId)
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
