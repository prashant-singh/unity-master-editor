using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
//using System;
public enum OPTIONS { 
	Int = 0, 
	String = 1, 
	Float = 2
}
public class OtherMasterTools : EditorWindow {
	public OPTIONS op;
	static Texture2D scriptIcon,audioIcon;
	[MenuItem("[Master_Tools]/Tools")]
	static void Init() 
	{
		GetWindow(typeof(OtherMasterTools));
		GetWindow(typeof(OtherMasterTools)).minSize = new Vector2(600,450);
		GetWindow(typeof(OtherMasterTools)).titleContent = new GUIContent("Master Tools");
		_playerPrefsKey = "Key";
		_playerPrefsValue = "Value";
		_playerPrefsFloatValue = 0.0f;
		_playerPrefsIntValue = 0;
		isAutoSaveBeforePlay = EditorPrefs.GetBool("autoSaveOnPlay",false);
		scriptIcon = EditorGUIUtility.ObjectContent(null,typeof(Texture)).image as Texture2D;
		audioIcon = EditorGUIUtility.ObjectContent(null,typeof(AudioClip)).image as Texture2D;
	}



	public OtherMasterTools()
	{
		EditorApplication.playmodeStateChanged += SaveCurrentScene;

	}

	GUIStyle styleHelpboxInner;
	GUIStyle titleLabel,normalLabel,subtitleLabel;
	string _allTextureSizeInAsset = "0 Bytes",_allAudioSizeInAsset="0 Bytes";
	static string _playerPrefsKey="Key",_playerPrefsValue;
	static float _playerPrefsFloatValue;
	static int _playerPrefsIntValue;

	string _playerPrefsNotificationLabel;

	bool _playerPrefBoolValue;
	static bool isAutoSaveBeforePlay,isAutoSaveWithinTime;

