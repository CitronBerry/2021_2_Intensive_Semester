using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    #region STATIC
    enum Stairs_State { start, left, right }
    Vector3 startPos = new Vector3(-0.8f, -4f, 0);
    Vector3 leftDir = new Vector3(-1.12f, 0.6f, 0);
    Vector3 rightDir = new Vector3(1.12f, 0.6f, 0);
    #endregion

    #region FIELDS

    public GameObject Player;
    Animator playerAnimator;

    public GameObject[] Stairs;
    bool isDirection = false;                   // false면 왼쪽, true면 오른쪽
    public bool[] isReverse = new bool[30];     // 방향을 바꿔야 하는지 확인하는 변수
    public int StairIndex = 0;

    Vector3 beforePos;
    
    public GameObject BackGround;

    public Image Gauge;
    public bool isGaugeStart = false;
    float GaugeRate = 0.002f;

    public Text TextScore;
    int Score = 0;

    public Image ClimbInfo;                     // 오르기 설명
    public Image ReverseInfo;                   // 방향전환 설명

    public bool isFail = false;

    Stairs_State state = Stairs_State.start;

    #endregion



    void Start()
    {
        playerAnimator = Player.GetComponent<Animator>();
        StairsInit();
        // 게이지 시작
        StartCoroutine("GaugeCheck");
        GaugeMove();
    }


    void Update()
    {
        if (isFail == true)
        {
            GameOver();
        }

        if (isGaugeStart == true)
        {
            ClimbInfo.gameObject.SetActive(false);
            ReverseInfo.gameObject.SetActive(false);
        }

        // 키보드로 이동
        if (isFail == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isGaugeStart = true;
                Climb();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Reverse();
            }
            ShowPlayScore();
        }

    }

    void StairsInit()
    {
        // 맨 처음에는 무조건 왼쪽으로 가야 함
        for (int i = 0; i < 30; i++)
        {
            switch (state)
            {
                case Stairs_State.start:
                    // isReverse[i]가 false면 왼쪽으로, true면 오르쪽으로 가는 방향
                    Stairs[i].transform.position = startPos;
                    isReverse[i] = false;
                    state = Stairs_State.left;
                    break;
                case Stairs_State.left:
                    Stairs[i].transform.position = beforePos + leftDir;
                    isReverse[i] = false;
                    break;
                case Stairs_State.right:
                    Stairs[i].transform.position = beforePos + rightDir;
                    isReverse[i] = true;
                    break;
            }
            beforePos = Stairs[i].transform.position;

            if(i != 0)
            {
                if( Random.Range(1, 10) < 3)
                {
                    if (state == Stairs_State.left)
                    {
                        state = Stairs_State.right;
                    }
                    else if (state == Stairs_State.right)
                    {
                        state = Stairs_State.left;
                    }
                }
            }
        }
    }

    public void Climb()
    {
        MoveAni();
        StairsMove(StairIndex, isDirection);
        if (++StairIndex == 30) StairIndex = 0;
    }

    public void Reverse()
    {
        if (isDirection == false)
        {
            Player.transform.rotation = Quaternion.Euler(0, -180, 0);
            isDirection = true;
        }
        else if (isDirection == true)
        {
            Player.transform.rotation = Quaternion.Euler(0, 0, 0);
            isDirection = false;
        }
        Climb();
    }

    void StairsCreate(int _num, bool _isDir)
    {
        if (_isDir == false)        // 왼쪽
        {
            Stairs[_num].transform.position = beforePos + leftDir;
            isReverse[_num] = false;
        }
        else if (_isDir == true)    // 오른쪽
        {
            Stairs[_num].transform.position = beforePos + rightDir;
            isReverse[_num] = true;
        }
        beforePos = Stairs[_num].transform.position;
    }

    public void StairsMove(int _index, bool _isDir)
    {
        if (isFail == true) return;

        // 계단 이동
        for (int i = 0; i < 30; i++)
        {
            if (_isDir == false) Stairs[i].transform.position -= leftDir;
            else if (_isDir == true) Stairs[i].transform.position -= rightDir;

        }

        // 안내려주면 좌표 꼬임
        if(_isDir == false) beforePos -= leftDir;
        else if (_isDir == true) beforePos -= rightDir;

        // 계단의 위치가 -5밑으로 내려갔을 때 그 계단을 위에 이동
        for (int i = 0; i < 30; i++)
        {
            if (Stairs[i].transform.position.y < -6f)
            {
                if (Random.Range(1, 10) < 5)
                    StairsCreate(i, false);
                else
                    StairsCreate(i, true);

            }
        }

        // 잘못 갔으면 게임오버
        if(isReverse[_index] != _isDir)
        {
            isFail = true;
            return;
        }

        // 성공하면 스코어와 게이지 오름
        Score++;
        Gauge.fillAmount += GaugeRate + 0.02f;

        // 배경화면 이동
        if (BackGround.transform.position.y >= -3.8)
        {
            Vector3 temp = new Vector3(0, -0.05f, 0);
            BackGround.transform.position += temp;
        }
    }

    void GaugeMove()
    {
        if (isGaugeStart == true)
        {
            // 스코어에 따른 게이지 감소량 증가
            if (Score > 30)  GaugeRate = 0.003f;
            if (Score > 60)  GaugeRate = 0.004f;
            if (Score > 100) GaugeRate = 0.005f;
            if (Score > 150) GaugeRate = 0.006f;
            if (Score > 210) GaugeRate = 0.007f;
            if (Score > 280) GaugeRate = 0.008f;
            if (Score > 370) GaugeRate = 0.009f;
            if (Score > 500) GaugeRate = 0.01f;
            Gauge.fillAmount -= GaugeRate;
        }
        Invoke("GaugeMove", 0.15f);
    }

    IEnumerator GaugeCheck()
    {
        // 게이지가 없을면 게임 오버
        while(Gauge.fillAmount != 0)
        {
            yield return new WaitForSeconds(0.3f);
        }
        isFail = true;
    }

    void ShowPlayScore()
    {
        TextScore.text = Score.ToString();
    }

    void MoveAni()
    {
        playerAnimator.SetBool("Move", true);

        Invoke("IdleAni", 0.05f);
    }

    void IdleAni()
    {
        playerAnimator.SetBool("Move", false);
    }

    void GameOver()
    {
        // 게이지 정지
        CancelInvoke();
        isGaugeStart = false;

        playerAnimator.SetBool("Move", false);
        // 캐릭터 애니메이션 
        playerAnimator.SetBool("Die", true);
    }

}
