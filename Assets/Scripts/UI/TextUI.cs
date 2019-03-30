using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUI : MonoBehaviour {

    public string textID;

    private TextMeshProUGUI text;

	// Use this for initialization
	void Awake () {
        GetComponent<TextMeshProUGUI>().text = I18nManager.Fields[textID];
    }

    public void OnLanguageUpdate()
    {
        GetComponent<TextMeshProUGUI>().text = I18nManager.Fields[textID];
    }

	
}
