using System.Collections.Generic;

/// <summary>
/// 맵 데이터를 가지고 있는 클래스
/// </summary>
namespace MapEditor
{
    [System.Serializable]
    public class MapData
    {
        public List<string> Stages;//스테이지 데이터 배열
        public List<int[]> Gimmick;//스테이지 기믹 데이터 배열
        public List<string> FlavorText;//스테이지 이름 목록
        public List<int> BestRes;//스테이지별 최저 이동수

        //public MapData(ProtoManager pm)
        //{
        //    Stages = pm.Stages;
        //    Gimmick = pm.Gimmicks;
        //    FlavorText = pm.FlavorText;
        //    BestRes = pm.BestRes;
        //}
    }

}
