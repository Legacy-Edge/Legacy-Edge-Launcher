# Legacy-Edge-Launcher
This program makes it as simple as a double click (and using UAC one time) to get back to the legacy version of Microsft Edge.<br/>
<a href="https://github.com/ShortDevelopment/Legacy-Edge-Launcher/releases/latest">Download</a>

# How it works
The app sets a registry key as described <a href="https://docs.microsoft.com/en-us/deployedge/microsoft-edge-sysupdate-access-old-edge#side-by-side-experience-with-microsoft-edge-stable-channel-and-microsoft-edge-legacy">here</a> that forces Windows to launch <i>Edge Legacy</i> instead of "<i>The new Edge</i>":<br/>
`Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate`<br/>
`Allowsxs` = `1`<br/>
<br/>
Then the app uses the `IApplicationActivationManager` <i>COM Interface</i> (as describled <a href="https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iapplicationactivationmanager">here</a>) to start <i>Edge Legacy</i> with the following package name:<br/>
`Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge`<br/>
<br/>
You can find the COM-Refrences in <a href="ShObjIdl_core.header">ShObjIdl_core.h(eader)</a> and the app logic in <a href="Program.vb">Program.vb</a>.<br/>
<br/>
Have fun!
