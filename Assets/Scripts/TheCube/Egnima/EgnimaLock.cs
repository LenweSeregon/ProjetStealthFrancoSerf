using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EgnimaLock : MonoBehaviour
{
    [HideInInspector]
    public bool hasBeenProceed;

    public EgnimaLock upperLock;
    public EgnimaLock lowerLock;
    public EgnimaLock rightLock;
    public EgnimaLock leftLock;

    [HideInInspector]
    public bool activateUpperLock;
    [HideInInspector]
    public bool activateLowerLock;
    [HideInInspector]
    public bool activateLeftLock;
    [HideInInspector]
    public bool activateRightLock;

    public Image image;
    public Sprite openLock;
    public Sprite closeLock;

    [HideInInspector]
    public bool isLock;

    private void Awake()
    {
        image = GetComponentsInChildren<Image>()[1];
        isLock = true;
    }

    public void CloseLock()
    {
        isLock = true;
        if(image != null)
        {
            image.sprite = closeLock;
        }
    }

    public void ChangeLockState()
    {
        isLock = !isLock;
        if(isLock)
        {
            image.sprite = closeLock;
        }
        else
        {
            image.sprite = openLock;
        }
    }

    public void LockClicked()
    {
        ChangeLockState();

        if(upperLock != null && activateUpperLock)
            upperLock.ChangeLockState();
        if(lowerLock != null && activateLowerLock)
            lowerLock.ChangeLockState();
        if(rightLock != null && activateRightLock)
            rightLock.ChangeLockState();
        if(leftLock != null && activateLeftLock)
            leftLock.ChangeLockState();
        
    }
}
