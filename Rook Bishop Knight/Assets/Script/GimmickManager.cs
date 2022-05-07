using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : MonoBehaviour
{
    public ProtoManager PM;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void SelectG(int index)
    {
        PM.nowG = index;
    }

    public void ActivateGimmick(int index)
    {
        switch (index)
        {
            case 1:
                PM.extraDir = 1;
                break;
            case 2:
                PM.extraDir = -1;
                break;
        }
    }
}
