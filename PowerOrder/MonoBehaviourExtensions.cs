using System;

namespace PowerOrder;

internal static class MonoBehaviourExtensions
{
	public static string GetGameObjectName(this MonoBehaviour monoBehaviour)
    {
        return monoBehaviour.gameObject.name;
    }
}
