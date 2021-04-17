using System.IO.Ports;
using System.Threading.Tasks;
using Artemis.Plugins.Tasmota.Settings;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services;
using FluentValidation;
using Stylet;

namespace Artemis.Plugins.Tasmota.ViewModels.Dialogs
{
    public class DeviceConfigurationDialogViewModel : DialogViewModelBase
    {
        private readonly TasmotaDeviceDefinitions _device;
        private string _name;
        private string _hostname;

        public DeviceConfigurationDialogViewModel(TasmotaDeviceDefinitions device, IModelValidator<DeviceConfigurationDialogViewModel> validator) : base(validator)
        {
            _device = device;

            Name = _device.Name;
            Hostname = _device.Hostname;
        }

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        public string Hostname
        {
            get => _hostname;
            set => SetAndNotify(ref _hostname, value);
        }

        public async Task Accept()
        {
            await ValidateAsync();

            if (HasErrors)
                return;

            if (!string.IsNullOrWhiteSpace(Name))
                _device.Name = Name;
            _device.Hostname = Hostname;

            Session.Close(true);
        }
    }

    public class DeviceConfigurationDialogViewModelValidator : AbstractValidator<DeviceConfigurationDialogViewModel>
    {
        public DeviceConfigurationDialogViewModelValidator()
        {
        }
    }
}