using BepInEx;
using HarmonyLib;
using PowerOrder.Extensions;
using PowerOrder.PowerSupliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PowerOrder;

[BepInPlugin(PluginInfo.Guid, PluginInfo.ProductName, PluginInfo.Version)]
[BepInDependency(Nautilus.PluginInfo.PLUGIN_GUID)]
internal sealed class Main : BaseUnityPlugin
{
    public void Awake()
    {
        PowerOrder.Logger.SetLogSource(Logger);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.Guid);

        Configuration.Config.Instance.Load();
        PowerOrder.Logger.Info("Active");
    }
}

[HarmonyPatch(typeof(PowerRelay), nameof(PowerRelay.AddInboundPower))]
internal class PowerRelayAddInboundPower
{
    [HarmonyPostfix]
    private static void Postfix(PowerRelay __instance, IPowerInterface powerInterface)
    {
        try
        {
            if (__instance is null || powerInterface is null)
            {
                return;
            }

            var inboundPowerSources = __instance.GetField<List<IPowerInterface>>("inboundPowerSources");

            if (inboundPowerSources is null || !inboundPowerSources.Contains(powerInterface))
            {
                return;
            }

            var addedPs = new PowerSuplier(powerInterface);
            var ordered = inboundPowerSources.Select(x => new PowerSuplier(x)).OrderBy(x => x, PowerSuplierComparer.Instance);

            __instance.SetField("inboundPowerSources", ordered.Select(x => x.PowerInterface).ToList());

            Logger.Info($"Add power source for {__instance.name}: {addedPs}. Current order: {string.Join(", ", ordered)}");
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}

[HarmonyPatch(typeof(PowerRelay), nameof(PowerRelay.RemoveInboundPower))]
internal class PowerRelayRemoveInboundPower
{
    [HarmonyPostfix]
    private static void Postfix(PowerRelay __instance, IPowerInterface powerInterface)
    {
        try
        {
            if (__instance is null || powerInterface is null)
            {
                return;
            }

            var inboundPowerSources = __instance.GetField<List<IPowerInterface>>("inboundPowerSources");

            if (inboundPowerSources is null || inboundPowerSources.Contains(powerInterface))
            {
                return;
            }

            var removedPs = new PowerSuplier(powerInterface);
            var supliers = inboundPowerSources.Select(x => new PowerSuplier(x));

            Logger.Info(
                $"Remove power source for {__instance.gameObject.name}: {removedPs}. Current order: {string.Join(", ", supliers)}");
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}
