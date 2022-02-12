# DD-PERSISTENTFLASHLIGHT - Distinctive Development

A **FiveM** resource coded in **C#** to get a persistent flashlight in game.

The resource disables the current default GTA5 flashlight and uses the Fivem Native: DrawSpotlight to create flashlights for all players who have them enabled on the gun (same hotkey: “E”). Everything is synced with other players and the script remembers if the flashlight was on the particular gun when you get it out again, for example entering and exiting a car.

Join our Discord if you are looking for support or just want to talk!
https://discord.distinctive-dev.com/

FiveM forum post https://forum.cfx.re/t/release-c-free-distinctive-development-persistent-flashlight/

<p align="center">
  <a href="https://www.youtube.com/watch?v=VHMngrDW_rM">
    <img src="https://forum.cfx.re/uploads/default/original/4X/1/9/1/1914b0c575f00932a67499453de9d145fb86590a.jpeg" target="_blank" width="500" >
  </a>
</p>

## Installation
1. Download the latest release [HERE](https://github.com/DistinctiveDevelopment/DD-PERSISTENTFLASHLIGHT/releases "DD-PERSISTENTFLASHLIGHT Releases")
2. Move the "DD-PERSISTENTFLASHLIGHT" folder to your server resources.
3. Add “DD-PERSISTENTFLASHLIGHT” to your server.cfg with start or ensure
4. **Optional** Configure the config
5. Start the resource or reboot the server

## CONFIG
taserflashlight: true/false (enable or disable the flashlight on the taser, use lowercase)
The other variables are from the Fivem native [DrawSpotlight](https://docs.fivem.net/natives/?_0xD0F64B265C8C8B33) (except pos and dir) and can be set in the config to change the flashlight to your own preference. Variables are set to INT in the code, so use whole numbers in the config, decimals won't work.

## Source Code
The source code is included inside the "source" folder

## Feedback
Any feedback on the resource is appreciated, this includes bugs, suggestions and improvements to the code.

## Screenshots
Some screenshots of the resource in game.
<img src="https://distinctive-dev.com/github/images/DD-PERSISTENTFLASHLIGHT/1.png" target="_blank" width="800" >
<img src="https://distinctive-dev.com/github/images/DD-PERSISTENTFLASHLIGHT/2.png" target="_blank" width="800" >
<img src="https://distinctive-dev.com/github/images/DD-PERSISTENTFLASHLIGHT/3.png" target="_blank" width="800" >

## Changelog
**V1.1.2.**
- Fixed bug with script getting the wrong ped

**V1.1.1**
- FIXED issue that the script was not working

**V1.1.0**
- Code improvements to enhance performance
- Fixed some weaponhashes
- Random fixes

**V1.0.0**
- Initial release (not on github)
