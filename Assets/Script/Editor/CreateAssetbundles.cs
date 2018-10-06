using UnityEditor;
using System.IO;

public class CreateAssetbundles
{
    [MenuItem("AssetsBundle/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string _dir = "AssetBundles";

        if (!Directory.Exists(_dir))
        {
            Directory.CreateDirectory(_dir);
        }

        BuildPipeline.BuildAssetBundles(_dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}