using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoadingScreenUpdater : MonoBehaviour {

    public RectTransform mecanism;
    public Slider progressBar;
    public TextMeshProUGUI upperText;
    public TextMeshProUGUI lowerText;

    private string upperTextID;
    public string UpperTextID
    {
        get
        {
            return upperTextID;
        }
        set
        {
            upperTextID = value;
            upperText.GetComponent<TextUI>().textID = upperTextID;
            upperText.text = I18nManager.Fields[upperTextID];
        }
    }

    private string lowerTextID;
    public string LowerTextID
    {
        get
        {
            return lowerTextID;
        }
        set
        {
            lowerTextID = value;
            lowerText.GetComponent<TextUI>().textID = lowerTextID;
            lowerText.text = I18nManager.Fields[lowerTextID];
        }
    }

    private bool isUpdating;
    public bool IsUpdating
    {
        get
        {
            return isUpdating;
        }
        set
        {
            isUpdating = value;
        }
    }

    private float percentageComplete;
    public float PercentageComplete
    {
        get
        {
            return percentageComplete;
        }
        set
        {
            percentageComplete = value;
        }
    }

    private int currentlyRegenerated;
    private int totalToRegenerate;

    public void StartUILoadingScreenUpdater()
    {
        isUpdating = true;
        progressBar.value = 0.0f;
        percentageComplete = 0.0f;
        mecanism.transform.rotation = Quaternion.identity;
        StartCoroutine(AnimationUpdating());
    }

    private IEnumerator AnimationUpdating()
    {
        while (isUpdating)
        {
            progressBar.value = percentageComplete;
            mecanism.Rotate(new Vector3(0, 0, -7));
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


}
