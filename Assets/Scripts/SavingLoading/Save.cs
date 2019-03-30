using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{
    public string saveName;
    public int ingameSeconds;
    public Player player;
    public StoryManager storyManager;
    public CameraController camera;
    public ResourcePoint[] rps;
    public Chest[] chests;
    public WorkTable[] workTables;
    public Furnace[] furnaces;

    public Save(string _saveName, int _ingameSeconds, Player _player, StoryManager _storyManager, CameraController _camera, ResourcePoint[] _rps, Chest[] _chests, WorkTable[] _workTables, Furnace[] _furnaces)
    {
        saveName = _saveName;
        ingameSeconds = _ingameSeconds;
        player = _player;
        storyManager = _storyManager;
        camera = _camera;
        rps = _rps;
        chests = _chests;
        workTables = _workTables;
        furnaces = _furnaces;
    }
}
