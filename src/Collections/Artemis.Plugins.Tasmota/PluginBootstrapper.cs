using Artemis.Core;
using Artemis.Plugins.Tasmota.ViewModels;
using Artemis.UI.Shared;

namespace Artemis.Plugins.Tasmota
{
    public class PluginBootstrapper : IPluginBootstrapper
    {
        public void Enable(Plugin plugin)
        {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<TasmotaConfigurationViewModel>();
        }

        public void Disable(Plugin plugin)
        {
        }
    }
}