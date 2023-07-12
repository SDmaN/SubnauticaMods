using PowerOrder.Configuration;
using System;
using System.Collections.Generic;

namespace PowerOrder.PowerSupliers;

internal sealed class PowerSuplierComparer : IComparer<PowerSuplier>
{
    public static readonly PowerSuplierComparer Instance = new();

    public int Compare(PowerSuplier x, PowerSuplier y)
    {
        var xIndex = GetIndex(x);
        var yIndex = GetIndex(y);

        return Math.Sign(xIndex - yIndex);
    }

    private static int GetIndex(PowerSuplier ps) => ps.Type is null || !Config.Instance.Order.TryGetValue(ps.Type.Value, out var i) ? -1 : i;
}
