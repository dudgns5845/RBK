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
}
