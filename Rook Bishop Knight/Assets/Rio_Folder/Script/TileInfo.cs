using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TileInfo : MonoBehaviour
{
    //������ Ÿ���� ������ ����
    //����Ʈ�� ��ĭ
    public itemdata index = itemdata.hole;
    Button Btn_Tile;
    public Image image;


    private void Start()
    {
        image = GetComponentInChildren<Image>();
        
        Btn_Tile = GetComponent<Button>();
        Btn_Tile.onClick.AddListener(TileClick);
    }

    //Ÿ���� ���������� ������ ��ư Ŭ�� ������ ������ ���¸� ��ȭ��Ų��.
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
