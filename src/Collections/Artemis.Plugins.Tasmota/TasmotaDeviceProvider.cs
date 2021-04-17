using Artemis.Core;
using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using Artemis.Plugins.Tasmota.RGB.NET;
using Artemis.Plugins.Tasmota.Settings;
using RGB.NET.Core;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Artemis.Plugins.Tasmota
{
    // This is your Artemis device provider, all it really does is act as a bridge between RGB.NET and Artemis
    // You will not write any device logic in here, refer to the RgbNetDeviceProvider project instead    
    public class TasmotaDeviceProvider : DeviceProvider
    {
        private readonly ILogger _logger;
        private readonly IRgbService _rgbService;
        private readonly PluginSettings _settings;

        public TasmotaDeviceProvider(IRgbService rgbService, ILogger logger, PluginSettings settings) : base(TasmotaRGBDeviceProvider.Instance)
        {
            _rgbService = rgbService;
            _logger = logger;
            _settings = settings;
        }

        public override void Enable()
        {
            PluginSetting<List<TasmotaDeviceDefinitions>> definitions = _settings.GetSetting("TasmotaDeviceDefinitions", new List<TasmotaDeviceDefinitions>());

            foreach (TasmotaDeviceDefinitions deviceDefinition in definitions.Value)
            {
                TasmotaRGBDeviceProvider.Instance.DeviceDefinitions.Add(deviceDefinition);
            }

            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }


        public override void Disable()
        {
            if (_settings.GetSetting("TurnOffLedsOnShutdown", false).Value)
                TurnOffLeds();

            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            TasmotaRGBDeviceProvider.Instance.Dispose();
        }

        private void TurnOffLeds()
        {
            // Disable the LEDs on every device before we leave
            foreach (IRGBDevice rgbDevice in RgbDeviceProvider.Devices)
            {
                ListLedGroup _ = new(_rgbService.Surface, rgbDevice)
                {
                    Brush = new SolidColorBrush(new Color(0, 0, 0)),
                    ZIndex = 999
                };
            }

            // Don't wait for the next update, force one now and flush all LEDs for good measure
            _rgbService.Surface.Update(true);
            // Give the update queues time to process
            Thread.Sleep(200);

            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
        }
    }
}