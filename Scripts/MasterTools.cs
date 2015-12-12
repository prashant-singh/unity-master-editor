using UnityEngine;
using System.Collections;
using UnityEditor;


public class MasterTools : MonoBehaviour {
	
	[MenuItem("GOL/Delete Prefs %#x")]
	static void DeletePrefs()
	{
		if(EditorUtility.DisplayDialog("Delete All PlayerPrefs?", "Do you really want to delete all the PlayerPrefs?", "Yes", "No") == true)

		{
			
			PlayerPrefs.DeleteAll()	;
		}
	}

	[MenuItem ("GOL/Disable All #d")]
	static void DisableEverything()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.name == "Camera")
			{
				for(int count = 0;count<go.transform.childCount;count++)
				{
					go.transform.GetChild(count).gameObject.SetActive(false);
				}
			}
		}
	}
	[MenuItem("GOL/Delete #x")]
	static void DeleteSelectedItem()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			DestroyImmediate(go);
		}
	}

	[MenuItem("GOL/Disable or Enable _d")]
	static void DisableOrEnableObject()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			for(int count = 0;count<Selection.gameObjects.Length;count++)
			{
				go.SetActive(!go.activeInHierarchy);
			}
		}
	}

	
}
