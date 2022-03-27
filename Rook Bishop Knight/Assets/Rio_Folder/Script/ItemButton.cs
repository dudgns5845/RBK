using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemButton : MonoBehaviour
{
    public itemdata idx;
    Button BTN;

    private void Start()
    {
        BTN =GetComponent<Button>();
        BTN.onClick.AddListener(ClickBTN);
    }

    void ClickBTN()
    {
        MapMaker_Rio.instance.nowState = (itemdata)Enum.Parse(typeof(itemdata), gameObject.name);
        print(MapMaker_Rio.instance.nowState.ToString());
    }
}
