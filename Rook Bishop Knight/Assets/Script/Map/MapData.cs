using System.Collections.Generic;

/// <summary>
/// �� �����͸� ������ �ִ� Ŭ����
/// </summary>
namespace MapEditor
{
    [System.Serializable]
    public class MapData
    {
        public List<string> Stages;//�������� ������ �迭
        public List<int[]> Gimmick;//�������� ��� ������ �迭
        public List<string> FlavorText;//�������� �̸� ���
        public List<int> BestRes;//���������� ���� �̵���

        //public MapData(ProtoManager pm)
        //{
        //    Stages = pm.Stages;
        //    Gimmick = pm.Gimmicks;
        //    FlavorText = pm.FlavorText;
        //    BestRes = pm.BestRes;
        //}
    }

}
