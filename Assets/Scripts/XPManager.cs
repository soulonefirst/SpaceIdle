using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : Singleton<XPManager>
{
    private int OreXP;
    private int OverallXP;

    public int[] OreXPLevls = new int[100];
    private int CurentOreLevl;

    public void ReciveOreXP()
    {
        OreXP++;
        OverallXP++;
    }
    public void GetCurentLevl()
    {

    }
}
