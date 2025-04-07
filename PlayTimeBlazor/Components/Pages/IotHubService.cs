using System.Text;
using Microsoft.Azure.Devices;

namespace PlayTimeBlazor.Components.Pages;

public class IoTHubService
{
    private readonly ServiceClient _serviceClient;
    private const string ConnectionString = "HostName=playtime-iot-hub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=Ncz0inQ2uVL69A6ELain08vIhyCTP/UvUAIoTHdg6iw=";

    public IoTHubService()
    {
        _serviceClient = ServiceClient.CreateFromConnectionString(ConnectionString);
    }

    public async Task SendCloudToDeviceMessage(string deviceId, string message)
    {
        var commandMessage = new Message(Encoding.ASCII.GetBytes(message));
        await _serviceClient.SendAsync(deviceId, commandMessage);
    }
}