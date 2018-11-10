using System.IO;
using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string KMConsolePath = "../KingmakerREPL/AssetBundles";
        if (!Directory.Exists(KMConsolePath))
        {
            Directory.CreateDirectory(KMConsolePath);
        }
        BuildPipeline.BuildAssetBundles(KMConsolePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        string uReplPath = "Assets/AssetBundles";
        if (!Directory.Exists(uReplPath))
        {
            Directory.CreateDirectory(uReplPath);
        }
        BuildPipeline.BuildAssetBundles(uReplPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}