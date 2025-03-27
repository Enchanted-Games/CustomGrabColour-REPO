using UnityEngine;

internal class Assets
{
	internal static AssetBundle mainAssetBundle;

	private static string[] assetNames = new string[0];

	internal static void LoadAssetBundle(string assetbundlePath)
	{
		if ((Object)mainAssetBundle == (Object)null)
		{
			mainAssetBundle = AssetBundle.LoadFromFile(assetbundlePath);
		}
		assetNames = mainAssetBundle.GetAllAssetNames();
	}

	internal static AssetBundle GetBundle()
	{
		if (!(Object)mainAssetBundle)
		{
			Debug.LogError((object)"There is no AssetBundle to load assets from.");
			return null;
		}
		return mainAssetBundle;
	}
}