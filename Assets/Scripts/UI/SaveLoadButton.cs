using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButton : MonoBehaviour
{
    public SaveUIBuilder saveBuilder;
    public LoadUIBuilder loadBuilder;
    public SaveJacket saveAssociated;

    private Button button;
    
	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
	}
	
    void OnClick()
    {
        FindObjectsOfType<AudioManager>()[0].PlayButtonClick();
        if (saveBuilder != null)
        {
            saveBuilder.BuildInformationSave(this);
        }
        if(loadBuilder != null)
        {
            loadBuilder.BuildInformationLoad(this);
        }
    }
}
