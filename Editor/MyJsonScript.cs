using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// You can modify the script as you want and do leave me a courtesy thanks.
/// Enjoy the free script.
/// if you need any help contact me on twitter @_prashantsingh_
/// </summary>
public class MyJsonScript : EditorWindow {

	[MenuItem("[Master_Tools]/Web Service Checker")]
	static void Init() {

		GetWindow(typeof(MyJsonScript));
		GetWindow(typeof(MyJsonScript)).minSize = new Vector2(600,500);
		GetWindow(typeof(MyJsonScript)).titleContent = new GUIContent("J.SON");
	}

	[MenuItem("[Master_Tools]/About")]
	static void SupportDeveloper()
	{
		Application.OpenURL("https://github.com/prashant-singh");
	}

	public MyJsonScript()
	{
		parametersNames = new List<string>();
		parametersValues = new List<string>();
	}

	float width,height;

	GUIStyle styleHelpboxInner;
	GUIStyle titleLabel,normalLabel,subtitleLabel,leftNormalLabel;
	string _data = "";
	string URL = "";
	string[] tabNames;
	int currentSettingNumber = 0;
	void InitStyles()
	{
		titleLabel = new GUIStyle();
		titleLabel.fontSize = 16;
		titleLabel.normal.textColor = Color.black;
		titleLabel.alignment = TextAnchor.UpperCenter;
		titleLabel.fixedHeight = 18;

		normalLabel = new GUIStyle();
		normalLabel.fontSize = 12;
		normalLabel.normal.textColor = Color.black;
		normalLabel.fixedHeight = 14;
		normalLabel.alignment = TextAnchor.MiddleRight;

		leftNormalLabel = new GUIStyle();
		leftNormalLabel.fontSize = 12;
		leftNormalLabel.normal.textColor = Color.black;
		leftNormalLabel.fixedHeight = 14;
		leftNormalLabel.alignment = TextAnchor.MiddleLeft;

		subtitleLabel = new GUIStyle();
		subtitleLabel.fontSize = 14;
		subtitleLabel.normal.textColor = Color.black;
		subtitleLabel.fixedHeight = 15;
		subtitleLabel.alignment = TextAnchor.MiddleLeft;

		styleHelpboxInner = new GUIStyle("HelpBox");
		styleHelpboxInner.padding = new RectOffset(6, 6, 6, 6);
	}

	void OnGUI() 
	{
		InitStyles();
		styleHelpboxInner = new GUIStyle("HelpBox");
		styleHelpboxInner.padding = new RectOffset(6, 6, 6, 6);
		// URL textfield 
		GUILayout.BeginVertical(styleHelpboxInner);
		GUILayout.Label("Master Tools version 0.2",normalLabel);
		URL = EditorGUILayout.TextField("URL", URL);
		if(URL.Equals(""))
		{
			EditorGUILayout.HelpBox("Enter URL in the text field and then choose your options", MessageType.Info);
		}
		GUILayout.EndVertical();

		// Tabs 
		GUILayout.BeginVertical(styleHelpboxInner);
		tabNames = new string[]{"JSON","Post"};
		GUILayout.BeginHorizontal();
		GUILayout.Label("Select your option",leftNormalLabel);
			currentSettingNumber = GUILayout.Toolbar(currentSettingNumber,tabNames,GUILayout.MinWidth(200),GUILayout.MaxWidth(300),GUILayout.Height(30));
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		switch (currentSettingNumber) 
		{
		case 0:
			GetJsonByURL();
			break;
		case 1:
			WebServicePosMethod();
			break;
		}
	}
	Vector2 scrollPos;

