using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace uREPL
{
    public class AssetHelper
    {
        static AssetBundle bundle;
        public static string bundlePath = "Assets/AssetBundles/urepl";
        public static T Load<T>(string name) where T : UnityEngine.Object
        {
            if (bundle == null) bundle = AssetBundle.LoadFromFile(bundlePath);
            var asset = bundle.LoadAsset<T>("Assets/uREPL/Resources/" + name);
            return asset;
        }
    }
}
