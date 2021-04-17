using Artemis.Plugins.Tasmota.Settings;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Plugins.Tasmota.RGB.NET
{
    public class TasmotaRGBDeviceProvider : AbstractRGBDeviceProvider
    {
        #region Constructors

        public TasmotaRGBDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(TasmotaRGBDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Properties & Fields

        private static TasmotaRGBDeviceProvider _instance;

        public List<TasmotaDeviceDefinitions> DeviceDefinitions { get; } = new();

        public static TasmotaRGBDeviceProvider Instance => _instance ?? new TasmotaRGBDeviceProvider();


        #endregion

        #region Methods

        public void AddDeviceDefinition(TasmotaDeviceDefinitions deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

        protected override void InitializeSDK() { }

        public override void Dispose()
        {
            base.Dispose();

            DeviceDefinitions.Clear();
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            int i = 0;
            foreach (TasmotaDeviceDefinitions deviceDefinition in DeviceDefinitions)
            {
                IDeviceUpdateTrigger updateTrigger = GetUpdateTrigger(i++);
                foreach (IRGBDevice device in deviceDefinition.CreateDevices(updateTrigger))
                    yield return device;
            }
        }

        #endregion
    }
}
