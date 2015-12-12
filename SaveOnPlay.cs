using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class SaveOnPlay  {
	
		static SaveOnPlay()
		{
			EditorApplication.playmodeStateChanged += SaveCurrentScene;
		}

		static void SaveCurrentScene()
		{
			if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
			{
				Debug.Log("Saving scene on play...");
				EditorApplication.SaveScene();
			}
	}
}
