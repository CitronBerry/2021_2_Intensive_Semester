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
    bool isDirection = false;                   // false�� ����, true�� ������
    public bool[] isReverse = new bool[30];     // ������ �ٲ�� �ϴ��� Ȯ���ϴ� ����
    public int StairIndex = 0;

    Vector3 beforePos;
    
    public GameObject BackGround;

    public Image Gauge;
    public bool isGaugeStart = false;
    float GaugeRate = 0.002f;

    public Text TextScore;
    int Score = 0;

    public Image ClimbInfo;                     // ������ ����
    public Image ReverseInfo;                   // ������ȯ ����

    public bool isFail = false;

    Stairs_State state = Stairs_State.start;

    #endregion



    void Start()
    {
        playerAnimator = Player.GetComponent<Animator>();
        StairsInit();
        // ������ ����
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

        // Ű����� �̵�
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
        // �� ó������ ������ �������� ���� ��
        for (int i = 0; i < 30; i++)
        {
            switch (state)
            {
                case Stairs_State.start:
                    // isReverse[i]�� false�� ��������, true�� ���������� ���� ����
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
        if (_isDir == false)        // ����
        {
            Stairs[_num].transform.position = beforePos + leftDir;
            isReverse[_num] = false;
        }
        else if (_isDir == true)    // ������
        {
            Stairs[_num].transform.position = beforePos + rightDir;
            isReverse[_num] = true;
        }
        beforePos = Stairs[_num].transform.position;
    }

    public void StairsMove(int _index, bool _isDir)
    {
        if (isFail == true) return;

        // ��� �̵�
        for (int i = 0; i < 30; i++)
        {
            if (_isDir == false) Stairs[i].transform.position -= leftDir;
            else if (_isDir == true) Stairs[i].transform.position -= rightDir;

        }

        // �ȳ����ָ� ��ǥ ����
        if(_isDir == false) beforePos -= leftDir;
        else if (_isDir == true) beforePos -= rightDir;

        // ����� ��ġ�� -5������ �������� �� �� ����� ���� �̵�
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

        // �߸� ������ ���ӿ���
        if(isReverse[_index] != _isDir)
        {
            isFail = true;
            return;
        }

        // �����ϸ� ���ھ�� ������ ����
        Score++;
        Gauge.fillAmount += GaugeRate + 0.02f;

        // ���ȭ�� �̵�
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
            // ���ھ ���� ������ ���ҷ� ����
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
        // �������� ������ ���� ����
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
        // ������ ����
        CancelInvoke();
        isGaugeStart = false;

        playerAnimator.SetBool("Move", false);
        // ĳ���� �ִϸ��̼� 
        playerAnimator.SetBool("Die", true);
    }

}
