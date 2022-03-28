using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public enum itemdata
{
    empty = 0,
    rook = 1,
    bishop = 2,
    knight = 3,
    king = 4,
    queen = 5,
    jack = 6,
    ace = 7,
    enemy = 8,
    hole = 9
}

public class ProtoManager : MonoBehaviour
{
    //�������� ����
    public string[] Stages;//�������� ������ �迭
    public string[] FlavorText;//�������� �̸� ���
    public int[] BestRes;//���������� ���� �̵���
    private int nowStage;//���� �������� �ѹ�

    //�� ����
    public GameObject[] Cubes; //�ʿ� �ִ� ť�� ������Ʈ
    public Transform origin; //a8 ť�� ��ġ, ���� �ı� ����Ʈ �ٿ��ֱ⿡ ���
    private GameObject[] Mapobject;//������, ������, �÷��̾� ��ġ ������
    public GameObject[] items;//�ʿ� ǥ�õǴ� ������ ������
    public bool firstmove;//ù �̵��̸� true, ù��° Ŭ���� ���� �ٲ��� ����

    //�÷��� ���� ����
    public int OnStage;//0�̸� ���� ��, 1�̸� �÷�����, 2�̸� Ŭ����
    private int playerindex;//�÷��̾� �� ��ġ
    public bool readytoMove;//���� �̵��� ���� �غ� Ȯ�� (Ŭ����, ������ ���� ������ ��ȯ)
    public float movespeed;//�÷��̾� �̵� �ӵ�
    public GameObject destination;//�÷��̾� ������
    public bool moving;//�÷��̾� �̵������� Ȯ��, ������ false
    public int playerstate;//���� �÷��̾� �� ����
    private bool aceSelecting; //���̽� ���������� Ȯ���ϴ� ����

    //�ǳ� �̵�
    public float panelSpeed;//�ǳ� �̵� �ӵ�
    public Transform TitleAwayPos; //Ÿ��Ʋ �ǳ� ġ��� ��ġ
    public Transform TitleShowPos; //Ÿ��Ʋ �ǳ� �����ִ� ��ġ
    public Transform ResultAwayPos; //���â �ǳ� ġ��� ��ġ
    public Transform ResultShowPos; //���â �ǳ� �����ִ� ��ġ

    //�ǳ� UI
    public GameObject panel;//���������� ǥ�� �ǳ�
    public GameObject resultPanel;//Ŭ���� ��� �ȳ� �ǳ�
    public Text msgbox;//�������� �̸� �ؽ�Ʈ����
    public Text resultbox;//Ŭ���� ��� �ؽ�Ʈ����
    public Text resultComment;//Ŭ���� ��� �ڸ�Ʈ �ؽ�Ʈ ����
    public Text bestResBox;//���� �̵��� ǥ�� �ؽ�Ʈ ����
    public Text Moves;//�̵��� ī����
    //public GameObject StartPanel;//���� �ǳ�
    //private bool fading; //�����ǳ� ������ ���������� Ȯ���ϴ� ����
    //public InputField stageSelecter; //�������� ���� �Է� �ʵ�


    //�귿 ����, ȸ��
    private int cycleDir;//����Ŭ ���� ����, 1�� ������
    public GameObject dirArrow;//���� ����Ű�� ȭ��ǥ
    public GameObject roulette;//�귿
    public GameObject[] rouletteParts;//�귿�� �ִ� �� �𵨵�
    private int rouletteangle;//�귿 ��ǥ ����
    public float rotateSpeed;//�귿 ȸ�� �ӵ�




    /*��� ���*/

    //������ ��ġ, ��
    [Header("items")]
    private bool itemActive;//������ Ȱ�� ����
    public GameObject[] Kings;//ŷ ��ġ��
    private int kingInd;//ŷ ���� ����
    public GameObject[] Queens;
    private int queenInd;
    public GameObject[] Jacks;
    private int jackInd;
    public GameObject[] Aces;
    private int aceInd;
    public AudioSource itemSor;//������ ��� ȿ���� �ҽ�

    //�� ������Ʈ���� ����ϴ� ���͸���
    [Header ("Material")]
    public Material white;
    public Material black;
    public Material selectedWhite;//���õ� ��ĭ
    public Material selectedBlack;//���õ� ����ĭ
    public Material Gold;
    public Material Gray;
    public Material Red;
    public Material Blue;

