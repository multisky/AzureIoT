using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Azure.Devices.Client;

namespace taeyoIoTDevice
{
    class Program
    {
        private static System.Timers.Timer SensorTimer;
        private const string DeviceConnectionString = "HostName=***.azure-devices.net;SharedAccessKeyName=device;SharedAccessKey=***";
        private const string DeviceID = "Device-00001";

        private static DeviceClient SensorDevice = null;
        private static DummySensor Sensor = new DummySensor();

        static void Main(string[] args)
        {
            SetTimer();

            SensorDevice = DeviceClient.CreateFromConnectionString(DeviceConnectionString, DeviceID);

            if (SensorDevice == null)
            {
                Console.WriteLine("Failed to create DeviceClient!");
                SensorTimer.Stop();
            }

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            SensorTimer.Stop();
            SensorTimer.Dispose();
        }

        private static void SetTimer()
        {
            SensorTimer = new Timer(2000);
            SensorTimer.Elapsed += SensorTimer_Elapsed;
            SensorTimer.AutoReset = true;
            SensorTimer.Enabled = true;
        }

        private static async void SensorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            await SendEvent();
            await ReceiveCommands();
        }

        // IoT Hub로 메시지를 보내는 메서드
        static async Task SendEvent()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Sensor.GetWetherData(DeviceID));

            Console.WriteLine(json);

            Message eventMessage = new Message(Encoding.UTF8.GetBytes(json));
            await SensorDevice.SendEventAsync(eventMessage);
        }

        // IoT Hub에서 보낸 메시지를 수신하는 메서드
        // 메시지 쏘는 것은 Device Explorer에서 하면 된다.
        static async Task ReceiveCommands()
        {
            Message receivedMessage;
            string messageData;

            receivedMessage = await SensorDevice.ReceiveAsync(TimeSpan.FromSeconds(1));

            if (receivedMessage != null)
            {
                messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                Console.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);

                await SensorDevice.CompleteAsync(receivedMessage);
            }
        }
    }
}
