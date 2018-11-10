using Harmony12;
using UnityModManagerNet;
using System.Reflection;
using System;
using Debug = System.Diagnostics.Debug;
using System.Diagnostics;
using UnityEngine;
using Kingmaker;

namespace KingmakerREPL
{

    public class ModMain
    {
        public static UnityModManager.ModEntry.ModLogger logger;
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugLog(string msg)
        {
            if (logger != null) logger.Log(msg);
        }
        public static bool enabled;
        static Settings settings;
        static ReplManager replManager;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
                var harmony = HarmonyInstance.Create(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                modEntry.OnToggle = OnToggle;
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;
                logger = modEntry.Logger;
                replManager = new GameObject().AddComponent<ReplManager>();
                UnityEngine.Object.DontDestroyOnLoad(replManager.gameObject);
            }
            catch (Exception e)
            {
                DebugLog(e.ToString() + "\n" + e.StackTrace);
                throw e;
            }
            return true;
        }
        // Called when the mod is turned to on/off.
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value /* active or inactive */)
        {
            enabled = value;
            return true; // Permit or not.
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            try
            {
                if (!enabled) return;
                GUILayout.Label("Shift+~ to togle REPL");
            }
            catch (Exception e)
            {
                DebugLog(e.ToString() + "\n" + e.StackTrace);
                throw e;
            }
        }
    }
}