    //Ŭ����� ��
    [Header("Stars")]
    public Color[] StarColor;//�� �����
    public GameObject[] StarGraphics;//�� ������Ʈ��

    //�� ��ȭ�� �ѷ����� ���� ��ƼŬ �ý���
    [Header("ParticleSystem")]
    public GameObject particleManager;//��ƼŬ �Ŵ���
    public ParticleSystem DebrisSystem;//���� �Ѹ��� ��ƼŬ �ý���
    public AudioSource DebrisSor;// �μ����� �Ҹ� �ҽ�

    //�������� ȿ����
    public AudioSource ClearSor; //Ŭ���� ȿ���� ���� ����� �ҽ�
    private bool isHole; //�ʿ� ������ 1���� ������ �������� �Ҹ��� �������� �����ϴ� ����
    public AudioSource BlockRumbleSor; //�������� ȿ������ ���� ����� �ҽ�

    //BGM ��ư
    public Renderer MusicButton; //��ǥ ��ũ ������, ���� �����ų� ������ ���� �ٲٱ� ���� ���

    //��� skybox �����
    public Color[] bgColors;//��� �����




    void Start()//�� �ʱ�ȭ, �� ť�� ���� ����
    {
        isHole = false;
        aceSelecting = false;
        //fading = false;
        rouletteangle = 0;
        firstmove = true;
        Mapobject = new GameObject[64];
        readytoMove = false;
        playerstate = 1;
        cycleDir = 1;
        OnStage = 0;
        for(int i = 0; i < 64; i++)
        {
            //Cubes[i].GetComponent<cubeData>().index = Mathf.FloorToInt(i / 8) + 8 * (7 - (i % 8));
            Cubes[i].GetComponent<cubeData>().index = i;//ť�� �����Ϳ� �ε��� �Է�
        }
        itemActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        RetryClick();
        if (OnStage == 0)//�������� ������. ���� �̸� �����ֱ�
        {
            ShowText();
        }
        else if (OnStage == 1)//�������� �÷��� ��
        {
            if (moving)//�� �̵��� - �ٸ� �Է� �����ϰ� �� �����Ҷ����� �̵�
            {
                MovePiece();
            }
            else if (aceSelecting)//���̽� �ߵ��� - ���̽� ��� �Ϸ�ñ��� �ٸ� �Է� ����
            {
                SelectAce();
            }
            else//Ŭ���ؼ� ���� ���� �ٲٴ��� Ȯ��
            {
                ClickButton();
            }
            RemoveText();
        }
        else if (OnStage == 2)//�������� Ŭ����. ���â�� ���� ǥ��
        {
            ShowResult();
        }

        if(OnStage != 2)//Ŭ��� �ƴѰ�쿣 ���â ġ���α�
        {
            RemoveResult();
        }
        RotateRoulette();//�귿 ���� ��ǥ ������ ������
        MusicClick();//���ǹ�ư �������� Ȯ��

        /*
        if (StartPanel != null)
        {
            FadeStartPanel();
        }
        */
    }

