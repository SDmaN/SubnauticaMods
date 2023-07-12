using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PowerOrder.PowerSupliers;

internal enum PowerSuplierType
{
    RegenPowerCell,
    SolarPanel,
    ThermalPlant,
    PowerTransmitter,
    BioReactor,
    NuclearReactor
}

internal static class PowerSuplierTypeExtensions
{
    public static List<PowerSuplierType> ToList() => Enum.GetValues(typeof(PowerSuplierType)).Cast<PowerSuplierType>().ToList();

    public static string GetFriendlyName(this PowerSuplierType powerSuplierType) => powerSuplierType switch
    {
        PowerSuplierType.RegenPowerCell => "Regen power cell",
        PowerSuplierType.SolarPanel => "Solar panel",
        PowerSuplierType.ThermalPlant => "Thermal plant",
        PowerSuplierType.PowerTransmitter => "Power trnsmitter",
        PowerSuplierType.BioReactor => "Bio reactor",
        PowerSuplierType.NuclearReactor => "Nuclear reactor",
        _ => throw new IndexOutOfRangeException($"Invalid {nameof(powerSuplierType)} value {powerSuplierType}."),
    };
}

internal record struct PowerSuplier
{
    private static readonly Regex NameCleaner = new(@"\(.*?\)");

    private readonly string _powerInterfaceName;
    private string _name;

    public PowerSuplier(IPowerInterface powerInterface)
    {
        PowerInterface = powerInterface;
        _powerInterfaceName = GetPowerInterfaceName(PowerInterface);
        Type = GetPowerSuplierType(_powerInterfaceName);
    }

    public IPowerInterface PowerInterface { get; }
    public PowerSuplierType? Type { get; }

    private static string ClearGameObjectName(string name) => NameCleaner.Replace(name, string.Empty);

    private static string GetPowerInterfaceName(IPowerInterface powerInterface) => ((MonoBehaviour)powerInterface).gameObject.name;

    private static PowerSuplierType? GetPowerSuplierType(string powerInterfaceName)
    {
        var clearedName = ClearGameObjectName(powerInterfaceName);

        return clearedName switch
        {
            "RegenPowerCell" => PowerSuplierType.RegenPowerCell,
            "SolarPanel" => PowerSuplierType.SolarPanel,
            "ThermalPlant" => PowerSuplierType.ThermalPlant,
            "PowerTransmitter" => PowerSuplierType.PowerTransmitter,
            "BaseBioReactorModule" => PowerSuplierType.BioReactor,
            "BaseNuclearReactorModule" => PowerSuplierType.NuclearReactor,
            _ => null
        };
    }

    public override string ToString() => _name ??= GetPowerSuplierName(Type, _powerInterfaceName);

    private static string GetPowerSuplierName(PowerSuplierType? type, string powerInterfaceName) => type.HasValue ? type.ToString() : $"Unknown ({powerInterfaceName})";
}
