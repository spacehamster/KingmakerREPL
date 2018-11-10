using Harmony12;
using Kingmaker;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uREPL;

namespace KingmakerREPL
{
    class ReplManager : MonoBehaviour
    {
        GameObject urepl;
        void Start()
        {
        }
        void Init()
        {
            if (urepl == null)
            {
                AssetHelper.bundlePath = "Mods/KingmakerREPL/AssetBundles/urepl";
                var prefab = AssetHelper.Load<GameObject>("uREPL/Prefabs/uREPL.prefab");
                urepl = UnityEngine.Object.Instantiate<GameObject>(prefab);
                var window = urepl.GetComponent<uREPL.Window>();
                var main = urepl.GetComponent<uREPL.Main>();
                if (main.parameters == null)
                {
                    ModMain.DebugLog("Error Deserializing, missing parameters");
                    main.parameters = new Parameters();
                }
                if (main.editor == null)
                {
                    ModMain.DebugLog("Error Deserializing, missing editor");
                    main.editor = new EditorParameters();
                }
                return;
            }
        }
        void Update()
        {
            try
            {
                if (Input.GetKeyUp(KeyCode.BackQuote)
                    && (Input.GetKey(KeyCode.LeftShift)
                    || Input.GetKey(KeyCode.RightShift)))
                {
                    if (urepl == null)
                    {
                        Init();
                        return;
                    }
                    var window = urepl.GetComponent<Window>();
                    if (window.isOpen)
                    {
                        Game.Instance.Keyboard.Disabled.SetValue(false);
                        window.Close();
                    }
                    else
                    {
                        Game.Instance.Keyboard.Disabled.SetValue(true);
                        window.Open();
                    }
                }
            } catch(Exception ex)
            {
                ModMain.DebugLog(ex.ToString() + "\n" + ex.StackTrace);
            }
        }
    }
}
