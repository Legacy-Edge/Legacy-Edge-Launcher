# Legacy-Edge-Launcher
This program makes it as simple as a double click (and using UAC one time) to get back to the legacy version of Microsft Edge.   
This tool even works in **20H2** Update!   
<a href="https://github.com/ShortDevelopment/Legacy-Edge-Launcher/releases/latest">Download</a>   
   
## 21H1 Update ⚠
Binaries of Legacy Edge were removed so you have to extract them from a backup of find them on the Internet.   
As far as I know, Edge Chromium now has to be removed (manually).

# How it works
The app sets a registry key as described <a href="https://docs.microsoft.com/en-us/deployedge/microsoft-edge-sysupdate-access-old-edge#side-by-side-experience-with-microsoft-edge-stable-channel-and-microsoft-edge-legacy">here</a> that forces Windows to launch <i>Edge Legacy</i> instead of "<i>The new Edge</i>":   
`Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate`   
`Allowsxs` = `1`   
If you have the new version already installed you have to launch the new edge installer. The new release (v2) now handels this process automatically wile showing a nice waiting dialog.   
   
### 20H2 Update
According to [this post](https://techcommunity.microsoft.com/t5/discussions/microsoft-edge-legacy/m-p/1624481/highlight/true#M34656) Legacy Edge Launcher now also deletes this registry key: `SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\ClientState\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}` -> `BrowserReplacement`   
   
# Tech Stuff
Then the app uses the `IApplicationActivationManager` <i>COM Interface</i> (as describled <a href="https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iapplicationactivationmanager">here</a>) to start <i>Edge Legacy</i> with the following package name:<br/>
`Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge`<br/>
<br/>
You can find the COM-Refrences in <a href="ShObjIdl_core.header">ShObjIdl_core.h(eader)</a> and the app logic in <a href="ApplicationActivationManager.vb">ApplicationActivationManager.vb</a>.<br/>
