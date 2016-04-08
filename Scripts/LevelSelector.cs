public class LevelSelector : EditorWindow
{
	string levelText;

	[MenuItem("[Custom Tools]/Level Selector", false, 103)]
	static void LaunchLevelSelector() 
	{
		LevelSelector window = (LevelSelector)EditorWindow.GetWindow (typeof(LevelSelector));
		window.title = "Level Selector";
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.HelpBox("Enter a level number below and press the 'Set' button, bro.", MessageType.Info, true);
		levelText = EditorGUILayout.TextField ("Level number:", levelText);

		var setButton = GUILayout.Button("Set", new GUIStyle(GUI.skin.GetStyle("Button")) { alignment = TextAnchor.MiddleCenter });


		if(setButton)
		{
			try 
			{ 
				int levelNumberVal = int.Parse(levelText);
				
				if(levelNumberVal>=0 && levelNumberVal<=35)
				{
					SanePrefs.levelSelected = levelNumberVal;
					Debug.Log("<color=blue>Level selected: </color>"+levelText);
					
				}
					
				else
				{
					Debug.Log("<color=red>Level not found!</color>");
				}
					
					levelText = "";
					GUI.FocusControl("setButton"); 
			}

			catch (System.Exception ex) 
			{
				Debug.Log("<color=red>Format exception.</color> Make sure the text field has a numeric value and is not blank.");
			}
		}
		EditorGUILayout.EndVertical();

	}
}