	void OnGUI()
	{
		InitStyles();

		GUILayout.BeginVertical(styleHelpboxInner);
		GUILayout.Label("Master Tools version 0.2",normalLabel);
		GUILayout.Label(" Other Tools ",titleLabel);
		GUILayout.Space(20);
		GUILayout.Label("  Total size of Images in project folder ",subtitleLabel);
		GUILayout.BeginHorizontal(styleHelpboxInner);
			// Get All the 
			if(GUILayout.Button(" Refresh ",GUILayout.MinWidth(80),GUILayout.MaxWidth(100),GUILayout.MinHeight(30)))
			{
				totalSize = 0;
				GetCompressedFileSize("Texture");
				_allTextureSizeInAsset = GetFormattedSize(totalSize);
				GetCompressedFileSize("Texture");
				_allAudioSizeInAsset = GetFormattedSize(totalSize);
				scriptIcon = EditorGUIUtility.ObjectContent(null,typeof(Texture)).image as Texture2D;
				audioIcon = EditorGUIUtility.ObjectContent(null,typeof(AudioClip)).image as Texture2D;
			}
		GUILayout.Space(200);
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Box(scriptIcon,GUILayout.MinWidth(30),GUILayout.MinHeight(30));
					GUILayout.Label(" : "+_allTextureSizeInAsset,normalLabel);
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.Space(10);
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Box(audioIcon,GUILayout.MinWidth(30),GUILayout.MinHeight(30));
					GUILayout.Label(" : "+_allAudioSizeInAsset,normalLabel);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
		GUILayout.Space(10);
			GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		GUILayout.Label(" Edit any player prefs ",subtitleLabel);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal(styleHelpboxInner);

		_playerPrefsKey = EditorGUILayout.TextField(_playerPrefsKey,GUILayout.MinHeight(20));
	
		switch (op) {
		default:
			_playerPrefsValue = EditorGUILayout.TextField(_playerPrefsValue,GUILayout.MinHeight(20));
			break;
		case OPTIONS.Float:
			_playerPrefsFloatValue = EditorGUILayout.FloatField(_playerPrefsFloatValue,GUILayout.MinHeight(20));
			break;
		case OPTIONS.Int:
			_playerPrefsIntValue = EditorGUILayout.IntField(_playerPrefsIntValue,GUILayout.MinHeight(20));
			break;
		}
		op = (OPTIONS) EditorGUILayout.EnumPopup( op,GUILayout.MaxWidth(80));

		GUI.SetNextControlName("clearButton");
		if(GUILayout.Button("X",GUILayout.MaxWidth(20),GUILayout.Height(20)))
		{
			_playerPrefsKey = "Key";
			_playerPrefsValue = "Value";
			_playerPrefsNotificationLabel = "";
			GUI.FocusControl("clearButton");
		}

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		if(GUILayout.Button("Save",GUILayout.MinWidth(100),GUILayout.MaxWidth(150),GUILayout.MinHeight(25)))
		{
			if(!string.IsNullOrEmpty(_playerPrefsKey))
			{
				if(op == OPTIONS.String)
				{
					PlayerPrefs.SetString(_playerPrefsKey,_playerPrefsValue.ToString());
				}
				else if(op == OPTIONS.Int)
				{
					PlayerPrefs.SetInt(_playerPrefsKey,_playerPrefsIntValue);
				}
				else if(op == OPTIONS.Float)
				{
					PlayerPrefs.SetFloat(_playerPrefsKey,_playerPrefsFloatValue);
				}
				_playerPrefsNotificationLabel = "Data saved";
			}
			else
			{
				_playerPrefsNotificationLabel = "Enter the PlayerPrefs Key";
			}
		}
		if(GUILayout.Button("Get",GUILayout.MinWidth(100),GUILayout.MaxWidth(150),GUILayout.MinHeight(25)))
		{
			if(!string.IsNullOrEmpty(_playerPrefsKey))
			{
			if(op == OPTIONS.String)
			{
				_playerPrefsValue = PlayerPrefs.GetString(_playerPrefsKey,_playerPrefsValue.ToString());
			}
			else if(op == OPTIONS.Int)
			{
				_playerPrefsIntValue = 	PlayerPrefs.GetInt(_playerPrefsKey,_playerPrefsIntValue);
			}
			else if(op == OPTIONS.Float)
			{
				_playerPrefsFloatValue =	PlayerPrefs.GetFloat(_playerPrefsKey,_playerPrefsFloatValue);
			}
			_playerPrefsNotificationLabel = "Data loaded";
			}
			else
			{
				_playerPrefsNotificationLabel = "Enter the PlayerPrefs Key";	
			}
		}

		if(GUILayout.Button("Delete this key",GUILayout.MinWidth(100),GUILayout.MaxWidth(150),GUILayout.MinHeight(25)))
		{
			if(!string.IsNullOrEmpty(_playerPrefsKey))
			{
			bool option = EditorUtility.DisplayDialog( "Do you really want to Delete "+_playerPrefsKey+ "'s data???",
				"Gotta confirm it bro!",
				"I know what I'm doing.",
				"No bro, I swear I didn't do anything."
			);			
			if(option)
			{
				PlayerPrefs.DeleteAll();
				_playerPrefsNotificationLabel = "Deleted current key's data";
			}
			}
			else
			{
				_playerPrefsNotificationLabel = "Enter the PlayerPrefs Key";	
			}
		}
		if(GUILayout.Button("Delete All",GUILayout.MinWidth(100),GUILayout.MaxWidth(150),GUILayout.MinHeight(25)))
		{
			bool option = EditorUtility.DisplayDialog( "Do you really want to Delete all the Data ???? ",
				"Gotta confirm it bro!",
				"I know what I'm doing.",
				"Noooooo!"
			);			
			if(option)
			{
				PlayerPrefs.DeleteAll();
				_playerPrefsNotificationLabel = "All data deleted";
			}
			else
			{
//				Debug.LogError(" Na bolta hai sala");
			}
		}

		GUILayout.EndHorizontal();
		GUILayout.Label(_playerPrefsNotificationLabel,normalLabel);
		GUILayout.EndVertical();

		GUILayout.Space(20);
		GUILayout.Label(" No more losing your changes ",subtitleLabel);
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal(styleHelpboxInner);
		isAutoSaveBeforePlay = GUILayout.Toggle(isAutoSaveBeforePlay,"Auto save scene before playing");
		if(GUILayout.Button("Save settings",GUILayout.MaxWidth(100),GUILayout.MaxHeight(30)))
		{
			EditorPrefs.SetBool("autoSaveOnPlay",isAutoSaveBeforePlay);
		}
//		isAutoSaveWithinTime = GUILayout.Toggle(isAutoSaveWithinTime,"Auto save within some time");
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndVertical();
	}

