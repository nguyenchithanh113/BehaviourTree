using UnityEngine;
using UnityEditor;
public static class MissingScriptUltility
{
	public static void FixMissingScript(Object toDelete)
	{
		//Create a new instance of the object to delete
		ScriptableObject newInstance = ScriptableObject.CreateInstance(toDelete.GetType());

		//Copy the original content to the new instance
		EditorUtility.CopySerialized(toDelete, newInstance);
		newInstance.name = toDelete.name;

		string toDeletePath = AssetDatabase.GetAssetPath(toDelete);
		string clonePath = toDeletePath.Replace(".asset", "CLONE.asset");

		//Create the new asset on the project files
		AssetDatabase.CreateAsset(newInstance, clonePath);
		AssetDatabase.ImportAsset(clonePath);

		//Unhide sub-assets
		var subAssets = AssetDatabase.LoadAllAssetsAtPath(toDeletePath);
		HideFlags[] flags = new HideFlags[subAssets.Length];
		for (int i = 0; i < subAssets.Length; i++)
		{
			//Ignore the "corrupt" one
			if (subAssets[i] == null)
				continue;

			//Store the previous hide flag
			flags[i] = subAssets[i].hideFlags;
			subAssets[i].hideFlags = HideFlags.None;
			EditorUtility.SetDirty(subAssets[i]);
		}

		EditorUtility.SetDirty(toDelete);
		AssetDatabase.SaveAssets();

		//Reparent the subAssets to the new instance
		foreach (var subAsset in AssetDatabase.LoadAllAssetRepresentationsAtPath(toDeletePath))
		{
			//Ignore the "corrupt" one
			if (subAsset == null)
				continue;

			//We need to remove the parent before setting a new one
			AssetDatabase.RemoveObjectFromAsset(subAsset);
			AssetDatabase.AddObjectToAsset(subAsset, newInstance);
		}

		//Import both assets back to unity
		AssetDatabase.ImportAsset(toDeletePath);
		AssetDatabase.ImportAsset(clonePath);

		//Reset sub-asset flags
		for (int i = 0; i < subAssets.Length; i++)
		{
			//Ignore the "corrupt" one
			if (subAssets[i] == null)
				continue;

			subAssets[i].hideFlags = flags[i];
			EditorUtility.SetDirty(subAssets[i]);
		}

		EditorUtility.SetDirty(newInstance);
		AssetDatabase.SaveAssets();

		//Here's the magic. First, we need the system path of the assets
		string globalToDeletePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), toDeletePath);
		string globalClonePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), clonePath);

		//We need to delete the original file (the one with the missing script asset)
		//Rename the clone to the original file and finally
		//Delete the meta file from the clone since it no longer exists

		System.IO.File.Delete(globalToDeletePath);
		System.IO.File.Delete(globalClonePath + ".meta");
		System.IO.File.Move(globalClonePath, globalToDeletePath);

		AssetDatabase.Refresh();
	}
}