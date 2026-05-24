using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

[assembly: ModInfo(
    "RedirectFix",
    "redirectfix",
    Description = "Fixes the client crash on server redirect.",
    Version = "1.0.0",
    Side = "Universal",
    RequiredOnClient = true,
    RequiredOnServer = false,
    Authors = new[] { "Tsu (imtsubaki)" })]

namespace RedirectFix;

public class RedirectFixModSystem : ModSystem
{
    private const string HarmonyId = "com.nimbus.redirectfix";
    private Harmony? _harmony;

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void StartClientSide(ICoreClientAPI api)
    {
        _harmony = new Harmony(HarmonyId);
        _harmony.PatchAll(typeof(RedirectFixModSystem).Assembly);
        api.Logger.Notification("[RedirectFix] patched");
    }

    public override void Dispose()
    {
        _harmony?.UnpatchAll(HarmonyId);
        _harmony = null;
    }
}
