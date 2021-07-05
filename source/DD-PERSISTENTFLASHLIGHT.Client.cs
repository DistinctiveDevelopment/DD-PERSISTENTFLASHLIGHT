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
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace DD_PERSISTENTFLASHLIGHT
{
    public class DD_FlashLight_client : BaseScript
    {
        public class WeaponInfo
        {
            public string Player { get; set; }
            public int PlayerID { get; set; }
            public uint WeaponHash { get; set; }
            public uint ComponentHash { get; set; }
            public bool FlashLightEnabled { get; set; }
        }

        public class Config
        {
            public bool Taserflashlight { get; set; }
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }
            public int Distance { get; set; }
            public int Brightness { get; set; }
            public int Hardness { get; set; }
            public int Radius { get; set; }
            public int Falloff { get; set; }
        }

        public static Dictionary<uint, uint> WeaponHashes = new Dictionary<uint, uint>
        {
            //TASER
            { 0x3656C8C1, 0x359B7AAE }, //WEAPON_STUNGUN - COMPONENT_AT_PI_FLSH

            //PISTOLS
            { 0x1B06D571, 0x359B7AAE }, //WEAPON_PISTOL - COMPONENT_AT_PI_FLSH
            { 0xBFE256D4, 0x43FD595B }, //WEAPON_PISTOL_MK2 - COMPONENT_AT_PI_FLSH_02
            { 0x5EF9FEC4, 0x359B7AAE }, //WEAPON_COMBATPISTOL - COMPONENT_AT_PI_FLSH
            { 0x99AEEB3B, 0x359B7AAE }, //WEAPON_PISTOL50 - COMPONENT_AT_PI_FLSH
            { 0x88374054, 0x4A4965F3 }, //WEAPONG_SNSPISTOL_MK2 - COMPONENT_AT_PI_FLSH_03
            { 0xD205520E, 0x359B7AAE }, //WEAPON_HEAVYPISTOL - COMPONENT_AT_PI_FLSH
            { 0xCB96392F, 0x359B7AAE }, //WEAPON_REVOLVER_MK2 - COMPONENT_AT_PI_FLSH
            { 0x22D8FE39, 0x359B7AAE }, //WEAPON_APPISTOL - COMPONENT_AT_PI_FLSH

            //SUBMACHINE GUNS
            { 0x13532244, 0x359B7AAE }, //WEAPON_MICROSMG - COMPONENT_AT_AR_FLSH
            { 0x2BE6766B, 0x7BC4CDDC }, //WEAPON_SMG - COMPONENT_AT_AR_FLSH
            { 0x78A97CD0, 0x7BC4CDDC }, //WEAPON_SMG_MK2 - COMPONENT_AT_AR_FLSH
            { 0xEFE7E2DF, 0x7BC4CDDC }, //WEAPON_ASSAULTSMG - COMPONENT_AT_AR_FLSH
            { 0x0A3D4D34, 0x7BC4CDDC }, //WEAPON_COMBATPDW - COMPONENT_AT_AR_FLSH

            //ASSAULT RIFLES
            { 0xBFEFFF6D, 0x7BC4CDDC }, //WEAPON_ASSAULTRIFLE - COMPONENT_AT_AR_FLSH
            { 0x394F415C, 0x7BC4CDDC }, //WEAPON_ASSAULTRIFLE_MK2 - COMPONENT_AT_AR_FLSH
            { 0x83BF0278, 0x7BC4CDDC }, //WEAPON_CARBINERIFLE - COMPONENT_AT_AR_FLSH
            { 0xFAD1F1C9, 0x7BC4CDDC }, //WEAPON_CARBINERIFLE_MK2 - COMPONENT_AT_AR_FLSH
            { 0xAF113F99, 0x7BC4CDDC }, //WEAPON_ADVANCEDRIFLE - COMPONENT_AT_AR_FLSH
            { 0xC0A3098D, 0x7BC4CDDC }, //WEAPON_SPECIALCARBINE - COMPONENT_AT_AR_FLSH
            { 0x969C3D67, 0x7BC4CDDC }, //WEAPON_SPECIALCARBINE_MK2  - COMPONENT_AT_AR_FLSH
            { 0x7F229F94, 0x7BC4CDDC }, //WEAPON_BULLPUPRIFLE - COMPONENT_AT_AR_FLSH
            { 0x84D6FAFD, 0x7BC4CDDC }, //WEAPON_BULLPUPRIFLE_MK2  - COMPONENT_AT_AR_FLSH

            //SHOTGUNS
            { 0x1D073A89, 0x7BC4CDDC }, //WEAPON_PUMPSHOTGUN - COMPONENT_AT_AR_FLSH
            { 0x555AF99A, 0x7BC4CDDC }, //WEAPON_PUMPSHOTGUN_MK2 - COMPONENT_AT_AR_FLSH
        };
        public static Dictionary<uint, Vector3[]> FlashlightVectors = new Dictionary<uint, Vector3[]>
        {
            { 0x7BC4CDDC, new Vector3[] { new Vector3(0.5f, 0.03f, 0.05f), new Vector3(1.0f, -0.16f, 0.145f)}}, //COMPONENT_AT_AR_FLSH
            { 0x359B7AAE, new Vector3[] { new Vector3(0.28f, 0.04f, 0.0f), new Vector3(1.0f, -0.12f, 0.05f) }}, //COMPONENT_AT_PI_FLSH
            { 0x43FD595B, new Vector3[] { new Vector3(0.28f, 0.04f, 0.0f), new Vector3(1.0f, -0.135f, 0.05f)}}, //COMPONENT_AT_PI_FLSH_02
            { 0x4A4965F3, new Vector3[] { new Vector3(0.28f, 0.04f, 0.0f), new Vector3(1.0f, -0.135f, 0.05f)}}, //COMPONENT_AT_PI_FLSH_03
        };

        private Config _config = new Config();

        private readonly List<WeaponInfo> _weaponList = new List<WeaponInfo>();
        private readonly List<WeaponInfo> _flashlightList = new List<WeaponInfo>();

        private uint _currentWeaponInt;
        private WeaponInfo _currentWeapon;

        public DD_FlashLight_client()
        {
            LoadConfig();
            Flashlight_client_eventhandlers();
            Init_Flashlights();

            Debug.WriteLine("DD-PERSISTENTFLASHLIGHT V1.1.2. LOADED");
        }

        [Tick]
        internal async Task OnTick()
        {
            GetCurrentPlayerWeapons();
            FlashLightToggle();
            Render_Flashlight();

            await Task.FromResult(0);
        }

        private void LoadConfig()
        {
            string json = LoadResourceFile(GetCurrentResourceName(), "config.json") ?? "[]";
            _config = JsonConvert.DeserializeObject<Config>(json);
        }

        private void Flashlight_client_eventhandlers()
        {
            EventHandlers["dd_checkFlashlight"] += new Action<string, uint, uint, bool>(CheckFlashlight);
            EventHandlers["dd_flashlightRemove"] += new Action<string>(RemoveFlashlight);
        }

        private void Init_Flashlights()
        {
            TriggerServerEvent("dd_init_flashlight");
        }

        private void CheckFlashlight(string player, uint weaponHash, uint componentHash, bool flashlightEnabled)
        {
            WeaponInfo search = _flashlightList.Find((WeaponInfo p) => p.Player == player && p.WeaponHash == weaponHash);
            if (search == null)
            {
                int.TryParse(player, out int playerid);
                int playerServerId = GetPlayerFromServerId(playerid);
                WeaponInfo newFlashLight = new WeaponInfo()
                {
                    Player = player,
                    PlayerID = playerid,
                    WeaponHash = weaponHash,
                    ComponentHash = componentHash,
                    FlashLightEnabled = flashlightEnabled
                };
                _flashlightList.Add(newFlashLight);
            }
            else
            {
                search.FlashLightEnabled = flashlightEnabled;
            }
        }

        private void RemoveFlashlight(string player)
        {
            List<WeaponInfo> allWeapons = new List<WeaponInfo>();
            foreach (var item in _flashlightList)
            {
                if (item.Player == player)
                {
                    allWeapons.Add(item);
                }
            }
            if (allWeapons.Count > 0)
            {
                foreach (var item in allWeapons)
                {
                    _flashlightList.Remove(item);
                }
            }
        }

        private void GetCurrentPlayerWeapons()
        {
            Ped playerPed = Game.PlayerPed;
            if (!IsPedInAnyVehicle(playerPed.Handle, false) && IsPedArmed(playerPed.Handle, 4))
            {
                uint current = (uint)GetSelectedPedWeapon(GetPlayerPed(-1));
                if (current != _currentWeaponInt && current != 0)
                {
                    _currentWeaponInt = current;
                    if (_currentWeaponInt == GetHashKey("WEAPON_STUNGUN") && !_config.Taserflashlight)
                    {
                        _currentWeapon = null;
                        return;
                    }

                    if (WeaponHashes.ContainsKey(_currentWeaponInt))
                    {
                        WeaponInfo search = _weaponList.Find((WeaponInfo w) => w.WeaponHash == _currentWeaponInt);

                        if (search == null)
                        {
                            WeaponHashes.TryGetValue(_currentWeaponInt, out uint componentHash);
                            if (HasPedGotWeaponComponent(GetPlayerPed(-1), _currentWeaponInt, componentHash) || (_currentWeaponInt == GetHashKey("WEAPON_STUNGUN") && _config.Taserflashlight))
                            {
                                WeaponInfo newWeapon = new WeaponInfo()
                                {
                                    WeaponHash = _currentWeaponInt,
                                    ComponentHash = componentHash,
                                    FlashLightEnabled = false
                                };
                                _weaponList.Add(newWeapon);
                                _currentWeapon = newWeapon;
                            }
                        }
                        else
                        {
                            _currentWeapon = search;
                        }
                    }
                    else if (_currentWeapon != null)
                    {
                        _currentWeapon = null;
                    }
                }
            }
        }

        private void FlashLightToggle()
        {
            DisableControlAction(0, 54, true);
            Ped playerPed = Game.PlayerPed;
            if (!IsPedInAnyVehicle(playerPed.Handle, true) && IsDisabledControlJustPressed(0, 54) && _currentWeapon != null)
            {
                if (HasPedGotWeaponComponent(playerPed.Handle, _currentWeapon.WeaponHash, _currentWeapon.ComponentHash) || (_currentWeaponInt == 0x3656C8C1 && _config.Taserflashlight))
                {
                    if (_currentWeapon.FlashLightEnabled)
                    {
                        _currentWeapon.FlashLightEnabled = false;
                        PlaySoundFrontend(-1, "COMPUTERS_MOUSE_CLICK", "", true);
                    }
                    else if (!_currentWeapon.FlashLightEnabled)
                    {
                        _currentWeapon.FlashLightEnabled = true;
                        PlaySoundFrontend(-1, "COMPUTERS_MOUSE_CLICK", "", true);
                    }
                    TriggerServerEvent("dd_flashlight_toggle", _currentWeapon.WeaponHash, _currentWeapon.ComponentHash, _currentWeapon.FlashLightEnabled);
                }
            }
        }

        private void Render_Flashlight()
        {
            foreach (var flashlight in _flashlightList)
            {
                if (flashlight.FlashLightEnabled && flashlight.PlayerID !=0)
                {
                    int sourcePed = GetPlayerPed(GetPlayerFromServerId(flashlight.PlayerID));
                    if ((uint)GetSelectedPedWeapon(sourcePed) == flashlight.WeaponHash && !IsPedInAnyVehicle(sourcePed, true))
                    {
                        Vector3[] vectors = FlashlightVectors[flashlight.ComponentHash];
                        if (vectors != null)
                        {
                            Vector3 Pos = GetPedBoneCoords(sourcePed, 0xDEAD, vectors[0].X, vectors[0].Y, vectors[0].Z);
                            Vector3 Dir = GetPedBoneCoords(sourcePed, 0xDEAD, vectors[1].X, vectors[1].Y, vectors[1].Z);
                            Vector3 DirVector = Dir - Pos;
                            float VectorMag = Vmag2(DirVector.X, DirVector.Y, DirVector.X);
                            Vector3 PosEnd = new Vector3(DirVector.X / VectorMag, DirVector.Y / VectorMag, DirVector.Z / VectorMag);
                           
                            DrawSpotLight(
                                Pos.X,
                                Pos.Y,
                                Pos.Z,
                                PosEnd.X,
                                PosEnd.Y,
                                PosEnd.Z,
                                _config.Red,
                                _config.Green,
                                _config.Blue,
                                _config.Distance,
                                _config.Brightness,
                                _config.Hardness,
                                _config.Radius,
                                _config.Falloff);
                        }
                    }
                }
            }
        }
    }
}
