using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

enum itemdata_new
{
    empty = '0',
    hole = '1',
    playerR = '2',
    playerB = '3',
    playerN = '4',
    P = 'p',
    R = 'r',
    B = 'b',
    N = 'n',
    Q = 'q',
    K = 'k'
}

public class ProtoManager : MonoBehaviour
{
    //�������� ����
    public List<string> Stages;//�������� ������ �迭
    public List<string> FlavorText;//�������� �̸� ���
    public List<int> BestRes;//���������� ���� �̵���
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
    public int playerstate;//���� �÷��̾� �� ����; 2,3,4 ��,���,����Ʈ
    private bool aceSelecting; //���̽� ���������� Ȯ���ϴ� ����

    //�ǳ� �̵�
    public float panelSpeed;//�ǳ� �̵� �ӵ�
    public Transform TitleAwayPos; //Ÿ��Ʋ �ǳ� ġ��� ��ġ
    public Transform TitleShowPos; //Ÿ��Ʋ �ǳ� �����ִ� ��ġ
    public Transform ResultAwayPos; //���â �ǳ� ġ��� ��ġ
    public Transform ResultShowPos; //���â �ǳ� �����ִ� ��ġ
    public bool resultSaved;//�����ϸ� true
    public bool resultEditing;//�����߿� true
    public InputField FlavIF;//�÷��̹� �Է�â
    public InputField BestResIF;//�ְ��� �Է�â
    public InputField changeStageNo;//�̵��� �������� �Է�â

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

    /*�� ���� ����*/
    public char nowitem;
    public GameObject nowObj;

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
    public Material trans;

    //Ŭ����� ��
    [Header("Stars")]
    public Color[] StarColor;//�� �����
    public GameObject[] StarGraphics;//�� ������Ʈ��
    public AudioSource ClearSor;

    //�� ��ȭ�� �ѷ����� ���� ��ƼŬ �ý���
    [Header("ParticleSystem")]
    public GameObject particleManager;//��ƼŬ �Ŵ���
    public ParticleSystem DebrisSystem;//���� �Ѹ��� ��ƼŬ �ý���
    public AudioSource DebrisSor;// �μ����� �Ҹ� �ҽ�

    //BGM ��ư
    public Renderer MusicButton; //��ǥ ��ũ ������, ���� �����ų� ������ ���� �ٲٱ� ���� ���

    //MapEdit ��ư
    public Renderer EditButton; //�ʿ��� ��ư ���

    //��� skybox �����
    public Color[] bgColors;//��� �����




