﻿using Artemis.Plugins.Tasmota.RGB.NET;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;

namespace Artemis.Plugins.Tasmota.Settings
{
    public class TasmotaDeviceDefinitions : AbstractBindable
    {
        private string _name;
        private string _hostname;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Hostname
        {
            get => _hostname;
            set => SetProperty(ref _hostname, value);
        }

        public IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger)
        {
            TasmotaLight light = new();
            light.ConnectAsync(IPAddress.Parse(this.Hostname));
            light.TurnOnAsync();
            light.AutoRefreshEnabled = true;
            yield return new TasmotaDevice(new TasmotaDeviceInfo(RGBDeviceType.LedStripe, this.Name, this.Hostname), new TasmotaUpdateQueue(updateTrigger, light));
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     Represents a basic bindable class which notifies when a property value changes.
    /// </summary>
    public abstract class AbstractBindable : IBindable
    {
        #region Events

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        ///     Checks if the property already matches the desirec value or needs to be updated.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to the backing-filed.</param>
        /// <param name="value">Value to apply.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool RequiresUpdate<T>(ref T storage, T value)
        {
            return !Equals(storage, value);
        }

        /// <summary>
        ///     Checks if the property already matches the desired value and updates it if not.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to the backing-filed.</param>
        /// <param name="value">Value to apply.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners. This value is optional
        ///     and can be provided automatically when invoked from compilers that support <see cref="CallerMemberNameAttribute" />
        ///     .
        /// </param>
        /// <returns><c>true</c> if the value was changed, <c>false</c> if the existing value matched the desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!RequiresUpdate(ref storage, value)) return false;

            storage = value;
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Triggers the <see cref="PropertyChanged" />-event when a a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners. This value is optional
        ///     and can be provided automatically when invoked from compilers that support <see cref="CallerMemberNameAttribute" />
        ///     .
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    /// <summary>
    ///     Represents a basic bindable class which notifies when a property value changes.
    /// </summary>
    public interface IBindable : INotifyPropertyChanged
    {
    }
}