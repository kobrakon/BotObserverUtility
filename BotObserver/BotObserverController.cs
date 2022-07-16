using UnityEngine;
using Comfort.Common;
using EFT;

namespace BotObserver
{
    public class BotObserverController : MonoBehaviour
    {
        private static float FrameCount = 0; // evil floating point number
        public static double BotNumber { get; private set; }
        void Update()
        {
            if (Ready())
            {
                FrameCount++; // method gets ran every frame so this is a reliable way to count frames

                if (Plugin.LogBotInfo.Value.IsDown())
                {
                    var gameWorld = Singleton<GameWorld>.Instance;
                    Plugin.logger.LogInfo($"Bot Data Log Request Handled on Frame {Mathf.Floor(FrameCount)}");
                    foreach (Player bot in gameWorld.RegisteredPlayers)
                    {
                        AccessibleBotData.BotPosition = bot.Transform.position;
                        AccessibleBotData.BotName = bot.ProfileId;
                        AccessibleBotData.BotID = bot.FullIdInfoClean;
                        AccessibleBotData.BotInfiltration = bot.Infiltration;
                        AccessibleBotData.BotDistFromPlayer = Vector3.Distance(gameWorld.AllPlayers[0].Transform.position, AccessibleBotData.BotPosition);

                        if (BotNumber == 0)
                        {
                            AccessibleBotData.BotName += "(you)";
                        }

                        switch (bot.Side)
                        {
                            case EPlayerSide.Savage:
                                AccessibleBotData.BotFaction = "Scav";
                                break;
                            case EPlayerSide.Bear:
                                AccessibleBotData.BotFaction = "BEAR";
                                break;
                            case EPlayerSide.Usec:
                                AccessibleBotData.BotFaction = "USEC";
                                break;
                        }

                        switch (bot.HealthStatus)
                        {
                            case ETagStatus.Healthy:
                                AccessibleBotData.BotStatus = "Healthy";
                                break;
                            case ETagStatus.Injured:
                                AccessibleBotData.BotStatus = "Injured";
                                break;
                            case ETagStatus.Dying:
                                AccessibleBotData.BotStatus = "Dying";
                                break;
                        }

                        Plugin.logger.LogInfo
                        (
                            $"BotNumber = {BotNumber} | " +
                            $"BotName = {AccessibleBotData.BotName} | " +
                            $"BotID = {AccessibleBotData.BotID} | " +
                            $"BotFaction = {AccessibleBotData.BotFaction} | " +
                            $"BotStatus = {AccessibleBotData.BotStatus} | " +
                            $"BotPosition = {AccessibleBotData.BotPosition} | " +
                            $"BotInfiltration = {AccessibleBotData.BotInfiltration} | " +
                            $"BotDistFromPlayer = {AccessibleBotData.BotDistFromPlayer}"
                        );

                        BotNumber++;
                    }
                    BotNumber = 0;
                }
            }
        }

        public float FetchActiveFrame() // unit testing to ensure framecounter is advancing properly
        {
            return FrameCount;
        }

        bool Ready() // make sure the gameworld is actually loaded before trying shit
        {
            var gameWorld = Singleton<GameWorld>.Instance;

            if (gameWorld == null || gameWorld.AllPlayers == null || gameWorld.AllPlayers.Count <= 0 || gameWorld.AllPlayers[0] is HideoutPlayer) // gotta make sure shit doesnt run if the player is in the hideout
            {
                return false;
            }
            return true;
        }
    }

    internal struct AccessibleBotData // data values for bots
    { 
        public static Vector3 BotPosition { get; set; }
        public static string BotName { get; set; }
        public static string BotID { get; set; }
        public static string BotStatus { get; set; }
        public static string BotInfiltration { get; set; }
        public static float BotDistFromPlayer { get; set; }
        public static string BotFaction { get; set; }
    }
}
