using BepInEx;
using UnityEngine;
using BepInEx.Logging;
using BepInEx.Configuration;

namespace BotObserver
{
    [BepInPlugin("com.kobrakon.botobserver", "kobrakon-botobserverutility", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ConfigEntry<KeyboardShortcut> LogBotInfo;
        public static ManualLogSource logger;
        private void Awake()
        {
            LogBotInfo = Config.Bind("KeyBinds", "LogBotInfo", new KeyboardShortcut(KeyCode.Keypad5));
            logger = Logger;
            GameObject Hook = new GameObject("BotObserver");
            Hook.AddComponent<BotObserverController>();
            DontDestroyOnLoad(Hook);
            Logger.LogInfo("BotObserver loaded");
        }
    }
}
