using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSaver
{
    public string[] mapdata;

    public DataSaver(ProtoManager pm)
    {
        mapdata = pm.Stages;
    }
}
