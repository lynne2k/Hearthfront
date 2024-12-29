using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Mobile : MonoBehaviour, ISaveable
{
    /*
     
     所有的动态对象都要继承Mobile。

     主打功能：存档和读档，用Save()和Load()
     现在用的是JSONString但是如果需要优化的话即时弃用，除了改ISavable接口还要
       逐个改每个继承Mobile对象的接口函数

     我们的GlobalManager应该会记忆全体Mobile对象以便于读存
     
     */
    public abstract string Save();            // Force derived classes to implement
    public abstract void Load(string data);   // Force derived classes to implement
    public abstract void OnTick(int tick);
    public abstract void OnPossess();
    public abstract void OnUnpossess();

    public readonly bool isPossessable = true;
    public Vector3Int gridPosition;
    public bool isPossessed = false;
    public bool isAlive = true;

    [TextArea]
    public List<string> spells;
    public List<string> acceptingSpellPrefixes;

    public string ToolTip;

    public string latestData;
    public void SaveData()
    {
        latestData = Save();
    }

    public void LoadData()
    {
        Load(latestData);
    }
}