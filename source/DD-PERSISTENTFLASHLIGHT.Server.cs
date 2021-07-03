//MIT License

//Copyright (c) 2021 Distinctive Development - https://discord.distinctive-dev.com/

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DD_PERSISTENTFLASHLIGHT.Server
{
    public class DD_FlashLight_server : BaseScript
    {
        public class WeaponInfo
        {
            public string Player { get; set; }
            public uint WeaponHash { get; set; }
            public uint ComponentHash { get; set; }
            public bool FlashLightEnabled { get; set; }
        }

        private readonly List<WeaponInfo> _flashlightList = new List<WeaponInfo>();

        public DD_FlashLight_server()
        {
            Server_Events();
        }

        private void Server_Events()
        {
            EventHandlers["dd_init_flashlight"] += new Action<Player>(Init_Flashlight);
            EventHandlers["dd_flashlight_toggle"] += new Action<Player, uint, uint, bool>(Flashlight_toggle);
            EventHandlers["playerDropped"] += new Action<Player>(PlayerDropped);
        }

        private void Init_Flashlight([FromSource] Player source)
        {
            foreach (var flashlight in _flashlightList)
            {
                TriggerClientEvent(source, "dd_checkFlashlight", flashlight.Player, flashlight.WeaponHash, flashlight.ComponentHash, flashlight.FlashLightEnabled);
            }
        }

        private void Flashlight_toggle([FromSource] Player source, uint weaponHash, uint componentHash, bool flashlightEnabled)
        {
            WeaponInfo search = _flashlightList.Find((WeaponInfo p) => p.Player == source.Handle && p.WeaponHash == weaponHash);
            if (search == null)
            {
                WeaponInfo newFlashLight = new WeaponInfo()
                {
                    Player = source.Handle,
                    WeaponHash = weaponHash,
                    ComponentHash = componentHash,
                    FlashLightEnabled = flashlightEnabled
                };
                _flashlightList.Add(newFlashLight);
                TriggerClientEvent("dd_checkFlashlight", source.Handle, newFlashLight.WeaponHash, newFlashLight.ComponentHash, newFlashLight.FlashLightEnabled);
            }
            else
            {
                search.FlashLightEnabled = flashlightEnabled;
                TriggerClientEvent("dd_checkFlashlight", search.Player, search.WeaponHash, search.ComponentHash, search.FlashLightEnabled);
            }
        }

        private void PlayerDropped([FromSource] Player source)
        {
            foreach (var flashlight in _flashlightList.ToList())
            {
                if (flashlight.Player == source.Handle)
                {
                    _flashlightList.Remove(flashlight);
                    TriggerClientEvent("dd_flashlightRemove", flashlight.Player);
                }
            }
        }
    }
}
