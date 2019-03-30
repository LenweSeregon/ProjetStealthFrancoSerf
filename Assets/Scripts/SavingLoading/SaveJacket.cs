using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveJacket
{
    public string saveName;
    public DateTime realDateTime;
    public int ingameSeconds;

    public SaveJacket(SaveJacketData jacketData)
    {
        saveName = jacketData.saveName;
        realDateTime = Convert.ToDateTime(jacketData.realDateTimeAsString);
        ingameSeconds = jacketData.ingameSeconds;
    }

    public SaveJacket(string _saveName, DateTime _realDateTime, int _ingameSeconds)
    {
        saveName = _saveName;
        realDateTime = _realDateTime;
        ingameSeconds = _ingameSeconds;
    }
}
