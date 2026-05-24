# RedirectFix

A small Vintage Story client mod that fixes the crash when a server sends you a redirect packet (`Packet_ServerRedirect`, id 29).

## The bug

Vanilla `ClientMain.ExitAndSwitchServer` sets `exitToMainMenu = true` and `RedirectTo` at the same time. Later, `DestroyGameSession` sees `exitToMainMenu` is already set and bails before calling `Dispose()`. The next `ClientMain` instance then dies registering the `selectionhandbook` hotkey (duplicate key) or NREs in `ShapeTesselator`.

## The fix

A Harmony prefix swaps the vanilla code path for the same one `.reconnect` uses: set `doReconnect = true`, leave `exitToMainMenu` alone, let `GuiScreenRunningGame.Reconnect()` dispose cleanly and then `StartGame()` on the cached connect data.

## Install

Drop `RedirectFix` into your `VintagestoryData/Mods/` folder. Client-only, no server install needed.

## Build

```
dotnet build -c Release
```

Expects Vintage Story installed at `A:\VintageStory\` and `VintagestoryAPI.dll` / `VintagestoryLib.dll` available next to the project. Adjust the paths in `RedirectFix.csproj` if yours differ.

## Notes

The reconnect path reuses the cached connect data from when the session started. If you connect through a proxy (e.g. Nimbus), the proxy is what the client reconnects to, and the proxy is responsible for routing you to the actual redirect target.
