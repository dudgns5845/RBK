using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TileInfo : MonoBehaviour
{
    //각각의 타일의 정보를 저장
    //디폴트는 빈칸
    public itemdata index = itemdata.hole;
    Button Btn_Tile;
    public Image image;


    private void Start()
    {
        image = GetComponentInChildren<Image>();
        
        Btn_Tile = GetComponent<Button>();
        Btn_Tile.onClick.AddListener(TileClick);
    }

    //타일을 선택했을때 아이템 버튼 클릭 정보를 가져와 상태를 변화시킨다.
    void TileClick()
    {
        GetComponent<Image>().color = Color.red;
        //Btn_Tile.image.color = Color.red;
        
        switch (MapMaker_Rio.instance.nowState)
        {
            case itemdata.empty:
                break;
            case itemdata.hole:
                break;
            case itemdata.rook:
                break;
            case itemdata.bishop:
                break;
            case itemdata.knight:
                break;
            case itemdata.king:
                break;
            case itemdata.queen:
                break;
            case itemdata.jack:
                break;
            case itemdata.enemy:
                break;
        }
    }
}
