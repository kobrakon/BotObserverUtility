using UnityEngine;
using Comfort.Common;
using EFT;

namespace BotObserver
{
    public class BotObserverController : MonoBehaviour
    {
        private static float FrameCount = 0; // evil floating point number
        private static GameWorld gameWorld = Singleton<GameWorld>.Instance;
        public static double BotNumber { get; private set; }
        void Update()
        {
            if (Ready())
            {
                FrameCount++; // method gets ran every frame so this is a reliable way to count frames

                if (Plugin.LogBotInfo.Value.IsDown())
                {
                    Plugin.logger.LogInfo($"Bot Data Log Request Handled on Frame {Mathf.Floor(FrameCount)}");
                    foreach (Player bot in gameWorld.AllPlayers)
                    {
                        GetBotData(bot);
                        BotNumber++;
                    }
                    BotNumber = 0;
                }
            }
        }

        public static void GetBotData(Player bot)
        {
            AccessibleBotData.BotPosition = bot.Transform.position;
            AccessibleBotData.BotName = bot.ProfileId;
            AccessibleBotData.BotID = bot.FullIdInfoClean;
            AccessibleBotData.BotInfiltration = bot.Infiltration;
            AccessibleBotData.BotStatus = bot.CurrentStateName;
            AccessibleBotData.BotFaction = bot.Side;
            AccessibleBotData.BotHealth = bot.HealthController.GetBodyPartHealth(EBodyPart.Common).Current;
            AccessibleBotData.BotHealthStatus = bot.HealthStatus;
            AccessibleBotData.BotDistFromPlayer = Vector3.Distance(gameWorld.AllPlayers[0].Transform.position, AccessibleBotData.BotPosition);

            if (BotNumber == 0)
            {
                AccessibleBotData.BotName += "(you)";
            }

            Plugin.logger.LogInfo
            (
                $"BotNumber = {BotNumber} | " +
                $"BotName = {AccessibleBotData.BotName} | " +
                $"BotID = {AccessibleBotData.BotID} | " +
                $"BotFaction = {AccessibleBotData.BotFaction} | " +
                $"BotStatus = {AccessibleBotData.BotStatus} | " +
                $"BotHealth = {AccessibleBotData.BotHealth} | " +
                $"BotHealthStatus = {AccessibleBotData.BotHealthStatus} | " +
                $"BotPosition = {AccessibleBotData.BotPosition} | " +
                $"BotInfiltration = {AccessibleBotData.BotInfiltration} | " +
                $"BotDistFromPlayer = {AccessibleBotData.BotDistFromPlayer}"
            );
        }

        public float FetchActiveFrame() // unit testing to ensure framecounter is advancing properly
        {
            return FrameCount;
        }

        bool Ready() // make sure the gameworld is actually loaded before trying shit
        {
            if (gameWorld == null || gameWorld.AllPlayers == null || gameWorld.AllPlayers.Count <= 0 || gameWorld.AllPlayers[0] is HideoutPlayer) // gotta make sure shit doesnt run if the player is in the hideout
            {
                return false;
            }
            return true;
        }
    }

    internal struct AccessibleBotData // data values for bots
    { 
        public static Vector3 BotPosition { get; internal set; }
        public static string BotName { get; internal set; }
        public static string BotID { get; internal set; }
        public static float BotHealth { get; internal set; }
        public static ETagStatus BotHealthStatus { get; internal set; }
        public static EPlayerState BotStatus { get; internal set; }
        public static string BotInfiltration { get; internal set; }
        public static float BotDistFromPlayer { get; internal set; }
        public static EPlayerSide BotFaction { get; internal set; }
    }
}
