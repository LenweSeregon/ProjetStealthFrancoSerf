using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EgnimaManager : MonoBehaviour
{
    public AInventory playerInventory;
    public EgnimaDoorManager egnimaDoorManager;
    public AudioManager audioManager;
    public EgnimaLock[] locks;
    public TextMeshProUGUI informationText;

    public float openingTime = 3.0f;

    private bool isOpeningDoor1;
    private Color savedColorDoor1;
    public GameObject door1Indicator;
    public GameObject door1;

    private bool isOpeningDoor2;
    private Color savedColorDoor2;
    public GameObject door2Indicator;
    public GameObject door2;

    private bool isOpeningDoor3;
    private Color savedColorDoor3;
    public GameObject door3Indicator;
    public GameObject door3;

    private bool isOpeningDoor4;
    private Color savedColorDoor4;
    public GameObject door4Indicator;
    public GameObject door4;

    [HideInInspector]
    public List<int> locksIndexForSuccess;
    [HideInInspector]
    public List<int> locksIndexNotMarked;

    //public List<int> indexLockSolution;

    private void Start()
    {
        savedColorDoor1 = door1Indicator.GetComponent<Image>().color;
        savedColorDoor2 = door2Indicator.GetComponent<Image>().color;
        savedColorDoor3 = door3Indicator.GetComponent<Image>().color;
        savedColorDoor4 = door4Indicator.GetComponent<Image>().color;
    }

    private void OnEnable()
    {
        isOpeningDoor1 = false;
        isOpeningDoor2 = false;
        isOpeningDoor3 = false;
        isOpeningDoor4 = false;
        locksIndexForSuccess = new List<int>();
        locksIndexNotMarked = new List<int>();
        for(int i = 0; i < locks.Length; i++)
        {
            locksIndexNotMarked.Add(i);
        }

        ReinitLocks();
        GenerateSuccessLevel();
        GenerateWrongPath();
        SetCryptedSolutionText();
    }

    private void OnDisable()
    {
        if (Door1Open() && Door2Open() && Door3Open() && Door4Open())
        {
            egnimaDoorManager.StartTimer();
        }
        else if(Door1Open() || Door2Open() || Door3Open() || Door4Open())
        {
            egnimaDoorManager.CloseDoors();
        }
    }

    public void DecodeButtonClicked()
    {
        // Test if decoder present
        if(playerInventory.GetQuantityOfItem("decoder") > 0)
        {
            playerInventory.RemoveQuantityFromInventory("decoder", 1);
            SetUncryptedSolutionText();
        }
    }

    private void Update()
    {
        // DOOR OPEN
        if(Door1Open() && !isOpeningDoor1)
        {
            isOpeningDoor1 = true;
            egnimaDoorManager.OpenDoor1();
        }
        if (Door2Open() && !isOpeningDoor2)
        {
            isOpeningDoor2 = true;
            egnimaDoorManager.OpenDoor2();
        } 
        if (Door3Open() && !isOpeningDoor3)
        {
            isOpeningDoor3 = true;
            egnimaDoorManager.OpenDoor3();
        }
        if (Door4Open() && !isOpeningDoor4)
        {
            isOpeningDoor4 = true;
            egnimaDoorManager.OpenDoor4();
        }

        // DOOR CLOSE
        if(!Door1Open() && isOpeningDoor1)
        {
            isOpeningDoor1 = false;
            egnimaDoorManager.CloseDoor1();
        }

        if (!Door2Open() && isOpeningDoor2)
        {
            isOpeningDoor2 = false;
            egnimaDoorManager.CloseDoor2();
        }

        if (!Door3Open() && isOpeningDoor3)
        {
            isOpeningDoor3 = false;
            egnimaDoorManager.CloseDoor3();
        }

        if (!Door4Open() && isOpeningDoor4)
        {
            isOpeningDoor4 = false;
            egnimaDoorManager.CloseDoor4();
        }

        // Coloring
        if(Door1Open())
        {
            door1Indicator.GetComponent<Image>().color = Color.green;
        }
        else
        {
            door1Indicator.GetComponent<Image>().color = savedColorDoor1;
        }

        if(Door2Open())
        {
            door2Indicator.GetComponent<Image>().color = Color.green;
        }
        else
        {
            door2Indicator.GetComponent<Image>().color = savedColorDoor2;
        }

        if (Door3Open())
        {
            door3Indicator.GetComponent<Image>().color = Color.green;
        }
        else
        {
            door3Indicator.GetComponent<Image>().color = savedColorDoor3;
        }

        if (Door4Open())
        {
            door4Indicator.GetComponent<Image>().color = Color.green;
        }
        else
        {
            door4Indicator.GetComponent<Image>().color = savedColorDoor4;
        }

    }

    private void SetCryptedSolutionText()
    {
        string possible = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN123456789";

        int range = Random.Range(50, 70);
        string crypted = "";
        for (int i = 0; i < range; i++)
        {
            crypted += possible[Random.Range(0, possible.Length - 1)];
        }
        informationText.text = crypted;
    }

    private void SetUncryptedSolutionText()
    {
        string solution = "START-";

        for(int i = 0; i < locks.Length; i++)
        {
            if(locksIndexForSuccess.Contains(i))
            {
                if(i == locks.Length - 1)
                {
                    solution += FromIndexToStringInformation(i);
                }
                else
                {
                    solution += FromIndexToStringInformation(i) + " /\\ ";
                }
            }
            else if(Random.Range(0.0f, 1.0f) < 0.35)
            {
                if (i == locks.Length - 1)
                {
                    solution += "NOT(" + FromIndexToStringInformation(i) + ")";
                }
                else
                {
                    solution += "NOT(" + FromIndexToStringInformation(i) + ") /\\ ";
                }
            }
        }

        solution += "-END";

        informationText.text = solution;
    }
    private string FromIndexToStringInformation(int index)
    {
        if (index == 0)
            return "11";
        else if (index == 1)
            return "21";
        else if (index == 2)
            return "31";
        else if (index == 3)
            return "41";
        else if (index == 4)
            return "12";
        else if (index == 5)
            return "22";
        else if (index == 6)
            return "32";
        else if (index == 7)
            return "42";
        else if (index == 8)
            return "13";
        else if (index == 9)
            return "23";
        else if (index == 10)
            return "33";
        else if (index == 11)
            return "43";
        else if (index == 12)
            return "14";
        else if (index == 13)
            return "24";
        else if (index == 14)
            return "34";
        else if (index == 15)
            return "44";

        return "";
    }

    private void ReinitLocks()
    {
        int i = 0;
        foreach(EgnimaLock _lock in locks)
        {
            _lock.activateUpperLock = false;
            _lock.activateLowerLock = false;
            _lock.activateLeftLock = false;
            _lock.activateRightLock = false;
            _lock.CloseLock();
            i++;
        }
    }

    private void GenerateSuccessLevel()
    {
        while(locksIndexNotMarked.Count > 0)
        {
            int random = Random.Range(0, locksIndexNotMarked.Count);
            int index = locksIndexNotMarked[random];
            GenerateActivationSuccessForLock(index);
        }
    }
    private void GenerateWrongPath()
    {
        for(int i = 0; i < locks.Length; i++)
        {
            if(!locksIndexForSuccess.Contains(i))
            {
                GenerateActivationWrongForLock(i);
            }
        }
    }

    private void GenerateActivationWrongForLock(int index)
    {
        EgnimaLock target = locks[index];
        List<ActivationGenerationNeighbour> neighboursAvailable = AvailableNeighbourForWrong(index);
        float percentage = 0.8f;
        for (int i = 0; i < neighboursAvailable.Count; i++)
        {
            float randomPercent = Random.Range(0.0f, 1.0f);
            if (randomPercent <= percentage)
            {
                if (neighboursAvailable[i].orientation == ActivationOrientation.UPPER)
                {
                    target.activateUpperLock = true;
                }
                else if (neighboursAvailable[i].orientation == ActivationOrientation.LOWER)
                {
                    target.activateLowerLock = true;
                }
                else if (neighboursAvailable[i].orientation == ActivationOrientation.LEFT)
                {
                    target.activateLeftLock = true;
                }
                else if (neighboursAvailable[i].orientation == ActivationOrientation.RIGHT)
                {
                    target.activateRightLock = true;
                }
                percentage -= 0.2f;
            }
            else
            {
                break;
            }
        }
    }
    private void GenerateActivationSuccessForLock(int index)
    {
        locksIndexForSuccess.Add(index);
        locksIndexNotMarked.Remove(index);
        EgnimaLock target = locks[index];
        List<ActivationGenerationNeighbour> neighboursAvailable = AvailableNeighbourForSuccess(index);
        float percentage = 1.0f;
        for (int i = 0; i < neighboursAvailable.Count; i++)
        {
            float randomPercent = Random.Range(0.0f, 1.0f);
            if (randomPercent <= percentage)
            {
                if (neighboursAvailable[i].orientation == ActivationOrientation.UPPER)
                {
                    target.activateUpperLock = true;
                    locksIndexNotMarked.Remove(neighboursAvailable[i].index);
                }
                else if (neighboursAvailable[i].orientation == ActivationOrientation.LOWER)
                {
                    target.activateLowerLock = true;
                    locksIndexNotMarked.Remove(neighboursAvailable[i].index);
                }
                else if (neighboursAvailable[i].orientation == ActivationOrientation.LEFT)
                {
                    target.activateLeftLock = true;
                    locksIndexNotMarked.Remove(neighboursAvailable[i].index);
                }
                else if (neighboursAvailable[i].orientation == ActivationOrientation.RIGHT)
                {
                    target.activateRightLock = true;
                    locksIndexNotMarked.Remove(neighboursAvailable[i].index);
                }

                percentage -= 0.3f;
            }
            else
            {
                break;
            }
        }
    }

    private List<ActivationGenerationNeighbour> AvailableNeighbourForWrong(int index)
    {
        EgnimaLock target = locks[index];
        List<ActivationGenerationNeighbour> availables = new List<ActivationGenerationNeighbour>();

        if (target.upperLock != null)
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.UPPER, index - 4));
        }
        if (target.lowerLock != null)
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.LOWER, index + 4));
        }
        if (target.leftLock != null)
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.LEFT, index - 1));
        }
        if (target.rightLock != null)
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.RIGHT, index + 1));
        }

        Shuffle(ref availables);
        return availables;
    }
    private List<ActivationGenerationNeighbour> AvailableNeighbourForSuccess(int index)
    {
        EgnimaLock target = locks[index];
        List<ActivationGenerationNeighbour> availables = new List<ActivationGenerationNeighbour>();

        if (target.upperLock != null && locksIndexNotMarked.Contains(index - 4))
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.UPPER, index - 4));
        }
        if (target.lowerLock != null && locksIndexNotMarked.Contains(index + 4))
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.LOWER, index + 4));
        }
        if (target.leftLock != null && locksIndexNotMarked.Contains(index - 1))
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.LEFT, index - 1));
        }
        if (target.rightLock != null && locksIndexNotMarked.Contains(index + 1))
        {
            availables.Add(new ActivationGenerationNeighbour(ActivationOrientation.RIGHT, index + 1));
        }

        Shuffle(ref availables);
        return availables;
    }

    private bool Door1Open()
    {
        for(int i = 0; i < 4; i++)
        {
            if(locks[i].isLock)
            {
                return false;
            }
        }
        return true;
    }
    private bool Door2Open()
    {
        for(int i = 4; i < 8; i++)
        {
            if (locks[i].isLock)
            {
                return false;
            }
        }
        return true;
    }
    private bool Door3Open()
    {
        for(int i = 8; i < 12; i++)
        {
            if (locks[i].isLock)
            {
                return false;
            }
        }
        return true;
    }
    private bool Door4Open()
    {
        for(int i = 12; i < 16; i++)
        {
            if (locks[i].isLock)
            {
                return false;
            }
        }
        return true;
    }

    private void Shuffle<T>(ref List<T> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            T temp = array[i];
            int randomIndex = Random.Range(i, array.Count);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    
    private enum ActivationOrientation
    {
        UPPER, LOWER, LEFT, RIGHT
    }
    private class ActivationGenerationNeighbour
    {
        public ActivationOrientation orientation;
        public int index;

        public ActivationGenerationNeighbour(ActivationOrientation _orientation, int _index)
        {
            orientation = _orientation;
            index = _index;
        }
    }
}
