using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class XPManager : Singleton<XPManager>
{
    private int _oreXp;
    private int _overallXp;

    public int[] OreXpLevls = new int[100];
    private int _curentOreLevl;

    public void ReciveOreXp()
    {
        _oreXp++;
        _overallXp++;
    }
    public void GetCurentLevl()
    {

    }
}
