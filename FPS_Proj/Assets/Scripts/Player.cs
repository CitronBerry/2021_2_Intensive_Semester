using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region FIELDS

    public float MoveSpeed = 5f;
    public float gravity = -20f;
    public float JumpPower = 5;
    float rotationY = 0.0f;
    float YelocityY;
    float mx;
    float my;
    bool isFloor = true;
    CharacterController pp;

    #endregion

    void Start()
    {
        pp = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 PlayerInput;
        PlayerInput.x = Input.GetAxisRaw("Horizontal");
        PlayerInput.y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(PlayerInput.x, 0, PlayerInput.y);
        dir.Normalize();

        // 카메라가 보는 위치
        dir = Camera.main.transform.TransformDirection(dir);

        if (Input.GetButtonDown("Jump"))
        {
            // 2단 점프 막음
            if(pp.isGrounded)
            {
                YelocityY = JumpPower;
            }
        }

        YelocityY += gravity * Time.deltaTime;
        dir.y = YelocityY;

        pp.Move(dir * MoveSpeed * Time.deltaTime);
    }

}
