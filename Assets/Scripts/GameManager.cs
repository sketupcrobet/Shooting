using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public bool debugMod;
	public bool UIOpen;

	public bool havePickAxe;
	public bool haveAxe;

	float deltaTime = 0.0f;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		//SceneManager.LoadScene("Title");
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		if (debugMod)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				Application.targetFrameRate = 120;
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				Application.targetFrameRate = 60;
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				Application.targetFrameRate = 30;
			}
		}
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 0.02f);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h / 30;
		style.normal.textColor = new Color(1, 1, 1, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		//string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		string text = string.Format("{0:0} fps", fps);
		GUI.Label(rect, text, style);
	}
}
