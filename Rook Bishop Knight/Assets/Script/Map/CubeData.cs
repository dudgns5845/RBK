using UnityEngine;
/// <summary>
/// 8X8의 각각 판에 대한 정보를 저장
/// 선택 여부, 0~63중에 몇번째인지 index, 기믹 여부
/// </summary>
public class CubeData :MonoBehaviour
{
    public bool selected;
    public int index;
    public int Gimmick;
}
