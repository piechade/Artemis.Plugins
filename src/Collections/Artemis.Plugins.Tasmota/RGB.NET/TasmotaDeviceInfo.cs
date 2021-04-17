using RGB.NET.Core;

namespace Artemis.Plugins.Tasmota.RGB.NET
{
    public class TasmotaDeviceInfo : IRGBDeviceInfo
    {
        public RGBDeviceType DeviceType { get; }

        public string DeviceName { get; }

        public string IpAddress { get; }

        public string Manufacturer => "Tasmota";

        public string Model { get; }

        public object? LayoutMetadata { get; set; }

        internal TasmotaDeviceInfo(RGBDeviceType deviceType, string model, string ipAddress)
        {
            this.DeviceType = deviceType;
            this.Model = model;
            this.IpAddress = IpAddress;

            DeviceName = "Tasmota Led Strip";
        }
    }
}
