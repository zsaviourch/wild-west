<<<<<<< Updated upstream
/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID BOSSSTART = 2978173602U;
        static const AkUniqueID BUTTONCLICK = 4051332235U;
        static const AkUniqueID BUTTONHOVER = 3035572085U;
        static const AkUniqueID BUTTONSTARTGAME = 282606109U;
        static const AkUniqueID CREDITSSTART = 2994446543U;
        static const AkUniqueID GOATATTACK = 720364476U;
        static const AkUniqueID GOATDIE = 404655026U;
        static const AkUniqueID GOATHURT = 2445642675U;
        static const AkUniqueID GOLEMATTACK = 4272317039U;
        static const AkUniqueID GOLEMDIE = 838703051U;
        static const AkUniqueID GOLEMHURT = 2573158568U;
        static const AkUniqueID GOLEMSHIELDHIT = 3112440327U;
        static const AkUniqueID GUNEMPTY = 147436440U;
        static const AkUniqueID GUNRELOAD = 323245414U;
        static const AkUniqueID GUNSHOOT = 3194616450U;
        static const AkUniqueID INTENSITYHIGH = 272640944U;
        static const AkUniqueID INTENSITYLOW = 1343576922U;
        static const AkUniqueID INTENSITYMID = 773284954U;
        static const AkUniqueID LAMIAATTACK = 1609923659U;
        static const AkUniqueID LAMIADIE = 3060973567U;
        static const AkUniqueID LAMIAHURT = 4077017716U;
        static const AkUniqueID LEVEL1START = 3438030758U;
        static const AkUniqueID LEVEL2START = 192279183U;
        static const AkUniqueID MUSICSTART = 1122283870U;
        static const AkUniqueID PLAYERDASH = 2525052962U;
        static const AkUniqueID PLAYERDIE = 3966601280U;
        static const AkUniqueID PLAYERENERGYEMPTY = 2374849433U;
        static const AkUniqueID PLAYERENERGYLOW = 1839772676U;
        static const AkUniqueID PLAYERFOOTSTEP = 3542290436U;
        static const AkUniqueID PLAYERHEALTHLOW = 583027834U;
        static const AkUniqueID PLAYERHURT = 3537581393U;
        static const AkUniqueID SNIPEREXPLODE = 734204211U;
        static const AkUniqueID SOULDROP = 2940614623U;
        static const AkUniqueID SOULPICKUP = 3739246548U;
        static const AkUniqueID TIKBALANGATTACK = 2408912248U;
        static const AkUniqueID TIKBALANGDIE = 2437003014U;
        static const AkUniqueID TIKBALANGHURT = 382555423U;
        static const AkUniqueID VICTORYSTART = 1460238139U;
        static const AkUniqueID WENDIGODIE = 1753832348U;
        static const AkUniqueID WENDIGOHURT = 4294852885U;
        static const AkUniqueID WENDIGOMISSILE = 2286643954U;
        static const AkUniqueID WENDIGOSHIELDHIT = 947858520U;
        static const AkUniqueID WENDIGOSLASH = 1823178165U;
        static const AkUniqueID WENDIGOSMASH = 4157141718U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace MUSICREGIONS
        {
            static const AkUniqueID GROUP = 2753803885U;

            namespace STATE
            {
                static const AkUniqueID MUSICBOSS = 2935806393U;
                static const AkUniqueID MUSICCREDITS = 435872092U;
                static const AkUniqueID MUSICLEVEL1 = 4206939873U;
                static const AkUniqueID MUSICLEVEL2 = 4206939874U;
                static const AkUniqueID MUSICMENU = 4082046343U;
                static const AkUniqueID MUSICVICTORY = 1359346036U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MUSICREGIONS

        namespace PLAYERLIFE
        {
            static const AkUniqueID GROUP = 444815956U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PLAYERALIVE = 2557321869U;
                static const AkUniqueID PLAYERDEFEATED = 818777268U;
            } // namespace STATE
        } // namespace PLAYERLIFE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace MUSICINTENSITY
        {
            static const AkUniqueID GROUP = 1301299809U;

            namespace SWITCH
            {
                static const AkUniqueID HIGHINT = 3420765524U;
                static const AkUniqueID LOWINT = 3325438800U;
                static const AkUniqueID MIDINT = 487624844U;
            } // namespace SWITCH
        } // namespace MUSICINTENSITY

        namespace PICKUPSWITCH
        {
            static const AkUniqueID GROUP = 2159605653U;

            namespace SWITCH
            {
                static const AkUniqueID HEALTHSWITCH = 3237077415U;
                static const AkUniqueID SOULSWITCH = 3942412836U;
            } // namespace SWITCH
        } // namespace PICKUPSWITCH

        namespace TERRAINMATERIAL
        {
            static const AkUniqueID GROUP = 461894747U;

            namespace SWITCH
            {
                static const AkUniqueID SANDTERRAIN = 887657946U;
                static const AkUniqueID WINTERTERRAIN = 1110108097U;
                static const AkUniqueID WOODTERRAIN = 45539941U;
            } // namespace SWITCH
        } // namespace TERRAINMATERIAL

        namespace WEAPONSWITCH
        {
            static const AkUniqueID GROUP = 2107435045U;

            namespace SWITCH
            {
                static const AkUniqueID GUNREVOLVER = 2548468192U;
                static const AkUniqueID GUNRIFLE = 163030769U;
                static const AkUniqueID GUNSHOTGUN = 831777997U;
                static const AkUniqueID GUNSNIPER = 998971168U;
            } // namespace SWITCH
        } // namespace WEAPONSWITCH

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID DISTANCE = 1240670792U;
        static const AkUniqueID MUSICVOLUME = 2346531308U;
        static const AkUniqueID ROFINCREASE = 2426350174U;
        static const AkUniqueID SFXVOLUME = 988953028U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID ENVIRONMENT1 = 3761286873U;
        static const AkUniqueID ENVIRONMENT2 = 3761286874U;
        static const AkUniqueID ENVIRONMENTBOSS = 863592867U;
        static const AkUniqueID GAMEPLAYLOOP = 1646894747U;
        static const AkUniqueID TITLESCREEN = 152105657U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID DIALOGUE = 3930136735U;
        static const AkUniqueID ENEMIES = 2242381963U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID NON_WORLD = 838047381U;
        static const AkUniqueID OBJECTS = 1695690031U;
        static const AkUniqueID PLAYER = 1069431850U;
        static const AkUniqueID UI = 1551306167U;
        static const AkUniqueID VOICE = 3170124113U;
        static const AkUniqueID WORLD = 2609808943U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
=======
/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID CRYSTALRIFLESHOT = 4170654817U;
        static const AkUniqueID EMPTYCLIP2 = 2528183560U;
        static const AkUniqueID GOATATTACK = 720364476U;
        static const AkUniqueID GOATDIE = 404655026U;
        static const AkUniqueID GOATHURT = 2445642675U;
        static const AkUniqueID GOLEMATTACK = 4272317039U;
        static const AkUniqueID GOLEMDIE = 838703051U;
        static const AkUniqueID GOLEMHURT = 2573158568U;
        static const AkUniqueID GUNRELOAD = 323245414U;
        static const AkUniqueID GUNSHOOT = 3194616450U;
        static const AkUniqueID LAMIAATTACK = 1609923659U;
        static const AkUniqueID LAMIADIE = 3060973567U;
        static const AkUniqueID LAMIAHURT = 4077017716U;
        static const AkUniqueID PLAYERDASH = 2525052962U;
        static const AkUniqueID PLAYERDIE = 3966601280U;
        static const AkUniqueID PLAYERENERGYEMPTY = 2374849433U;
        static const AkUniqueID PLAYERENERGYLOW = 1839772676U;
        static const AkUniqueID PLAYERENERGYMAX = 1605033336U;
        static const AkUniqueID PLAYERHEALTHLOW = 583027834U;
        static const AkUniqueID PLAYERHEALTHMAX = 4173482238U;
        static const AkUniqueID PLAYERHEALTHPICKUP = 3292114096U;
        static const AkUniqueID PLAYERHURT = 3537581393U;
        static const AkUniqueID SNIPEREXPLODE = 734204211U;
        static const AkUniqueID SOULDROP = 2940614623U;
        static const AkUniqueID SOULPICKUP = 3739246548U;
        static const AkUniqueID TIKBALANGATTACK = 2408912248U;
        static const AkUniqueID TIKBALANGDIE = 2437003014U;
        static const AkUniqueID TIKBALANGHURT = 382555423U;
        static const AkUniqueID WENDIGOCHARGE = 1016938702U;
        static const AkUniqueID WENDIGODIE = 1753832348U;
        static const AkUniqueID WENDIGOHURT = 4294852885U;
        static const AkUniqueID WENDIGOMISSILE = 2286643954U;
        static const AkUniqueID WENDIGOSLAM = 3438274769U;
        static const AkUniqueID WENDIGOSLASH = 1823178165U;
    } // namespace EVENTS

    namespace SWITCHES
    {
        namespace WEAPONSWITCH
        {
            static const AkUniqueID GROUP = 2107435045U;

            namespace SWITCH
            {
                static const AkUniqueID GUNREVOLVER = 2548468192U;
                static const AkUniqueID GUNRIFLE = 163030769U;
                static const AkUniqueID GUNSHOTGUN = 831777997U;
                static const AkUniqueID GUNSNIPER = 998971168U;
            } // namespace SWITCH
        } // namespace WEAPONSWITCH

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID DISTANCE = 1240670792U;
        static const AkUniqueID MUSICVOLUME = 2346531308U;
        static const AkUniqueID ROFINCREASE = 2426350174U;
        static const AkUniqueID SFXVOLUME = 988953028U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID ENVIRONMENT1 = 3761286873U;
        static const AkUniqueID ENVIRONMENT2 = 3761286874U;
        static const AkUniqueID ENVIRONMENTBOSS = 863592867U;
        static const AkUniqueID GAMEPLAYLOOP = 1646894747U;
        static const AkUniqueID NEW_GAMEPLAYLOOP = 56277894U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID DIALOGUE = 3930136735U;
        static const AkUniqueID ENEMIES = 2242381963U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID NON_WORLD = 838047381U;
        static const AkUniqueID PLAYER = 1069431850U;
        static const AkUniqueID UI = 1551306167U;
        static const AkUniqueID VOICE = 3170124113U;
        static const AkUniqueID WORLD = 2609808943U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
>>>>>>> Stashed changes
