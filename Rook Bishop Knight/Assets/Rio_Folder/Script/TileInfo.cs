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
    public Image SubImage;

    private void Start()
    {
        Btn_Tile = GetComponent<Button>();
        Btn_Tile.onClick.AddListener(TileClick);
    }

    //Ÿ���� ���������� ������ ��ư Ŭ�� ������ ������ ���¸� ��ȭ��Ų��.
    void TileClick()
    {
        index = MapMaker_Rio.instance.nowState;
        switch (MapMaker_Rio.instance.nowState)
        {
          
            case itemdata.empty:
                image.gameObject.SetActive(true);
                break;
            case itemdata.hole:
                SubImage.color = Color.clear;
                image.gameObject.SetActive(false);
                break;

            default:
                image.gameObject.SetActive(true);
                SubImage.sprite = MapMaker_Rio.instance.sprites[(int)index];
                SubImage.color = Color.white;
                break;

            //case itemdata.rook:
            //    break;
            //case itemdata.bishop:
            //    break;
            //case itemdata.knight:
            //    break;
            //case itemdata.king:
            //    break;
            //case itemdata.queen:
            //    break;
            //case itemdata.jack:
            //    break;
            //case itemdata.enemy:
            //    break;
        }
    }
}