    void Start()//�� �ʱ�ȭ, �� ť�� ���� ����
    {
        try
        {
            LoadMap();
        }
        catch
        {
            Stages = new List<string>();
            Stages.Add("");
            FlavorText = new List<string>();
            FlavorText.Add("Lv.0");
            BestRes = new List<int>();
            BestRes.Add(999);
            SaveSystem.SaveStage(this);
        }
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
        nowitem = '0';
        nowObj = GameObject.Instantiate(items[Data2Ind(nowitem)], Moves.transform.position, Quaternion.identity);
        nowObj.SetActive(false);
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
        else if (OnStage == 3)//�ʿ���
        {
            MapEdit();
        }

        if(OnStage != 2)//Ŭ��� �ƴѰ�쿣 ���â ġ���α�
        {
            RemoveResult();
        }
        RotateRoulette();//�귿 ���� ��ǥ ������ ������
        MusicClick();//���ǹ�ư �������� Ȯ��
        MapEditClick();//�ʿ��� ��ư ���ȴ��� Ȯ��
        ChangeStage();//esc ������ �������� ����

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
            }
            Destroy(Mapobject[i]);
        }
        DeSelectAll();//���� �������� �ٲ��ֱ�
        readytoMove = false;
        StringBuilder sb = new StringBuilder(stagedata);
        sb.Replace("\t", "");
        sb.Replace("\n", "");
        sb.Replace(" ", "");
        if (stagedata.Length < 64)
        {
            for (int i = 0; i < 64 - stagedata.Length; i++)
            {
                sb.Append('1');
            }
        }
        Stages[nowStage] = sb.ToString();
        for (int i = 0; i < 64; i++)
        {
            SetGrid(sb[i], i);
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
        resultEditing = false;
        resultSaved = false;
        if(OnStage != 3)
        {
            OnStage = 1;
        }
    }
    void SetGrid(int item, int index)
    {
        SetGrid((char)(item + '0'), index);
    }
    void SetGrid(char item, int index)//��ǥ�� ������ ��������
    {
        int alphabet = Mathf.FloorToInt(index / 8);
        int num = index - alphabet * 8;
        if (Mapobject[index] != null)//��ǥ�� ������ �̹� ����
        {
            Destroy(Mapobject[index]);//������ �ı�
        }

        switch (Data2Ind(item))
        {
            case 0:
                if (OnStage != 3)
                {
                    Cubes[index].SetActive(false);
                }
                else
                {
                    Cubes[index].GetComponent<Renderer>().material = trans;
                }
                break;
            case 1:
                if ((alphabet + num) % 2 == 1)
                {
                    Cubes[index].GetComponent<Renderer>().material = black;
                }
                else
                {
                    Cubes[index].GetComponent<Renderer>().material = white;
                }
                break;
            case 2:
            case 3:
            case 4:
                playerindex = index;
                playerstate = item - '0';
                Mapobject[index] = GameObject.Instantiate(items[item - '0'], origin.position + Vector3.back * alphabet + Vector3.right * num, Quaternion.identity);
                break;
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                Mapobject[index] = GameObject.Instantiate(items[Data2Ind(item)], origin.position + Vector3.back * alphabet + Vector3.right * num, Quaternion.identity);
                break;
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

    void CheckBlockClick()//������ Ŭ���� �̵� ����
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
            case '2'://��
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
                            else if (Mapobject[playerindex + i].GetComponent<itemData>().data == '0')//����ĭ��
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
                            else if (Mapobject[playerindex - i].GetComponent<itemData>().data == '0')//����ĭ��
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
                        else if (Mapobject[playerindex + 8 * i].GetComponent<itemData>().data == '0')//����ĭ��
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
                        else if (Mapobject[playerindex - 8 * i].GetComponent<itemData>().data == '0')//����ĭ��
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
            case '3'://���
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
                            else if (Mapobject[playerindex - 7 * i].GetComponent<itemData>().data == '0')//����ĭ��
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
                            else if (Mapobject[playerindex - 9 * i].GetComponent<itemData>().data == '0')//����ĭ��
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
                            else if (Mapobject[playerindex + 9 * i].GetComponent<itemData>().data == '0')//����ĭ��
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
                            else if (Mapobject[playerindex + 7 * i].GetComponent<itemData>().data == '0')//����ĭ��
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
            case '4'://����Ʈ
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
                int itemtemp = Data2Ind(Mapobject[playerindex].GetComponent<itemData>().data);
                if (itemtemp > 4 && itemtemp < 11)
                {
                    StepItem(itemtemp);
                }
            }
            SetGrid(playerstate, playerindex);
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
        if (playerstate == 5)
        {
            playerstate = 2;
        }
        else if (playerstate == 1)
        {
            playerstate = 4;
        }
        SetGrid(playerstate, playerindex);
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

    void StepItem(int dat)//�� ������ ����
    {
        if (dat > 4)
        {
            KillEnemy();
        }
        /*
        switch (dat)
        {
            case '4':
                Kings[kingInd].SetActive(true);
                kingInd++;
                break;
            case '5':
                Queens[queenInd].SetActive(true);
                queenInd++;
                break;
            case '6':
                Jacks[jackInd].SetActive(true);
                jackInd++;
                break;
            case '7':
                Aces[aceInd].SetActive(true);
                aceInd++;
                break;
            case '8':
                KillEnemy();
                break;
        }*/
    }

    void itemWork(char dat)//������ ���� ȿ�� �ߵ�
    {
        itemSor.Play();
        switch (dat)
        {
            /*
            case itemdata_new.king:
                kingInd--;
                Kings[kingInd].SetActive(false);
                break;
            case itemdata_new.queen:
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
            case itemdata_new.jack:
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
            case itemdata_new.ace:
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
            */
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
                SetStage(Stages[nowStage]);
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
                resultComment.text = "New Record!!!";
                StarGraphics[0].SetActive(true);
                StarGraphics[0].GetComponent<Renderer>().material.color = StarColor[3];
                StarGraphics[1].SetActive(true);
                StarGraphics[1].GetComponent<Renderer>().material.color = StarColor[3];
                StarGraphics[2].SetActive(true);
                StarGraphics[2].GetComponent<Renderer>().material.color = StarColor[3];
                BestRes[nowStage - 1] = int.Parse(resultbox.text);
                SaveSystem.SaveStage(this);
            }
            else if (int.Parse(resultbox.text) == BestRes[nowStage - 1])
            {
                resultComment.text = "Excellent move!!";
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
                resultComment.text = "Nice move";
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
        if (Input.GetMouseButtonDown(0) && !resultEditing && resultSaved)
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
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (itemActive)
                {
                    switch (hit.transform.gameObject.tag)
                    {
                        /*
                        case "King":
                            DeActItems();
                            itemWork(itemdata_new.king);
                            SelectMove();
                            return;
                        case "Queen":
                            DeActItems();
                            itemWork(itemdata_new.queen);
                            SelectMove();
                            return;
                        case "Jack":
                            DeActItems();
                            itemWork(itemdata_new.jack);
                            Invoke("SelectMove", 0.3f);
                            return;
                        case "Ace":
                            DeActItems();
                            itemWork(itemdata_new.ace);
                            return;
                        default:
                            break;
                        */
                    }
                }
                if(hit.transform.gameObject.name == "MusicButton" || hit.transform.gameObject.name == "MapEditButton")
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
                CheckBlockClick();
            }
        }
     }

    void KillEnemy()//�� ���� ����� ���
    {
        int Ecount;
        Ecount = 0;
        for(int i = 0; i < Mapobject.Length; i++)
        {
            if(Mapobject[i] != null && Data2Ind(Mapobject[i].GetComponent<itemData>().data) > 4)
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
                        SetGrid(playerstate, playerindex);
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
                        SetGrid(playerstate, playerindex);
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
                        SetGrid(playerstate, playerindex);
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
        if (Input.GetMouseButtonDown(0) && OnStage != 3)
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

    void MapEditClick()
    {
        if (OnStage != 0 && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.name == "MapEditButton")
                {
                    SaveSystem.SaveStage(this);
                    if (OnStage != 3)
                    {
                        OnStage = 3;
                        EditButton.material = white;
                        SetStage(Stages[nowStage]);
                        Camera.main.backgroundColor = Color.black;
                        nowObj.SetActive(true);
                    }
                    else
                    {
                        LoadMap();
                        OnStage = 0;
                        EditButton.material = black;
                        Camera.main.backgroundColor = bgColors[nowStage % bgColors.Length];
                        nowObj.SetActive(false);
                    }
                }
            }
        }
    }

    void MapEdit()
    {
        if (nowObj.GetComponent<itemData>() && nowObj.GetComponent<itemData>().data != nowitem)
        {
            Destroy(nowObj);
            nowObj = GameObject.Instantiate(items[Data2Ind(nowitem)], Moves.transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            nowitem = '0';
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            nowitem = '1';
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            nowitem = '2';
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            nowitem = '3';
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            nowitem = '4';
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            nowitem = 'p';
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            nowitem = 'r';
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            nowitem = 'b';
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            nowitem = 'n';
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            nowitem = 'q';
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            nowitem = 'k';
        }



        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.tag == "cube")
                {
                    int targetInd = hit.transform.gameObject.GetComponent<cubeData>().index;
                    SetGrid(nowitem, targetInd);
                    StringBuilder sb = new StringBuilder(Stages[nowStage]);
                    sb[targetInd] = nowitem;
                    Stages[nowStage] = sb.ToString();
                }
            }
        }
    }

    void LoadMap()
    {
        DataSaver data = SaveSystem.LoadStage();

        Stages = data.Stages;
        BestRes = data.BestRes;
        FlavorText = data.FlavorText;
    }

    int Data2Ind(char dat)
    {
        switch (dat)
        {
            case '0':
                return 0;
            case '1':
                return 1;
            case '2':
                return 2;
            case '3':
                return 3;
            case '4':
                return 4;
            case 'p':
                return 5;
            case 'r':
                return 6;
            case 'b':
                return 7;
            case 'n':
                return 8;
            case 'q':
                return 9;
            case 'k':
                return 10;
            default:
                return 1;
        }
    }
    char Ind2Data(int dat)
    {
        switch (dat)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                return (char)(dat+'0');
            case 5:
                return 'p';
            case 6:
                return 'r';
            case 7:
                return 'b';
            case 8:
                return 'n';
            case 9:
                return 'q';
            case 10:
                return 'k';
            default:
                return '1';
        }
    }

    public void SaveResult()
    {
        FlavorText[nowStage - 1] = FlavIF.text;
        int temp;
        if (int.TryParse(BestResIF.text, out temp))
        {
            BestRes[nowStage - 1] = int.Parse(BestResIF.text);
        }
        SaveSystem.SaveStage(this);
        if (nowStage >= Stages.Count)
        {
            Stages.Add("");
            BestRes.Add(999);
            FlavorText.Add("Lv. " + nowStage);
        }
        resultSaved = true;
    }

    public void EditResult()
    {
        if (!resultEditing)
        {
            resultEditing = true;
            BestResIF.gameObject.SetActive(true);
            BestResIF.text = BestRes[nowStage - 1].ToString();
            bestResBox.gameObject.SetActive(false);
            FlavIF.gameObject.SetActive(true);
            FlavIF.text = FlavorText[nowStage - 1];
            resultComment.gameObject.SetActive(false);
        }
        else
        {
            FlavorText[nowStage - 1] = FlavIF.text;
            int temp;
            if (int.TryParse(BestResIF.text, out temp))
            {
                BestRes[nowStage - 1] = int.Parse(BestResIF.text);
            }
            SaveSystem.SaveStage(this);
            ShowResult();
            resultEditing = false;
            BestResIF.gameObject.SetActive(false);
            bestResBox.gameObject.SetActive(true);
            FlavIF.gameObject.SetActive(false);
            resultComment.gameObject.SetActive(true);
        }
    }

    public void ChangeStage()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (msgbox.gameObject.activeSelf)
            {
                msgbox.gameObject.SetActive(false);
                changeStageNo.gameObject.SetActive(true);
                changeStageNo.placeholder.GetComponent<Text>().text = (Stages.Count - 1).ToString();
            }
            else
            {
                int temp;
                if (int.TryParse(changeStageNo.text, out temp) && int.Parse(changeStageNo.text) < Stages.Count && OnStage != 3)
                {
                    nowStage = int.Parse(changeStageNo.text);
                    resultbox.text = "0";
                    OnStage = 0;
                }
                msgbox.gameObject.SetActive(true);
                changeStageNo.gameObject.SetActive(false);
            }
        }
    }
}