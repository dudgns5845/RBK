using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSaver
{
    public List<string> Stages;//�������� ������ �迭
    public List<string> FlavorText;//�������� �̸� ���
    public List<int> BestRes;//���������� ���� �̵���

    public DataSaver(ProtoManager pm)
    {
        Stages = pm.Stages;
        FlavorText = pm.FlavorText;
        BestRes = pm.BestRes;
    }
}
