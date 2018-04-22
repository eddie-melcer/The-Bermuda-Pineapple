using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public Texture2D logoTex;

    [Range(0,0.9f)]
    public float yPos = 0.15f;
    [Range(0, 0.9f)]
    public float textPos = 0.15f;
    [Range(0, 0.3f)]
    public float textSize = 0.1f;

    void Start () {
		
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MasterScene");
        }
	}

    private void OnGUI()
    {
        float w = Screen.width / 3;
        float h = w;
        GUI.DrawTexture(new Rect(Screen.width / 2 - w / 2, Screen.height * yPos, w, h),logoTex);

        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.color = Color.yellow;
        GUI.skin.label.fontSize = (int)(Screen.height * textSize);

        GUI.Label(new Rect(0, Screen.height * textPos, Screen.width, h), "The Bermuda Pineapple");
        GUI.color = Color.white;
    }
}
