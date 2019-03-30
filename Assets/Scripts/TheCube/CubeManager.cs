using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeManager : MonoBehaviour {

    public AudioManager audioManager;
    public TextMeshProUGUI textReasonLose;
    public GUIManager guiManager;
    private bool losed;
    private bool hasGetHistoryFragment;

	public enum LoseReason
    {
        CATCHED,
        OVER_TIME,
        SYSTEM
    }
    
    // Use this for initialization
	void Start () {
        InGameInformationHolder.hasGetScroll = false;
        losed = false;
        hasGetHistoryFragment = false;
	}

    public void Lose(LoseReason reason)
    {
        if(reason == LoseReason.CATCHED)
        {
            textReasonLose.text = I18nManager.Fields["thecube.gui.lose.reasonLose.catched"];
        }
        else if(reason == LoseReason.OVER_TIME)
        {
            textReasonLose.text = I18nManager.Fields["thecube.gui.lose.reasonLose.overTime"];
        }
        else if(reason == LoseReason.SYSTEM)
        {
            textReasonLose.text = I18nManager.Fields["thecube.gui.lose.reasonLose.tricked"];
        }

        losed = true;
        audioManager.StopAlarm();
        guiManager.SwitchToWindow("LoseMenu");
    }

    public void HistoryFragmentGathered()
    {
        hasGetHistoryFragment = true;
        InGameInformationHolder.hasGetScroll = true;
    }
}
