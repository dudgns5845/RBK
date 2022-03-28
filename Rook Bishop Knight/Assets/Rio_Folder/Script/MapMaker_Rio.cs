using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMaker_Rio : MonoBehaviour
{
    public static MapMaker_Rio instance;

    public List<Button> ItemButtons;
    public List<Sprite> sprites;
    public itemdata nowState;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ResetBTN(itemdata btn)
    {
        int idx = (int)btn;
        for(int i = 0; i < ItemButtons.Count; i++)
        {
            if (i == idx) ItemButtons[idx].image.color = Color.red;
            else
            {
                ItemButtons[i].image.color = Color.white;
            }
        }
    }
}
