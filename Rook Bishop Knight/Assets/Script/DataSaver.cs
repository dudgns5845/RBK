using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSaver
{
    public List<string> Stages;//스테이지 데이터 배열
    public List<int[]> Gimmick;//스테이지 기믹 데이터 배열
    public List<string> FlavorText;//스테이지 이름 목록
    public List<int> BestRes;//스테이지별 최저 이동수

    public DataSaver(ProtoManager pm)
    {
        Stages = pm.Stages;
        Gimmick = pm.Gimmicks;
        FlavorText = pm.FlavorText;
        BestRes = pm.BestRes;
    }
}
