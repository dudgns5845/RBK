using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

enum itemdata
{
    empty = 0,
    rook = 1,
    bishop = 2,
    knight = 3,
    king = 4,
    queen = 5,
    jack = 6,
    ace = 7,
    destintaion = 8,
    hole = 9
}

public class MapManager : MonoBehaviour
{
    public string dummydata;
    public string[] Stages;
    public string[] FlavorText;
    public int[] BestRes;
    private int nowStage;
    public float panelSpeed;
    public Transform TitleAwayPos;
    public Transform TitleShowPos;
    public Transform ResultAwayPos;
    public Transform ResultShowPos;
    public AudioSource ClearSor;
    private bool isHole;
    public AudioSource BlockRumbleSor;
    public Renderer MusicButton;
    public GameObject[] Cubes;
    public Transform origin;
    private GameObject[] Mapobject;
    public GameObject[] items;
    private int playerindex;
    public bool readytoMove;
    public float movespeed;
    public GameObject destination;
    public bool moving;
    public int playerstate;
    private int cycleDir;
    public GameObject dirArrow;
    public GameObject panel;
    public GameObject resultPanel;
    public Text msgbox;
    public Text resultbox;
    public Text resultComment;
    public Text bestResBox;
    public int OnStage;
    public bool firstmove;
    public GameObject roulette;
    public GameObject[] rouletteParts;
    private int rouletteangle;
    public float rotateSpeed;
    public Text Moves;
    public GameObject StartPanel;
    private bool fading;
    private bool aceSelecting;
    public InputField stageSelecter;

    [Header("items")]
    private bool itemActive;
    public GameObject[] Kings;
    private int kingInd;
    public GameObject[] Queens;
    private int queenInd;
    public GameObject[] Jacks;
    private int jackInd;
    public GameObject[] Aces;
    private int aceInd;
    public AudioSource itemSor;

    [Header ("Material")]
    public Material white;
    public Material black;
    public Material selectedWhite;
    public Material selectedBlack;
    public Material Gold;
    public Material Gray;
    public Material Red;
    public Material Blue;

    public Color[] bgColors;

    [Header("Sprite")]
    public Color[] StarColor;
    public GameObject[] StarGraphics;

    [Header("ParticleSystem")]
    public GameObject particleManager;
    public ParticleSystem DebrisSystem;
    public AudioSource DebrisSor;

    // Start is called before the first frame update
    void Start()
    {
        isHole = false;
        aceSelecting = false;
        fading = false;
        rouletteangle = 0;
        firstmove = true;
        Mapobject = new GameObject[64];
        readytoMove = false;
        playerstate = 1;
        cycleDir = 1;
        OnStage = 0;
        /*for(int i = 0; i < Cubes.Length; i++)
        {
            if ((Mathf.FloorToInt(i/8) + i) % 2 == 0)
            {
                Cubes[i].GetComponent<Renderer>().material = white;
            }
            else
            {
                Cubes[i].GetComponent<Renderer>().material = black;
            }
        }*/
        for(int i = 0; i < 64; i++)
        {
            Cubes[i].GetComponent<cubeData>().index = Mathf.FloorToInt(i / 8) + 8 * (7 - (i % 8));
        }
        itemActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        RetryClick();
        if (OnStage == 1)
        {
            if (!moving)
            {
                if (!aceSelecting)
                {
                    ClickButton();
                }
                else
                {
                    SelectAce();
                }
            }
            else
            {
                MovePiece();
            }
            RemoveText();
        }
        else if (OnStage == 0)
        {
            ShowText();
        }
        else if (OnStage == 2)
        {
            ShowResult();
        }
        if(OnStage != 2)
        {
            RemoveResult();
        }
        RotateRoulette();
        MusicClick();
        if (StartPanel != null)
        {
            FadeStartPanel();
        }
    }

