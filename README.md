# Legacy-Edge-Launcher
This program makes it as simple as a double click (and using UAC one time) to get back to the legacy version of Microsft Edge (Spartan).   

> **Note**   
> Currently this application does not work any more!

> **Warning**   
> The legacy version does no longer receive security updates and is lacking a lot of widely used features!

<a href="https://github.com/ShortDevelopment/Legacy-Edge-Launcher/releases/latest">Download</a>   

# How it works
 - The app enables [Side-by-Side mode](https://docs.microsoft.com/en-us/deployedge/microsoft-edge-sysupdate-access-old-edge#side-by-side-experience-with-microsoft-edge-stable-channel-and-microsoft-edge-legacy)
`Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate`   
`Allowsxs` = `1`   
 - According to [this post](https://techcommunity.microsoft.com/t5/discussions/microsoft-edge-legacy/m-p/1624481/highlight/true#M34656) Legacy Edge Launcher now also deletes this registry key: `SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\ClientState\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}` -> `BrowserReplacement`  
 - The legacy edge binaries have to be copied back
 - The new edge has to be uninstalled (Beta Chromium & Legacy Version may be installed side-by-side)
   
## Tech Stuff
Then the app uses the [`IApplicationActivationManager` interface](https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iapplicationactivationmanager) to start <i>Edge Legacy</i> with the following `appUserModelId`: `Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge`