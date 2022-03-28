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
    //스테이지 정보
    public string[] Stages;//스테이지 데이터 배열
    public string[] FlavorText;//스테이지 이름 목록
    public int[] BestRes;//스테이지별 최저 이동수
    private int nowStage;//현재 스테이지 넘버

    //맵 생성
    public GameObject[] Cubes; //맵에 있는 큐브 오브젝트
    public Transform origin; //a8 큐브 위치, 구멍 파괴 이펙트 붙여넣기에 사용
    private GameObject[] Mapobject;//목적지, 아이템, 플레이어 위치 데이터
    public GameObject[] items;//맵에 표시되는 아이템 프리펩
    public bool firstmove;//첫 이동이면 true, 첫번째 클릭은 말을 바꾸지 않음

    //플레이 상태 정보
    public int OnStage;//0이면 시작 전, 1이면 플레이중, 2이면 클리어
    private int playerindex;//플레이어 맵 위치
    public bool readytoMove;//다음 이동을 위한 준비 확인 (클릭시, 아이템 사용시 참으로 변환)
    public float movespeed;//플레이어 이동 속도
    public GameObject destination;//플레이어 목적지
    public bool moving;//플레이어 이동중인지 확인, 도착시 false
    public int playerstate;//현재 플레이어 말 상태
    private bool aceSelecting; //에이스 선택중인지 확인하는 논리값

    //판넬 이동
    public float panelSpeed;//판넬 이동 속도
    public Transform TitleAwayPos; //타이틀 판넬 치우는 위치
    public Transform TitleShowPos; //타이틀 판넬 보여주는 위치
    public Transform ResultAwayPos; //결과창 판넬 치우는 위치
    public Transform ResultShowPos; //결과창 판넬 보여주는 위치

    //판넬 UI
    public GameObject panel;//스테이지명 표시 판넬
    public GameObject resultPanel;//클리어 결과 안내 판넬
    public Text msgbox;//스테이지 이름 텍스트상자
    public Text resultbox;//클리어 기록 텍스트상자
    public Text resultComment;//클리어 기록 코멘트 텍스트 상자
    public Text bestResBox;//최저 이동수 표기 텍스트 상자
    public Text Moves;//이동수 카운터
    //public GameObject StartPanel;//시작 판넬
    //private bool fading; //시작판넬 완전히 지워졌는지 확인하는 논리값
    //public InputField stageSelecter; //스테이지 숏컷 입력 필드


    //룰렛 구성, 회전
    private int cycleDir;//사이클 도는 방향, 1이 정방향
    public GameObject dirArrow;//방향 가르키는 화살표
    public GameObject roulette;//룰렛
    public GameObject[] rouletteParts;//룰렛에 있는 말 모델들
    private int rouletteangle;//룰렛 목표 각도
    public float rotateSpeed;//룰렛 회전 속도




    /*장식 요소*/

    //아이템 위치, 모델
    [Header("items")]
    private bool itemActive;//아이템 활성 여부
    public GameObject[] Kings;//킹 위치들
    private int kingInd;//킹 현재 개수
    public GameObject[] Queens;
    private int queenInd;
    public GameObject[] Jacks;
    private int jackInd;
    public GameObject[] Aces;
    private int aceInd;
    public AudioSource itemSor;//아이템 사용 효과음 소스

    //맵 오브젝트들이 사용하는 매터리얼
    [Header ("Material")]
    public Material white;
    public Material black;
    public Material selectedWhite;//선택된 흰칸
    public Material selectedBlack;//선택된 검은칸
    public Material Gold;
    public Material Gray;
    public Material Red;
    public Material Blue;

    //클리어시 별
    [Header("Stars")]
    public Color[] StarColor;//별 색상들
    public GameObject[] StarGraphics;//별 오브젝트들

    //말 변화시 뿌려지는 파편 파티클 시스템
    [Header("ParticleSystem")]
    public GameObject particleManager;//파티클 매니저
    public ParticleSystem DebrisSystem;//파편 뿌리는 파티클 시스템
    public AudioSource DebrisSor;// 부서지는 소리 소스

    //무너지는 효과음
    public AudioSource ClearSor; //클리어 효과음 내는 오디오 소스
    private bool isHole; //맵에 구멍이 1개라도 있으면 무너지는 소리를 내기위해 판정하는 논리값
    public AudioSource BlockRumbleSor; //무너지는 효과음을 내는 오디오 소스

    //BGM 버튼
    public Renderer MusicButton; //음표 마크 렌더러, 음악 켜지거나 꺼지면 색상 바꾸기 위해 사용

    //배경 skybox 색상들
    public Color[] bgColors;//배경 색상들




    void Start()//값 초기화, 맵 큐브 정보 저장
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
            Cubes[i].GetComponent<cubeData>().index = i;//큐브 데이터에 인덱스 입력
        }
        itemActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        RetryClick();
        if (OnStage == 0)//스테이지 시작함. 레벨 이름 보여주기
        {
            ShowText();
        }
        else if (OnStage == 1)//스테이지 플레이 중
        {
            if (moving)//말 이동중 - 다른 입력 무시하고 말 도착할때까지 이동
            {
                MovePiece();
            }
            else if (aceSelecting)//에이스 발동중 - 에이스 사용 완료시까지 다른 입력 무시
            {
                SelectAce();
            }
            else//클릭해서 다음 말로 바꾸는지 확인
            {
                ClickButton();
            }
            RemoveText();
        }
        else if (OnStage == 2)//스테이지 클리어. 결과창과 정보 표기
        {
            ShowResult();
        }

        if(OnStage != 2)//클리어가 아닌경우엔 결과창 치워두기
        {
            RemoveResult();
        }
        RotateRoulette();//룰렛 각도 목표 각도로 돌리기
        MusicClick();//음악버튼 눌렀는지 확인

        /*
        if (StartPanel != null)
        {
            FadeStartPanel();
        }
        */
    }

    void SetStage(string stagedata)//스테이지 시작시 데이터를 받아 스테이지를 구성
    {
        for(int i = 0; i < 64; i++)
        {
            if(Cubes[i].activeInHierarchy == false)//이전 스테이지에서 구멍이었던 부분 메우기
            {
                Cubes[i].SetActive(true);
            }
            SetGrid(0, i);
        }
        DeSelectAll();//비선택 색상으로 바꿔주기
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
        for(int i = 0; i < Kings.Length; i++)//사용 아이템 비활성화
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

    void SetGrid(itemdata item, int index)//좌표로 아이템 가져오기
    {
        int alphabet = Mathf.FloorToInt(index / 8);
        int num = index - alphabet * 8;
        if (Mapobject[index] != null)//좌표에 아이템 이미 있음
        {
            Destroy(Mapobject[index]);//아이템 파괴
        }
        if((int)item == 9)//구멍인 경우
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
        if ((int)item != 0)//빈칸이 아니면 아이템 소환
        {
            Mapobject[index] = GameObject.Instantiate(items[(int)item], origin.position + Vector3.back * alphabet + Vector3.right * num, Quaternion.identity);
            if ((int)item > 0 && (int)item < 4)//플레이어 말 확인시
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

    void SelectCube(int index)//index 번째 큐브 선택 색상으로 변환
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

    void DeSelectAll()//모든 칸 비선택 색상으로 변환
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

    void Move()//목적지 클릭시 이동 시작
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

    void DeActItems()//아이템 사용 불가 상황(이미 다음 말로 바꿈)에서 비활성화
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

    void SelectMove()//이동할 수 있는 칸 표기
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
            case 1://룩
                bool doneR = false;
                bool doneL = false;
                bool doneU = false;
                bool doneD = false;
                for (int i = 1; i < 8; i++)
                {
                    //오른쪽 스캔
                    if (playerindex + i < Mapobject.Length && !doneR)//총 길이를 벗어나지 않는 범위
                    {
                        if (Mathf.FloorToInt((playerindex + i) / 8) == alphabet)//같은 알파벳 줄 확인
                        {
                            if (Mapobject[playerindex + i] == null)//빈칸인가?
                            {
                                SelectCube(playerindex + i);//선택하기
                            }
                            else if (Mapobject[playerindex + i].GetComponent<itemData>().data == 9)//구멍칸임
                            {
                                doneR = true;
                            }
                            else//구멍칸은 아님, 아이템칸임
                            {
                                SelectCube(playerindex + i);//선택하기
                                doneR = true;
                            }
                        }
                    }
                    //왼쪽 스캔
                    if (playerindex - i > -1 && !doneL)//총 길이를 벗어나지 않는 범위
                    {
                        if (Mathf.FloorToInt((playerindex - i) / 8) == alphabet)//같은 알파벳 줄 확인
                        {
                            if (Mapobject[playerindex - i] == null)//빈칸인가?
                            {
                                SelectCube(playerindex - i);//선택하기
                            }
                            else if (Mapobject[playerindex - i].GetComponent<itemData>().data == 9)//구멍칸임
                            {
                                doneL = true;
                            }
                            else//구멍칸은 아님, 아이템칸임
                            {
                                SelectCube(playerindex - i);//선택하기
                                doneL = true;
                            }
                        }
                    }
                    //아래 스캔
                    if (playerindex + 8 * i < Mapobject.Length && !doneD)//총 길이를 벗어나지 않는 범위
                    {
                        if (Mapobject[playerindex + 8 * i] == null)//빈칸인가?
                        {
                            SelectCube(playerindex + 8 * i);//선택하기
                        }
                        else if (Mapobject[playerindex + 8 * i].GetComponent<itemData>().data == 9)//구멍칸임
                        {
                            doneD = true;
                        }
                        else//구멍칸은 아님, 아이템칸임
                        {
                            SelectCube(playerindex + 8 * i);//선택하기
                            doneD = true;
                        }
                    }
                    //위 스캔
                    if (playerindex - 8 * i > -1 && !doneU)//총 길이를 벗어나지 않는 범위
                    {
                        if (Mapobject[playerindex - 8 * i] == null)//빈칸인가?
                        {
                            SelectCube(playerindex - 8 * i);//선택하기
                        }
                        else if (Mapobject[playerindex - 8 * i].GetComponent<itemData>().data == 9)//구멍칸임
                        {
                            doneU = true;
                        }
                        else//구멍칸은 아님, 아이템칸임
                        {
                            SelectCube(playerindex - 8 * i);//선택하기
                            doneU = true;
                        }
                    }
                }
                break;
            case 2://비숍
                bool doneRU = false;
                bool doneLU = false;
                bool doneRD = false;
                bool doneLD = false;
                for (int i = 1; i < 8; i++)
                {
                    //오른쪽 위 스캔
                    if (playerindex - 7 * i > -1 && !doneRU)//총 길이를 벗어나지 않는 범위
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex - 7 * i) / 8) + (playerindex - 7 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex - 7 * i] == null)//빈칸인가?
                            {
                                SelectCube(playerindex - 7 * i);//선택하기
                            }
                            else if (Mapobject[playerindex - 7 * i].GetComponent<itemData>().data == 9)//구멍칸임
                            {
                                doneRU = true;
                            }
                            else//구멍칸은 아님, 아이템칸임
                            {
                                SelectCube(playerindex - 7 * i);//선택하기
                                doneRU = true;
                            }
                        }
                    }
                    //왼쪽 위 스캔
                    if (playerindex - 9 * i > -1 && !doneLU)//총 길이를 벗어나지 않는 범위
                    {
                        if ((Mathf.FloorToInt(playerindex/8)+playerindex)%2 == (Mathf.FloorToInt((playerindex - 9 * i) / 8) + (playerindex - 9 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex - 9 * i] == null)//빈칸인가?
                            {
                                SelectCube(playerindex - 9 * i);//선택하기
                            }
                            else if (Mapobject[playerindex - 9 * i].GetComponent<itemData>().data == 9)//구멍칸임
                            {
                                doneLU = true;
                            }
                            else//구멍칸은 아님, 아이템칸임
                            {
                                SelectCube(playerindex - 9 * i);//선택하기
                                doneLU = true;
                            }
                        }
                    }
                    //오른쪽 아래 스캔
                    if (playerindex + 9 * i < Mapobject.Length && !doneRD)//총 길이를 벗어나지 않는 범위
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex + 9 * i) / 8) + (playerindex + 9 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex + 9 * i] == null)//빈칸인가?
                            {
                                SelectCube(playerindex + 9 * i);//선택하기
                            }
                            else if (Mapobject[playerindex + 9 * i].GetComponent<itemData>().data == 9)//구멍칸임
                            {
                                doneRD = true;
                            }
                            else//구멍칸은 아님, 아이템칸임
                            {
                                SelectCube(playerindex + 9 * i);//선택하기
                                doneRD = true;
                            }
                        }
                    }
                    //왼쪽 아래 스캔
                    if (playerindex + 7 * i < Mapobject.Length && !doneLD)//총 길이를 벗어나지 않는 범위
                    {
                        if ((Mathf.FloorToInt(playerindex / 8) + playerindex) % 2 == (Mathf.FloorToInt((playerindex + 7 * i) / 8) + (playerindex + 7 * i) % 8) % 2)
                        {
                            if (Mapobject[playerindex + 7 * i] == null)//빈칸인가?
                            {
                                SelectCube(playerindex + 7 * i);//선택하기
                            }
                            else if (Mapobject[playerindex + 7 * i].GetComponent<itemData>().data == 9)//구멍칸임
                            {
                                doneLD = true;
                            }
                            else//구멍칸은 아님, 아이템칸임
                            {
                                SelectCube(playerindex + 7 * i);//선택하기
                                doneLD = true;
                            }
                        }
                    }
                }
                break;
            case 3://나이트
                for(int alpha = -2; alpha < 3; alpha++)
                {
                    for(int num= -2; num < 3; num++)
                    {
                        if(playerindex + alpha * 8 + num > -1 && playerindex + alpha * 8 + num < 64)//총 길이를 벗어나지 않는 범위
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

    void MovePiece()//플레이어 목표지로 이동, 아이템 획득, 다음 이동을 위한 아이템 활성화
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

    void goCycle()//룰렛 목표 각도 조정
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

    void RotateRoulette()//룰렛 목표각도로 회전
    {
        roulette.transform.rotation = Quaternion.Lerp(roulette.transform.rotation, Quaternion.Euler(0, rouletteangle, 0), Time.deltaTime * rotateSpeed);
    }

    void itemSave(itemdata dat)//맵 아이템 획득 후 정보 저장, 슬롯 표기
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

    void itemWork(itemdata dat)//아이템 사용시 효과 발동
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

    void ShowText()//레벨 타이틀 갱신, 보여주기
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
                //MapMaker테스트를 위해 주석처리
                //SetStage(Stages[nowStage]);
                string Mapdata = PlayerPrefs.GetString("Mapdata");
                SetStage(Mapdata);
            }
        }
    }

    public void RemoveText()//레벨 타이틀 치우기
    {
        panel.transform.position = Vector3.Lerp(panel.gameObject.transform.position, TitleAwayPos.position, Time.deltaTime * panelSpeed);
        panel.transform.localScale = Vector3.Lerp(panel.gameObject.transform.localScale, TitleAwayPos.localScale, Time.deltaTime * panelSpeed);
    }

    void ShowResult()//결과 내용 갱신, 보여주기
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

    void RemoveResult()//결과 내용 치우기
    {
        resultPanel.transform.position = Vector3.Lerp(resultPanel.gameObject.transform.position, ResultAwayPos.position, Time.deltaTime * panelSpeed);
        resultPanel.transform.localScale = Vector3.Lerp(resultPanel.gameObject.transform.localScale, ResultAwayPos.localScale, Time.deltaTime * panelSpeed);
    }

    void ClickButton()//버튼 클릭시 어딜 눌렀는지에 따라 처리
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

    void KillEnemy()//적 유닛 잡았을 경우
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
    void SelectAce()//에이스 사용시 클릭 확인
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
    void RetryClick()//재시도 클릭 확인
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
    void MusicClick()//음악 버튼 클릭 확인
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