    void SetGrid(itemdata item, int alphabet, int num)//ÁÂÇ¥·Î ¾ÆÀÌÅÛ °¡Á®¿À±â
    {
        if(Mapobject[alphabet * 8 + num] != null)//ÁÂÇ¥¿¡ ¾ÆÀÌÅÛ ÀÌ¹Ì ÀÖÀ½
        {
            Destroy(Mapobject[alphabet * 8 + num]);//¾ÆÀÌÅÛ ÆÄ±«
        }
        if((int)item == 9)//±¸¸ÛÀÎ °æ¿ì
        {
            Cubes[(7 - alphabet) + num * 8].SetActive(false);
            Mapobject[alphabet * 8 + num] = GameObject.Instantiate(items[(int)item], origin.position + Vector3.back * alphabet + Vector3.right * num, Quaternion.identity);
            ParticleSystemRenderer ps = Mapobject[alphabet * 8 + num].GetComponent<ParticleSystemRenderer>();
            if ((alphabet + num) % 2 == 0)
            {
                ps.material = white;
            }
            else
            {
                ps.material = black;
            }
            Mapobject[alphabet * 8 + num].GetComponent<ParticleSystem>().Play();
            if (!isHole)
            {
                isHole = true;
            }
        }
        if ((int)item != 0)//ºóÄ­ÀÌ ¾Æ´Ï¸é ¾ÆÀÌÅÛ ¼ÒÈ¯
        {
            Mapobject[alphabet * 8 + num] = GameObject.Instantiate(items[(int)item], origin.position + Vector3.back * alphabet + Vector3.right * num, Quaternion.identity);
            if ((int)item > 0 && (int)item < 4)
            {
                playerindex = alphabet * 8 + num;
                playerstate = (int)item;
                switch (playerstate)
                {
                    case 1:
                        rouletteParts[0].GetComponent<Renderer>().material = white;
                        rouletteParts[1].GetComponent<Renderer>().material = black;
                        rouletteParts[2].GetComponent<Renderer>().material = black;
                        break;
                    case 2:
                        rouletteParts[0].GetComponent<Renderer>().material = black;
                        rouletteParts[1].GetComponent<Renderer>().material = white;
                        rouletteParts[2].GetComponent<Renderer>().material = black;
                        break;
                    case 3:
                        rouletteParts[0].GetComponent<Renderer>().material = black;
                        rouletteParts[1].GetComponent<Renderer>().material = black;
                        rouletteParts[2].GetComponent<Renderer>().material = white;
                        break;
                }
            }
        }
    }

    void SelectCube(int index)
    {
        int alphabet;
        int num;
        alphabet = 7 - Mathf.FloorToInt(index / 8);
        num = index % 8;
        if((alphabet + num) % 2 == 0)
        {
            Cubes[alphabet + num * 8].GetComponent<Renderer>().material = selectedWhite;
            Cubes[alphabet + num * 8].GetComponent<cubeData>().selected = true;
        }
        else
        {
            Cubes[alphabet + num * 8].GetComponent<Renderer>().material = selectedBlack;
            Cubes[alphabet + num * 8].GetComponent<cubeData>().selected = true;
        }
    }

    void DeSelectAll()
    {
        for(int i = 0; i < 64; i++)
        {
            if (Cubes[i].GetComponent<cubeData>().selected)
            {
                int alphabet;
                int num;
                alphabet = 7 - Mathf.FloorToInt(i / 8);
                num = i % 8;
                if ((alphabet + num) % 2 == 0)
                {
                    Cubes[i].GetComponent<Renderer>().material = black;
                }
                else
                {
                    Cubes[i].GetComponent<Renderer>().material = white;
                }
                Cubes[i].GetComponent<cubeData>().selected = false;
            }
        }
    }