	// Getting JSON result with the url
	private void GetJsonByURL()
	{
		//Get JSON button 
		GUILayout.BeginVertical(styleHelpboxInner);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Get JSON",GUILayout.Width(100),GUILayout.Height(30)) ) 
		{
			if(URL.Equals(""))			// If URL field is empty
			{
				_data = "Enter the URL";
				return;
			}

			// Getting data
			WWW test = new WWW(URL);
			while (!test.isDone) ;
			if (!string.IsNullOrEmpty(test.error)) {
				Debug.Log(test.error);
				_data = test.error;
			} else {
				Debug.Log(test.text);
				_data = FormatDataTodisplyWell(test.text);
			}
		}

		if(GUILayout.Button("Clear", GUILayout.Width(100), GUILayout.Height(30)))
		{
			_data = "";
		}
		GUILayout.EndHorizontal();
		// Showing data on the label
		EditorGUILayout.TextArea(_data,new GUIStyle(GUI.skin.label));
		GUILayout.EndVertical();

	}

	List<string> parametersNames,parametersValues;
	int parameterCounters = 0;
	bool canShowMinusButton = false;

	// Firing post method with parameters
	private void WebServicePosMethod()
	{
		
		// Showing the parameters text fields
		GUILayout.BeginVertical(styleHelpboxInner);
		GUILayout.BeginVertical();

		for (int count = 0; count < parametersNames.Count; count++) 
		{
			GUILayout.BeginHorizontal();

			parametersNames[count] = EditorGUILayout.TextField(parametersNames[count],GUILayout.MinWidth(260));
			parametersValues[count] = EditorGUILayout.TextField(parametersValues[count],GUILayout.MinWidth(260));

			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();


		if(parameterCounters<=0)
		{
			canShowMinusButton = false;
		}
		else
		{
			canShowMinusButton = true;
		}


		// plus minus buttons
		GUILayout.BeginHorizontal(styleHelpboxInner);
		GUILayout.Label("Add/Remove Parameters ",new GUIStyle(GUI.skin.label));
		if(GUILayout.Button("+", GUILayout.Width(30), GUILayout.Height(30)))
		{
			parameterCounters++;
			parametersNames.Add("var "+(parameterCounters));
			parametersValues.Add("value "+(parameterCounters));
		}

		if(canShowMinusButton)
		{
			if(GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(30)))
			{
				parametersNames.RemoveAt(parameterCounters-1);
				parametersValues.RemoveAt(parameterCounters-1);
				parameterCounters--;
			}
		}


		if(GUILayout.Button("Clear", GUILayout.Width(100), GUILayout.Height(30)))
		{
			parameterCounters = 0;
			parametersNames.Clear();
			_data = "";
		}




		// Submit button to get the response
		if (GUILayout.Button("Submit", GUILayout.Width(100), GUILayout.Height(30)))
		{
			if(URL.Equals(""))
			{
				_data = "Enter the URL";
				return;
			}
			WWWForm form = new WWWForm();

			// This fills parameters and values in the form
			for (int count = 0; count < parametersNames.Count; count++) 
			{
				form.AddField(parametersNames[count],parametersValues[count]);
			}

			WWW www = new WWW(URL,form);

			while (!www.isDone) ;
			if (!string.IsNullOrEmpty(www.error)) 
			{
				Debug.Log(www.error);
				_data = www.error;
			}
			else
			{
				Debug.Log(www.text);
				_data = FormatDataTodisplyWell(www.text);
			}
		}
		GUILayout.EndHorizontal();


		if(!string.IsNullOrEmpty(_data))
		{
			// Scroll view for displaying data
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos,false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));
			GUI.skin.label.wordWrap = true;
			GUILayout.TextArea(_data,new GUIStyle(GUI.skin.label));
			GUILayout.EndScrollView();
		}
		GUILayout.EndVertical();
	}

	// this is a little trick for formating the data to look like well formatted JSON data 
	string FormatDataTodisplyWell(string currentData)
	{
		string tempNewData = "";
		tempNewData = currentData.Replace("{","\n{\n\t");
		tempNewData = tempNewData.Replace(",",",\n\t");
		tempNewData = tempNewData.Replace("}","\n}\n");
		return tempNewData;
	}






}
