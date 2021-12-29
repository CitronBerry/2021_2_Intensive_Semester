using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Header("Settings")]

    [SerializeField]
    private Vector2 sensitivity = new Vector2(1, 1);
    [Tooltip("Y����")]
    [SerializeField]
    private Vector2 yClamp = new Vector2(-90, 90);
    [SerializeField]
    private float RotationSpeed;

    #endregion

    #region FIELDS

    public GameObject player;
    float x;
    float y;

    GameObject pistol;
    GameObject ar;
    bool isEmpty;

    #endregion

    void Start()
    {

    }

    
    void Update()
    {
        Vector2 frameInput;
        frameInput.x = Input.GetAxis("Mouse X");
        frameInput.y = Input.GetAxis("Mouse Y");

        x += frameInput.x * RotationSpeed * Time.deltaTime;
        y += frameInput.y * RotationSpeed * Time.deltaTime;

        // �ݵ�
        if (Input.GetButton("Fire1"))
        {
            // �Ѿ� ������ �ݵ� ���� �ϱ�
            x += Random.Range(-0.1f, 0.1f);
            y += Random.Range(0.1f, 0.2f);
        }

        // y���� üũ
        y = Mathf.Clamp(y, yClamp.x, yClamp.y);

        transform.eulerAngles = new Vector3(-y, x, 0);
    }

}