    void SetStage(string stagedata)
    {
        for(int i = 0; i < 64; i++)
        {
            if(Cubes[i].activeInHierarchy == false)
            {
                Cubes[i].SetActive(true);
            }
            SetGrid(0, Mathf.FloorToInt(i / 8), i % 8);
        }
        DeSelectAll();
        readytoMove = false;
        isHole = false;
        StringBuilder sb = new StringBuilder(stagedata);
        sb.Replace("\t", "");
        sb.Replace("\n", "");
        sb.Replace(" ", "");
        for (int i = 0; i < 64; i++)
        {
            SetGrid((itemdata) (sb[i] - '0'), Mathf.FloorToInt(i / 8), i % 8);
        }
        switch (playerstate)
        {
            case 1:
                rouletteangle = 0;
                break;
            case 2:
                rouletteangle = -120;
                break;
            case 3:
                rouletteangle = -240;
                break;
        }
        cycleDir = 1;
        firstmove = true;
        for(int i = 0; i < Kings.Length; i++)
        {
            Kings[i].SetActive(false);
            Queens[i].SetActive(false);
            Jacks[i].SetActive(false);
            Aces[i].SetActive(false);
        }
        kingInd = 0;
        queenInd = 0;
        jackInd = 0;
        aceInd = 0;
        dirArrow.transform.localScale = new Vector3(cycleDir * -6.5f, 6.5f, 6.5f);
        dirArrow.GetComponent<Renderer>().material = Red;
        itemActive = false;
        Moves.text = "0";
        aceSelecting = false;
        rouletteParts[3].GetComponent<Renderer>().material = Gray;
        rouletteParts[4].GetComponent<Renderer>().material = Gray;
        rouletteParts[5].GetComponent<Renderer>().material = Gray;
        Camera.main.backgroundColor = bgColors[nowStage % bgColors.Length];
        if (isHole)
        {
            BlockRumbleSor.Play();
        }
    }