	static void SaveCurrentScene()
	{
		if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode && isAutoSaveBeforePlay)
		{
			Debug.Log("Saving scene on play...");
			EditorApplication.SaveScene();
		}
	}


	void InitStyles()
	{
		titleLabel = new GUIStyle();
		titleLabel.fontSize = 16;
		titleLabel.fontStyle = FontStyle.Bold;
		titleLabel.normal.textColor = Color.white;
		titleLabel.alignment = TextAnchor.UpperCenter;
		titleLabel.fixedHeight = 18;

		normalLabel = new GUIStyle();
		normalLabel.fontSize = 12;
		normalLabel.normal.textColor = Color.white;
		normalLabel.fixedHeight = 34;
		normalLabel.alignment = TextAnchor.MiddleCenter;

		subtitleLabel = new GUIStyle();
		subtitleLabel.fontSize = 14;
		titleLabel.fontStyle = FontStyle.Bold;
		subtitleLabel.normal.textColor = Color.white;
		subtitleLabel.fixedHeight = 15;
		subtitleLabel.alignment = TextAnchor.MiddleLeft;

		styleHelpboxInner = new GUIStyle("HelpBox");
		styleHelpboxInner.padding = new RectOffset(6, 6, 6, 6);
	}

	static long totalSize = 0;
	//++++++++++++++++++++++++++++++++++++++++++++++++++++ GET ALL TEXTURE SIZE IN ASSETS FOLDER +++++++++++++++++++++++++++++++++++++++++++++

	private static void GetCompressedFileSize(string fileType)
	{

		Object[] AllObjects;
		if(fileType.Equals("Audio"))
		{
			AllObjects	= Resources.FindObjectsOfTypeAll(typeof(AudioClip));
		}
		else
		{
			AllObjects	= Resources.FindObjectsOfTypeAll(typeof(Texture));
		}
		
		foreach (Object currentObj in AllObjects) 
		{
			string tempPath = AssetDatabase.GetAssetPath(currentObj);
			if (!string.IsNullOrEmpty(tempPath))
			{
				string guid = AssetDatabase.AssetPathToGUID(tempPath);
				string p = Path.GetFullPath(Application.dataPath + "../../Library/metadata/" + guid.Substring(0, 2) + "/" + guid);
				if (File.Exists(p))
				{
					var file = new FileInfo(p);
					totalSize += file.Length;
				}
			}
		}
//		Debug.LogError("Total Size "+GetFormattedSize(totalSize));
	}
		
	static string GetFormattedSize(double tempSizeInBytes)
	{
		if(tempSizeInBytes>=1024 && tempSizeInBytes<1048576)
		{
			return (tempSizeInBytes/1024).ToString("00.##") +" Kb";
		}
		if(tempSizeInBytes>=1048576)
		{
			return ((tempSizeInBytes/1024)/1024).ToString("00.##") + " Mb";
		}
		else
		{
			return tempSizeInBytes.ToString()+ " Bytes";
		}
	}


	




	//++++++++++++++++++++++++++++++++++++++++++++++++++++ QUICK ACTIONS +++++++++++++++++++++++++++++++++++++++++++++
	[MenuItem("[Master_Tools]/Quick Actions/Delete Prefs %#x")]
	static void DeletePrefs()
	{
		if(EditorUtility.DisplayDialog("Delete All PlayerPrefs?", "Do you really want to delete all the PlayerPrefs?", "Yes", "No") == true)

		{

			PlayerPrefs.DeleteAll()	;
		}
	}

	[MenuItem ("[Master_Tools]/Quick Actions/Disable All #d")]
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
	[MenuItem("[Master_Tools]/Quick Actions/Delete #x")]
	static void DeleteSelectedItem()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			DestroyImmediate(go);
		}
	}

	[MenuItem("[Master_Tools]/Quick Actions/Disable or Enable _d")]
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

