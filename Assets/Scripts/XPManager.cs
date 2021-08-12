using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XPManager
{
    public static int OreXP;
    public static int OverallXP;

    public static void ReciveOreXP()
    {
        OreXP++;
        OverallXP++;
    }
}