    void Move()
    {
        if (readytoMove)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.tag == "cube")
                {
                    if (hit.transform.gameObject.GetComponent<cubeData>().selected)
                    {
                        Moves.text = (int.Parse(Moves.text) + 1).ToString();
                        DeSelectAll();
                        moving = true;
                        readytoMove = false;
                        destination = hit.transform.gameObject;
                    }
                }
            }
        }
    }

    void DeActItems()
    {
        itemActive = false;
        for (int i = 0; i < Kings.Length; i++)
        {
            if (Kings[i].activeSelf)
            {
                Renderer[] temp = Kings[i].GetComponentsInChildren<Renderer>();
                temp[0].material = Gray;
                temp[1].material = Gray;
            }
            if (Queens[i].activeSelf)
            {
                Renderer[] temp = Queens[i].GetComponentsInChildren<Renderer>();
                temp[0].material = Gray;
                temp[1].material = Gray;
            }
            if (Jacks[i].activeSelf)
            {
                Renderer[] temp = Jacks[i].GetComponentsInChildren<Renderer>();
                temp[0].material = Gray;
                temp[1].material = Gray;
            }
            if (Aces[i].activeSelf)
            {
                Renderer[] temp = Aces[i].GetComponentsInChildren<Renderer>();
                temp[0].material = Gray;
                temp[1].material = Gray;
            }
        }
    }
    void SelectMove()
    {
        DeActItems();
        if(Moves.text != "0")
        {
            int alphabet;
            int no;
            alphabet = 7 - Mathf.FloorToInt(playerindex / 8);
            no = playerindex % 8;
            particleManager.transform.position = Cubes[alphabet + no * 8].transform.position;
            DebrisSystem.Play();
            DebrisSystem.Emit(50);
            DebrisSor.Play();
        }
        switch (Mapobject[playerindex].GetComponent<itemData>().data)
        {
            case 1://·è
                bool doneR = false;
                bool doneL = false;
                bool doneU = false;
                bool doneD = false;
                for (int i = 1; i < 8; i++)
                {
                    //¿À¸¥ÂÊ ½ºÄµ
                    if (playerindex + i < Mapobject.Length && !doneR)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if (Mathf.FloorToInt((playerindex + i) / 8) == Mathf.FloorToInt(playerindex / 8))//°°Àº ¾ËÆÄºª ÁÙ È®ÀÎ
                        {
                            if (Mapobject[playerindex + i] == null)//ºóÄ­ÀÎ°¡?
                            {
                                SelectCube(playerindex + i);//¼±ÅÃÇÏ±â
                            }
                            else if (Mapobject[playerindex + i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                            {
                                doneR = true;
                            }
                            else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                            {
                                SelectCube(playerindex + i);//¼±ÅÃÇÏ±â
                                doneR = true;
                            }
                        }
                    }
                    //¿ÞÂÊ ½ºÄµ
                    if (playerindex - i > -1 && !doneL)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if (Mathf.FloorToInt((playerindex - i) / 8) == Mathf.FloorToInt(playerindex / 8))//°°Àº ¾ËÆÄºª ÁÙ È®ÀÎ
                        {
                            if (Mapobject[playerindex - i] == null)//ºóÄ­ÀÎ°¡?
                            {
                                SelectCube(playerindex - i);//¼±ÅÃÇÏ±â
                            }
                            else if (Mapobject[playerindex - i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                            {
                                doneL = true;
                            }
                            else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                            {
                                SelectCube(playerindex - i);//¼±ÅÃÇÏ±â
                                doneL = true;
                            }
                        }
                    }
                    //¾Æ·¡ ½ºÄµ
                    if (playerindex + 8 * i < Mapobject.Length && !doneD)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if (Mapobject[playerindex + 8 * i] == null)//ºóÄ­ÀÎ°¡?
                        {
                            SelectCube(playerindex + 8 * i);//¼±ÅÃÇÏ±â
                        }
                        else if (Mapobject[playerindex + 8 * i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                        {
                            doneD = true;
                        }
                        else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                        {
                            SelectCube(playerindex + 8 * i);//¼±ÅÃÇÏ±â
                            doneD = true;
                        }
                    }
                    //À§ ½ºÄµ
                    if (playerindex - 8 * i > -1 && !doneU)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if (Mapobject[playerindex - 8 * i] == null)//ºóÄ­ÀÎ°¡?
                        {
                            SelectCube(playerindex - 8 * i);//¼±ÅÃÇÏ±â
                        }
                        else if (Mapobject[playerindex - 8 * i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                        {
                            doneU = true;
                        }
                        else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                        {
                            SelectCube(playerindex - 8 * i);//¼±ÅÃÇÏ±â
                            doneU = true;
                        }
                    }
                }
                break;
            case 2://ºñ¼ó
                bool doneRU = false;
                bool doneLU = false;
                bool doneRD = false;
                bool doneLD = false;
                for (int i = 1; i < 8; i++)
                {
                    //¿À¸¥ÂÊ À§ ½ºÄµ
                    if (playerindex - 7 * i > -1 && !doneRU)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex - 7 * i) / 8) + (playerindex - 7 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex - 7 * i] == null)//ºóÄ­ÀÎ°¡?
                            {
                                SelectCube(playerindex - 7 * i);//¼±ÅÃÇÏ±â
                            }
                            else if (Mapobject[playerindex - 7 * i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                            {
                                doneRU = true;
                            }
                            else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                            {
                                SelectCube(playerindex - 7 * i);//¼±ÅÃÇÏ±â
                                doneRU = true;
                            }
                        }
                    }
                    //¿ÞÂÊ À§ ½ºÄµ
                    if (playerindex - 9 * i > -1 && !doneLU)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if ((Mathf.FloorToInt(playerindex/8)+playerindex)%2 == (Mathf.FloorToInt((playerindex - 9 * i) / 8) + (playerindex - 9 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex - 9 * i] == null)//ºóÄ­ÀÎ°¡?
                            {
                                SelectCube(playerindex - 9 * i);//¼±ÅÃÇÏ±â
                            }
                            else if (Mapobject[playerindex - 9 * i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                            {
                                doneLU = true;
                            }
                            else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                            {
                                SelectCube(playerindex - 9 * i);//¼±ÅÃÇÏ±â
                                doneLU = true;
                            }
                        }
                    }
                    //¿À¸¥ÂÊ ¾Æ·¡ ½ºÄµ
                    if (playerindex + 9 * i < Mapobject.Length && !doneRD)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex + 9 * i) / 8) + (playerindex + 9 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex + 9 * i] == null)//ºóÄ­ÀÎ°¡?
                            {
                                SelectCube(playerindex + 9 * i);//¼±ÅÃÇÏ±â
                            }
                            else if (Mapobject[playerindex + 9 * i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                            {
                                doneRD = true;
                            }
                            else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                            {
                                SelectCube(playerindex + 9 * i);//¼±ÅÃÇÏ±â
                                doneRD = true;
                            }
                        }
                    }
                    //¿ÞÂÊ ¾Æ·¡ ½ºÄµ
                    if (playerindex + 7 * i < Mapobject.Length && !doneLD)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex + 7 * i) / 8) + (playerindex + 7 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex + 7 * i] == null)//ºóÄ­ÀÎ°¡?
                            {
                                SelectCube(playerindex + 7 * i);//¼±ÅÃÇÏ±â
                            }
                            else if (Mapobject[playerindex + 7 * i].GetComponent<itemData>().data == 9)//±¸¸ÛÄ­ÀÓ
                            {
                                doneLD = true;
                            }
                            else//±¸¸ÛÄ­Àº ¾Æ´Ô, ¾ÆÀÌÅÛÄ­ÀÓ
                            {
                                SelectCube(playerindex + 7 * i);//¼±ÅÃÇÏ±â
                                doneLD = true;
                            }
                        }
                    }
                }
                break;
            case 3://³ªÀÌÆ®
                for(int alpha = -2; alpha < 3; alpha++)
                {
                    for(int num= -2; num < 3; num++)
                    {
                        if(playerindex + alpha * 8 + num > -1 && playerindex + alpha * 8 + num < 64)//ÃÑ ±æÀÌ¸¦ ¹þ¾î³ªÁö ¾Ê´Â ¹üÀ§
                        {
                            if(Mathf.Abs(alpha) + Mathf.Abs(num) == 3)
                            {
                                if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 != (Mathf.FloorToInt((playerindex + alpha * 8 + num) / 8) + (playerindex + alpha * 8 + num) % 8) % 2)
                                {
                                    SelectCube(playerindex + alpha * 8 + num);
                                }
                            }
                        }
                    }
                }
                break;
        }
        readytoMove = true;
    }
    void MovePiece()
    {
        if(Mapobject[playerindex].transform.position == destination.transform.position)
        {
            int temp = playerindex;
            playerindex = destination.GetComponent<cubeData>().index;
            if (Mapobject[playerindex] != null)
            {
                int itemtemp = Mapobject[playerindex].GetComponent<itemData>().data;
                if (itemtemp > 3 && itemtemp < 9)
                {
                    itemSave((itemdata)itemtemp);
                }
            }
            SetGrid((itemdata) playerstate, Mathf.FloorToInt(playerindex / 8), playerindex % 8);
            Destroy(Mapobject[temp]);
            itemActive = true;
            for (int i = 0; i < Kings.Length; i++)
            {
                if (Kings[i].activeSelf)
                {
                    Renderer[] Rends = Kings[i].GetComponentsInChildren<Renderer>();
                    Rends[0].material = Gold;
                    Rends[1].material = white;
                }
                if (Queens[i].activeSelf)
                {
                    Renderer[] Rends = Queens[i].GetComponentsInChildren<Renderer>();
                    Rends[0].material = Gold;
                    Rends[1].material = white;
                }
                if (Jacks[i].activeSelf)
                {
                    Renderer[] Rends = Jacks[i].GetComponentsInChildren<Renderer>();
                    Rends[0].material = Gold;
                    Rends[1].material = white;
                }
                if (Aces[i].activeSelf)
                {
                    Renderer[] Rends = Aces[i].GetComponentsInChildren<Renderer>();
                    Rends[0].material = Gold;
                    Rends[1].material = white;
                }
            }
            Mapobject[temp] = null;
            moving = false;
        }
        else
        {
            Mapobject[playerindex].transform.position = Vector3.MoveTowards(Mapobject[playerindex].transform.position, destination.transform.position, Time.deltaTime * movespeed);
        }
    }

    void goCycle()
    {
        playerstate += cycleDir;
        if (playerstate == 4)
        {
            playerstate = 1;
        }
        else if (playerstate == 0)
        {
            playerstate = 3;
        }
        SetGrid((itemdata)playerstate, Mathf.FloorToInt(playerindex / 8), playerindex % 8);
        rouletteangle -= 120 * cycleDir;
        switch (rouletteangle)
        {
            case 0:
                rouletteParts[0].GetComponent<Renderer>().material = white;
                rouletteParts[1].GetComponent<Renderer>().material = black;
                rouletteParts[2].GetComponent<Renderer>().material = black;
                break;
            case -120:
                rouletteParts[0].GetComponent<Renderer>().material = black;
                rouletteParts[1].GetComponent<Renderer>().material = white;
                rouletteParts[2].GetComponent<Renderer>().material = black;
                break;
            case -240:
                rouletteParts[0].GetComponent<Renderer>().material = black;
                rouletteParts[1].GetComponent<Renderer>().material = black;
                rouletteParts[2].GetComponent<Renderer>().material = white;
                break;
        }
    }

    void RotateRoulette()
    {
        roulette.transform.rotation = Quaternion.Lerp(roulette.transform.rotation, Quaternion.Euler(0, rouletteangle, 0), Time.deltaTime * rotateSpeed);
    }

    void itemSave(itemdata dat)
    {
        switch (dat)
        {
            case itemdata.king:
                Kings[kingInd].SetActive(true);
                kingInd++;
                break;
            case itemdata.queen:
                Queens[queenInd].SetActive(true);
                queenInd++;
                break;
            case itemdata.jack:
                Jacks[jackInd].SetActive(true);
                jackInd++;
                break;
            case itemdata.ace:
                Aces[aceInd].SetActive(true);
                aceInd++;
                break;
            case itemdata.destintaion:
                OnStage = 2;
                nowStage++;
                break;
        }
    }

    void itemWork(itemdata dat)
    {
        itemSor.Play();
        switch (dat)
        {
            case itemdata.king:
                kingInd--;
                Kings[kingInd].SetActive(false);
                break;
            case itemdata.queen:
                cycleDir *= -1;
                dirArrow.transform.localScale = new Vector3(cycleDir * -6.5f, 6.5f, 6.5f);
                if (cycleDir == 1)
                {
                    dirArrow.GetComponent<Renderer>().material = Red;
                }
                else
                {
                    dirArrow.GetComponent<Renderer>().material = Blue;
                }
                queenInd--;
                Queens[queenInd].SetActive(false);
                goCycle();
                break;
            case itemdata.jack:
                int alphabet;
                int no;
                alphabet = 7 - Mathf.FloorToInt(playerindex / 8);
                no = playerindex % 8;
                particleManager.transform.position = Cubes[alphabet + no * 8].transform.position;
                DebrisSystem.Play();
                DebrisSystem.Emit(50);
                DebrisSor.Play();
                jackInd--;
                Jacks[jackInd].SetActive(false);
                goCycle();
                Invoke("goCycle", 0.2f);
                break;
            case itemdata.ace:
                aceInd--;
                Aces[aceInd].SetActive(false);
                aceSelecting = true;
                rouletteParts[0].GetComponent<Renderer>().material = white;
                rouletteParts[1].GetComponent<Renderer>().material = white;
                rouletteParts[2].GetComponent<Renderer>().material = white;


                rouletteParts[3].GetComponent<Renderer>().material = Gold;
                rouletteParts[4].GetComponent<Renderer>().material = Gold;
                rouletteParts[5].GetComponent<Renderer>().material = Gold;
                SelectAce();
                break;
        }
    }

    void ShowText()
    {
        if(msgbox.text != FlavorText[nowStage])
        {
            msgbox.text = FlavorText[nowStage];
        }
        panel.transform.position = Vector3.Lerp(panel.gameObject.transform.position, TitleShowPos.position, Time.deltaTime * panelSpeed);
        panel.transform.localScale = Vector3.Lerp(panel.gameObject.transform.localScale, TitleShowPos.localScale, Time.deltaTime * panelSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.name == "panel")
                {
                    OnStage = 1;
                    SetStage(Stages[nowStage]);
                }
            }
        }
    }
    public void RemoveText()
    {
        panel.transform.position = Vector3.Lerp(panel.gameObject.transform.position, TitleAwayPos.position, Time.deltaTime * panelSpeed);
        panel.transform.localScale = Vector3.Lerp(panel.gameObject.transform.localScale, TitleAwayPos.localScale, Time.deltaTime * panelSpeed);
    }

    void ShowResult()
    {
        if (resultbox.text != Moves.text)
        {
            resultbox.text = Moves.text;
            bestResBox.text = BestRes[nowStage - 1].ToString();
            ClearSor.Play();
            if (int.Parse(resultbox.text) < BestRes[nowStage - 1])
            {
                resultComment.text = "You've just beat the record of developer!";
                StarGraphics[0].SetActive(true);
                StarGraphics[0].GetComponent<Renderer>().material.color = StarColor[3];
                StarGraphics[1].SetActive(true);
                StarGraphics[1].GetComponent<Renderer>().material.color = StarColor[3];
                StarGraphics[2].SetActive(true);
                StarGraphics[2].GetComponent<Renderer>().material.color = StarColor[3];
            }
            else if (int.Parse(resultbox.text) == BestRes[nowStage - 1])
            {
                resultComment.text = "Excellent move!";
                StarGraphics[0].SetActive(true);
                StarGraphics[0].GetComponent<Renderer>().material.color = StarColor[2];
                StarGraphics[1].SetActive(true);
                StarGraphics[1].GetComponent<Renderer>().material.color = StarColor[2];
                StarGraphics[2].SetActive(true);
                StarGraphics[2].GetComponent<Renderer>().material.color = StarColor[2];
            }
            else if (int.Parse(resultbox.text) - BestRes[nowStage - 1] < 4)
            {
                resultComment.text = "Great move!";
                StarGraphics[0].SetActive(true);
                StarGraphics[0].GetComponent<Renderer>().material.color = StarColor[1];
                StarGraphics[1].SetActive(true);
                StarGraphics[1].GetComponent<Renderer>().material.color = StarColor[1];
                StarGraphics[2].SetActive(false);
            }
            else if (int.Parse(resultbox.text) - BestRes[nowStage - 1] < 6)
            {
                resultComment.text = "Nice move!";
                StarGraphics[0].SetActive(true);
                StarGraphics[0].GetComponent<Renderer>().material.color = StarColor[0];
                StarGraphics[1].SetActive(false);
                StarGraphics[2].SetActive(false);
            }
            else
            {
                resultComment.text = "Having a hard time?";
                StarGraphics[0].SetActive(false);
                StarGraphics[1].SetActive(false);
                StarGraphics[2].SetActive(false);
            }
        }
        resultPanel.transform.position = Vector3.Lerp(resultPanel.gameObject.transform.position, ResultShowPos.position, Time.deltaTime * panelSpeed);
        resultPanel.transform.localScale = Vector3.Lerp(resultPanel.gameObject.transform.localScale, ResultShowPos.localScale, Time.deltaTime * panelSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.name == "resultPanel")
                {
                    OnStage = 0;
                }
            }
        }
    }

    void RemoveResult()
    {
        resultPanel.transform.position = Vector3.Lerp(resultPanel.gameObject.transform.position, ResultAwayPos.position, Time.deltaTime * panelSpeed);
        resultPanel.transform.localScale = Vector3.Lerp(resultPanel.gameObject.transform.localScale, ResultAwayPos.localScale, Time.deltaTime * panelSpeed);
    }

    void ClickButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (itemActive)
                {
                    switch (hit.transform.gameObject.tag)
                    {
                        case "King":
                            DeActItems();
                            itemWork(itemdata.king);
                            SelectMove();
                            return;
                        case "Queen":
                            DeActItems();
                            itemWork(itemdata.queen);
                            SelectMove();
                            return;
                        case "Jack":
                            DeActItems();
                            itemWork(itemdata.jack);
                            Invoke("SelectMove", 0.3f);
                            return;
                        case "Ace":
                            DeActItems();
                            itemWork(itemdata.ace);
                            return;
                        default:
                            break;
                    }
                }
                if(hit.transform.gameObject.name == "MusicButton")
                {
                    return;
                }
            }
            if (!readytoMove)
            {
                if (firstmove)
                {
                    firstmove = false;
                }
                else
                {
                    goCycle();
                }
                SelectMove();
            }
            else
            {
                Move();
            }
        }
     }

    void FadeStartPanel()
    {
        if (!fading)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.transform.gameObject.tag == "StartPanel")
                    {
                        if (stageSelecter.text != "")
                        {
                            nowStage = int.Parse(stageSelecter.text);
                        }
                        Invoke("DestroyStartPanel", 5f);
                        fading = true;
                        this.GetComponent<AudioSource>().Play();
                        return;
                    }
                }
            }
        }
        else
        {
            StartPanel.transform.localScale = Vector3.Lerp(StartPanel.transform.localScale, Vector3.zero, Time.deltaTime * movespeed);
            StartPanel.transform.Translate(Vector3.forward * Time.deltaTime * movespeed);
        }
    }
    void DestroyStartPanel()
    {
        Destroy(StartPanel);
        StartPanel = null;
    }

    void SelectAce()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                switch (hit.transform.gameObject.name)
                {
                    case "RookCircle":
                        playerstate = 1;
                        rouletteangle = 0;
                        SetGrid((itemdata)playerstate, Mathf.FloorToInt(playerindex / 8), playerindex % 8);
                        rouletteParts[0].GetComponent<Renderer>().material = white;
                        rouletteParts[1].GetComponent<Renderer>().material = black;
                        rouletteParts[2].GetComponent<Renderer>().material = black;
                        rouletteParts[3].GetComponent<Renderer>().material = Gray;
                        rouletteParts[4].GetComponent<Renderer>().material = Gray;
                        rouletteParts[5].GetComponent<Renderer>().material = Gray;
                        SelectMove();
                        aceSelecting = false;
                        return;
                    case "BishopCircle":
                        playerstate = 2;
                        rouletteangle = -120;
                        SetGrid((itemdata)playerstate, Mathf.FloorToInt(playerindex / 8), playerindex % 8);
                        rouletteParts[0].GetComponent<Renderer>().material = black;
                        rouletteParts[1].GetComponent<Renderer>().material = white;
                        rouletteParts[2].GetComponent<Renderer>().material = black;
                        rouletteParts[3].GetComponent<Renderer>().material = Gray;
                        rouletteParts[4].GetComponent<Renderer>().material = Gray;
                        rouletteParts[5].GetComponent<Renderer>().material = Gray;
                        SelectMove();
                        aceSelecting = false;
                        return;
                    case "KnightCircle":
                        playerstate = 3;
                        rouletteangle = -240;
                        SetGrid((itemdata)playerstate, Mathf.FloorToInt(playerindex / 8), playerindex % 8);
                        rouletteParts[0].GetComponent<Renderer>().material = black;
                        rouletteParts[1].GetComponent<Renderer>().material = black;
                        rouletteParts[2].GetComponent<Renderer>().material = white;
                        rouletteParts[3].GetComponent<Renderer>().material = Gray;
                        rouletteParts[4].GetComponent<Renderer>().material = Gray;
                        rouletteParts[5].GetComponent<Renderer>().material = Gray;
                        SelectMove();
                        aceSelecting = false;
                        return;
                }
            }
        }
     }
    void RetryClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.name == "Retry")
                {
                    if(OnStage == 2)
                    {
                        nowStage--;
                    }
                    resultbox.text = "0";
                    OnStage = 0;
                    return;
                }
            }
        }

    }
    void MusicClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.name == "MusicButton")
                {
                    if (this.GetComponent<AudioSource>().mute)
                    {
                        this.GetComponent<AudioSource>().mute = false;
                        MusicButton.material = Gold;
                    }
                    else
                    {
                        this.GetComponent<AudioSource>().mute = true;
                        MusicButton.material = black;
                    }
                }
            }
        }

    }
}