    void SetStage(string stagedata)//�������� ���۽� �����͸� �޾� ���������� ����
    {
        for(int i = 0; i < 64; i++)
        {
            if(Cubes[i].activeInHierarchy == false)//���� ������������ �����̾��� �κ� �޿��
            {
                Cubes[i].SetActive(true);
            }
            SetGrid(0, i);
        }
        DeSelectAll();//���� �������� �ٲ��ֱ�
        readytoMove = false;
        isHole = false;
        StringBuilder sb = new StringBuilder(stagedata);
        sb.Replace("\t", "");
        sb.Replace("\n", "");
        sb.Replace(" ", "");
        for (int i = 0; i < 64; i++)
        {
            SetGrid((itemdata) (sb[i] - '0'), i);
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
        for(int i = 0; i < Kings.Length; i++)//��� ������ ��Ȱ��ȭ
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
        OnStage = 1;
    }

    void SetGrid(itemdata item, int index)//��ǥ�� ������ ��������
    {
        int alphabet = Mathf.FloorToInt(index / 8);
        int num = index - alphabet * 8;
        if (Mapobject[index] != null)//��ǥ�� ������ �̹� ����
        {
            Destroy(Mapobject[index]);//������ �ı�
        }
        if((int)item == 9)//������ ���
        {
            Cubes[index].SetActive(false);
            Mapobject[index] = GameObject.Instantiate(items[(int)item], origin.position + Vector3.back * alphabet + Vector3.right * num, Quaternion.identity);
            ParticleSystemRenderer ps = Mapobject[index].GetComponent<ParticleSystemRenderer>();
            if ((alphabet + num) % 2 == 1)
            {
                ps.material = white;
            }
            else
            {
                ps.material = black;
            }
            Mapobject[index].GetComponent<ParticleSystem>().Play();
            if (!isHole)
            {
                isHole = true;
            }
        }
        if ((int)item != 0)//��ĭ�� �ƴϸ� ������ ��ȯ
        {
            Mapobject[index] = GameObject.Instantiate(items[(int)item], origin.position + Vector3.back * alphabet + Vector3.right * num, Quaternion.identity);
            if ((int)item > 0 && (int)item < 4)//�÷��̾� �� Ȯ�ν�
            {
                playerindex = index;
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

    void SelectCube(int index)//index ��° ť�� ���� �������� ��ȯ
    {
        int alphabet;
        int num;
        alphabet = Mathf.FloorToInt(index / 8);
        num = index % 8;
        if((alphabet + num) % 2 == 1)
        {
            Cubes[index].GetComponent<Renderer>().material = selectedBlack;
            Cubes[index].GetComponent<cubeData>().selected = true;
        }
        else
        {
            Cubes[index].GetComponent<Renderer>().material = selectedWhite;
            Cubes[index].GetComponent<cubeData>().selected = true;
        }
    }

    void DeSelectAll()//��� ĭ ���� �������� ��ȯ
    {
        for(int i = 0; i < 64; i++)
        {
            if (Cubes[i].GetComponent<cubeData>().selected)
            {
                int alphabet;
                int num;
                alphabet = Mathf.FloorToInt(i / 8);
                num = i % 8;
                if ((alphabet + num) % 2 == 1)
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

    void Move()//������ Ŭ���� �̵� ����
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

    void DeActItems()//������ ��� �Ұ� ��Ȳ(�̹� ���� ���� �ٲ�)���� ��Ȱ��ȭ
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

    void SelectMove()//�̵��� �� �ִ� ĭ ǥ��
    {
        DeActItems();
        int alphabet;
        int no;
        alphabet = Mathf.FloorToInt(playerindex / 8);
        no = playerindex - alphabet * 8;
        if (Moves.text != "0")
        {
            particleManager.transform.position = Cubes[playerindex].transform.position;
            DebrisSystem.Play();
            DebrisSystem.Emit(50);
            DebrisSor.Play();
        }
        switch (Mapobject[playerindex].GetComponent<itemData>().data)
        {
            case 1://��
                bool doneR = false;
                bool doneL = false;
                bool doneU = false;
                bool doneD = false;
                for (int i = 1; i < 8; i++)
                {
                    //������ ��ĵ
                    if (playerindex + i < Mapobject.Length && !doneR)//�� ���̸� ����� �ʴ� ����
                    {
                        if (Mathf.FloorToInt((playerindex + i) / 8) == alphabet)//���� ���ĺ� �� Ȯ��
                        {
                            if (Mapobject[playerindex + i] == null)//��ĭ�ΰ�?
                            {
                                SelectCube(playerindex + i);//�����ϱ�
                            }
                            else if (Mapobject[playerindex + i].GetComponent<itemData>().data == 9)//����ĭ��
                            {
                                doneR = true;
                            }
                            else//����ĭ�� �ƴ�, ������ĭ��
                            {
                                SelectCube(playerindex + i);//�����ϱ�
                                doneR = true;
                            }
                        }
                    }
                    //���� ��ĵ
                    if (playerindex - i > -1 && !doneL)//�� ���̸� ����� �ʴ� ����
                    {
                        if (Mathf.FloorToInt((playerindex - i) / 8) == alphabet)//���� ���ĺ� �� Ȯ��
                        {
                            if (Mapobject[playerindex - i] == null)//��ĭ�ΰ�?
                            {
                                SelectCube(playerindex - i);//�����ϱ�
                            }
                            else if (Mapobject[playerindex - i].GetComponent<itemData>().data == 9)//����ĭ��
                            {
                                doneL = true;
                            }
                            else//����ĭ�� �ƴ�, ������ĭ��
                            {
                                SelectCube(playerindex - i);//�����ϱ�
                                doneL = true;
                            }
                        }
                    }
                    //�Ʒ� ��ĵ
                    if (playerindex + 8 * i < Mapobject.Length && !doneD)//�� ���̸� ����� �ʴ� ����
                    {
                        if (Mapobject[playerindex + 8 * i] == null)//��ĭ�ΰ�?
                        {
                            SelectCube(playerindex + 8 * i);//�����ϱ�
                        }
                        else if (Mapobject[playerindex + 8 * i].GetComponent<itemData>().data == 9)//����ĭ��
                        {
                            doneD = true;
                        }
                        else//����ĭ�� �ƴ�, ������ĭ��
                        {
                            SelectCube(playerindex + 8 * i);//�����ϱ�
                            doneD = true;
                        }
                    }
                    //�� ��ĵ
                    if (playerindex - 8 * i > -1 && !doneU)//�� ���̸� ����� �ʴ� ����
                    {
                        if (Mapobject[playerindex - 8 * i] == null)//��ĭ�ΰ�?
                        {
                            SelectCube(playerindex - 8 * i);//�����ϱ�
                        }
                        else if (Mapobject[playerindex - 8 * i].GetComponent<itemData>().data == 9)//����ĭ��
                        {
                            doneU = true;
                        }
                        else//����ĭ�� �ƴ�, ������ĭ��
                        {
                            SelectCube(playerindex - 8 * i);//�����ϱ�
                            doneU = true;
                        }
                    }
                }
                break;
            case 2://���
                bool doneRU = false;
                bool doneLU = false;
                bool doneRD = false;
                bool doneLD = false;
                for (int i = 1; i < 8; i++)
                {
                    //������ �� ��ĵ
                    if (playerindex - 7 * i > -1 && !doneRU)//�� ���̸� ����� �ʴ� ����
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex - 7 * i) / 8) + (playerindex - 7 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex - 7 * i] == null)//��ĭ�ΰ�?
                            {
                                SelectCube(playerindex - 7 * i);//�����ϱ�
                            }
                            else if (Mapobject[playerindex - 7 * i].GetComponent<itemData>().data == 9)//����ĭ��
                            {
                                doneRU = true;
                            }
                            else//����ĭ�� �ƴ�, ������ĭ��
                            {
                                SelectCube(playerindex - 7 * i);//�����ϱ�
                                doneRU = true;
                            }
                        }
                    }
                    //���� �� ��ĵ
                    if (playerindex - 9 * i > -1 && !doneLU)//�� ���̸� ����� �ʴ� ����
                    {
                        if ((Mathf.FloorToInt(playerindex/8)+playerindex)%2 == (Mathf.FloorToInt((playerindex - 9 * i) / 8) + (playerindex - 9 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex - 9 * i] == null)//��ĭ�ΰ�?
                            {
                                SelectCube(playerindex - 9 * i);//�����ϱ�
                            }
                            else if (Mapobject[playerindex - 9 * i].GetComponent<itemData>().data == 9)//����ĭ��
                            {
                                doneLU = true;
                            }
                            else//����ĭ�� �ƴ�, ������ĭ��
                            {
                                SelectCube(playerindex - 9 * i);//�����ϱ�
                                doneLU = true;
                            }
                        }
                    }
                    //������ �Ʒ� ��ĵ
                    if (playerindex + 9 * i < Mapobject.Length && !doneRD)//�� ���̸� ����� �ʴ� ����
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex + 9 * i) / 8) + (playerindex + 9 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex + 9 * i] == null)//��ĭ�ΰ�?
                            {
                                SelectCube(playerindex + 9 * i);//�����ϱ�
                            }
                            else if (Mapobject[playerindex + 9 * i].GetComponent<itemData>().data == 9)//����ĭ��
                            {
                                doneRD = true;
                            }
                            else//����ĭ�� �ƴ�, ������ĭ��
                            {
                                SelectCube(playerindex + 9 * i);//�����ϱ�
                                doneRD = true;
                            }
                        }
                    }
                    //���� �Ʒ� ��ĵ
                    if (playerindex + 7 * i < Mapobject.Length && !doneLD)//�� ���̸� ����� �ʴ� ����
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex + 7 * i) / 8) + (playerindex + 7 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex + 7 * i] == null)//��ĭ�ΰ�?
                            {
                                SelectCube(playerindex + 7 * i);//�����ϱ�
                            }
                            else if (Mapobject[playerindex + 7 * i].GetComponent<itemData>().data == 9)//����ĭ��
                            {
                                doneLD = true;
                            }
                            else//����ĭ�� �ƴ�, ������ĭ��
                            {
                                SelectCube(playerindex + 7 * i);//�����ϱ�
                                doneLD = true;
                            }
                        }
                    }
                }
                break;
            case 3://����Ʈ
                for(int alpha = -2; alpha < 3; alpha++)
                {
                    for(int num= -2; num < 3; num++)
                    {
                        if(playerindex + alpha * 8 + num > -1 && playerindex + alpha * 8 + num < 64)//�� ���̸� ����� �ʴ� ����
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

    void MovePiece()//�÷��̾� ��ǥ���� �̵�, ������ ȹ��, ���� �̵��� ���� ������ Ȱ��ȭ
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
            SetGrid((itemdata) playerstate, playerindex);
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

    void goCycle()//�귿 ��ǥ ���� ����
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
        SetGrid((itemdata)playerstate, playerindex);
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

    void RotateRoulette()//�귿 ��ǥ������ ȸ��
    {
        roulette.transform.rotation = Quaternion.Lerp(roulette.transform.rotation, Quaternion.Euler(0, rouletteangle, 0), Time.deltaTime * rotateSpeed);
    }

    void itemSave(itemdata dat)//�� ������ ȹ�� �� ���� ����, ���� ǥ��
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
            case itemdata.enemy:
                KillEnemy();
                break;
        }
    }

    void itemWork(itemdata dat)//������ ���� ȿ�� �ߵ�
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

    void ShowText()//���� Ÿ��Ʋ ����, �����ֱ�
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
            if (Physics.Raycast(ray, out hit, 100f) && hit.transform.gameObject.name == "panel")
            {
                //MapMaker�׽�Ʈ�� ���� �ּ�ó��
                //SetStage(Stages[nowStage]);
                string Mapdata = PlayerPrefs.GetString("Mapdata");
                SetStage(Mapdata);
            }
        }
    }

    public void RemoveText()//���� Ÿ��Ʋ ġ���
    {
        panel.transform.position = Vector3.Lerp(panel.gameObject.transform.position, TitleAwayPos.position, Time.deltaTime * panelSpeed);
        panel.transform.localScale = Vector3.Lerp(panel.gameObject.transform.localScale, TitleAwayPos.localScale, Time.deltaTime * panelSpeed);
    }

    void ShowResult()//��� ���� ����, �����ֱ�
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

    void RemoveResult()//��� ���� ġ���
    {
        resultPanel.transform.position = Vector3.Lerp(resultPanel.gameObject.transform.position, ResultAwayPos.position, Time.deltaTime * panelSpeed);
        resultPanel.transform.localScale = Vector3.Lerp(resultPanel.gameObject.transform.localScale, ResultAwayPos.localScale, Time.deltaTime * panelSpeed);
    }

    void ClickButton()//��ư Ŭ���� ��� ���������� ���� ó��
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

    void KillEnemy()//�� ���� ����� ���
    {
        int Ecount;
        Ecount = 0;
        for(int i = 0; i < Mapobject.Length; i++)
        {
            if(Mapobject[i] != null && Mapobject[i].GetComponent<itemData>().data == 8)
            {
                Ecount++;
            }
        }
        if (Ecount == 1)
        {
            OnStage = 2;
            nowStage++;
        }

    }

    /*
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
    */
    /*
    void DestroyStartPanel()
    {
        Destroy(StartPanel);
        StartPanel = null;
    }
    */
    void SelectAce()//���̽� ���� Ŭ�� Ȯ��
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
                        SetGrid((itemdata)playerstate, playerindex);
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
                        SetGrid((itemdata)playerstate, playerindex);
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
                        SetGrid((itemdata)playerstate, playerindex);
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
    void RetryClick()//��õ� Ŭ�� Ȯ��
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
    void MusicClick()//���� ��ư Ŭ�� Ȯ��
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