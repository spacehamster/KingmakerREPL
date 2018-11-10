using Harmony12;
using Kingmaker;
using Kingmaker.UI.SettingsUI;
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
        KeyCode[] primaryCameraKeys = new KeyCode[4] { KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None };
        KeyCode[] secondaryCameraKeys = new KeyCode[4] { KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None };
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
                uREPL.Mono.Run("using Kingmaker;");
                return;
            }
        }
        void DisableCamera()
        {
            var cameraKeys = new SettingsEntityKeybind[]
            {
                SettingsRoot.Instance.CameraUp,
                SettingsRoot.Instance.CameraDown,
                SettingsRoot.Instance.CameraLeft,
                SettingsRoot.Instance.CameraRight
            };
            for(int i = 0; i < cameraKeys.Length; i++)
            {
                primaryCameraKeys[i] = cameraKeys[i].GetBinding(0).Key;
                secondaryCameraKeys[i] = cameraKeys[i].GetBinding(1).Key;
                cameraKeys[i].GetBinding(0).Key = KeyCode.None;
                cameraKeys[i].GetBinding(1).Key = KeyCode.None;
            }
        }
        void EnableCamera()
        {
            var cameraKeys = new SettingsEntityKeybind[]
            {
                SettingsRoot.Instance.CameraUp,
                SettingsRoot.Instance.CameraDown,
                SettingsRoot.Instance.CameraLeft,
                SettingsRoot.Instance.CameraRight
            };
            for (int i = 0; i < cameraKeys.Length; i++)
            {
                cameraKeys[i].GetBinding(0).Key = primaryCameraKeys[i];
                cameraKeys[i].GetBinding(1).Key = secondaryCameraKeys[i];
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
                        Game.Instance.Keyboard.Disabled.SetValue(true);
                        DisableCamera();
                        return;
                    }
                    var window = urepl.GetComponent<Window>();
                    if (window.isOpen)
                    {
                        Game.Instance.Keyboard.Disabled.SetValue(false);
                        EnableCamera();
                        window.Close();
                    }
                    else
                    {
                        Game.Instance.Keyboard.Disabled.SetValue(true);
                        DisableCamera();
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
