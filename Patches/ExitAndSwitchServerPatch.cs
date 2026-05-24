using HarmonyLib;
using Vintagestory.API.Server;
using Vintagestory.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace RedirectFix.Patches;

// Vanilla ExitAndSwitchServer sets exitToMainMenu=true alongside RedirectTo, which makes DestroyGameSession early-return and skip Dispose.
// The next ClientMain then dies registering the selectionhandbook hotkey or NREs in ShapeTesselator. 
// Route through the .reconnect path instead it disposes cleanly first, then StartGame's with the cached connectData.
[HarmonyPatch(typeof(ClientMain), nameof(ClientMain.ExitAndSwitchServer))]
public static class ExitAndSwitchServerPatch
{
    [HarmonyPrefix]
    public static bool Prefix(ClientMain __instance, MultiplayerServerEntry redirect)
    {
        if (__instance.IsSingleplayer)
        {
            __instance.Platform.ExitSinglePlayerServer(EnumExitMode.SoftExit);
        }

        __instance.RedirectTo = redirect;
        __instance.doReconnect = true;
        // do NOT set exitToMainMenu, that's the vanilla bug!!

        return false;
    }
}
