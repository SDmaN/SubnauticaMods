using Nautilus.Options;
using PowerOrder.PowerSupliers;
using System;
using System.Collections.Generic;

namespace PowerOrder.Configuration;

internal sealed class Config
{
    private static readonly Lazy<Config> _lazyInstance = new(() =>
    {
        var config = new Config();
        config.Load();

        return config;
    });

    private static readonly List<PowerSuplierType> DefaultOrder = PowerSuplierTypeExtensions.ToList();

    private readonly Dictionary<PowerSuplierType, int> _currentOrder = new();

    public static Config Instance => _lazyInstance.Value;

    public IReadOnlyDictionary<PowerSuplierType, int> Order => _currentOrder;

    public void Load()
    {
        var configFile = new ConfigFile
        {
            Order = DefaultOrder
        };

        configFile.Load();

        _currentOrder.Clear();

        var index = 0;

        foreach (var item in configFile.Order)
        {
            _currentOrder[item] = index;
            index++;
        }

        Logger.Info($"Config loaded. Config order: {string.Join(", ", configFile.Order)}");
    }

    private sealed class ConfigFile : Nautilus.Json.ConfigFile
    {
        public List<PowerSuplierType> Order;

        public ConfigFile()
        {
        }
    }
}
