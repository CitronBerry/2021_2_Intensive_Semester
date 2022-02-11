using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    #region FIELDS

    public ParticleSystem Particle;
    AudioSource Audio;
    public AudioClip FireSound;
    public AudioClip EmptyFireSound;
    public AudioClip ReloadSound;
    public AudioClip EmptyReloadSound;

    Animator anim;
    bool isReloading;               // ������
    public bool isEmpty;            // �Ѿ� �� ������ Ȯ��

    public int allBullet;           // ��ü ź ��
    public int availableBullet;     // ��� ������ ź ��
    public int currBullet;          // ���� ź ��
    public float fireRate;          // ���� �߻� �ð�
    float fireTimer;

    public float reloadRate;

    public Text BulletCount;        // ź �� ǥ���� �ؽ�Ʈ

    public GameObject HitHole;      // ź ����
    public GameObject HitSplash;    // ź ������ Ƣ��� ��

    #endregion

    void Start()
    {
        currBullet = availableBullet;
        Audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        isReloading = false;

        BulletCount.text = currBullet + " / " + allBullet;
    }

    
    void Update()
    {

        if (isReloading == true)
        {
            // ���ε� ��������
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload")
                && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= reloadRate)
            {
                ReloadText();
                isReloading = false;
            }
            else
            {
                return;
            }
        }

        if (currBullet == 0)
        {
            anim.CrossFadeInFixedTime("Reload Empty", 0.01f);
            isEmpty = true;
        }

        // ���콺 ���� Ŭ��
        if (Input.GetButton("Fire1"))
        {
            Shot();
        }

        // ������
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        BulletCount.text = currBullet + " / " + allBullet;

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

    }

    void Shot()
    {
        if(fireTimer < fireRate)
        {
            return;
        }

        if (currBullet > 0)
        {
            isEmpty = false;
            Audio.clip = FireSound;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                GameObject Splash = Instantiate(HitSplash, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(Splash, 0.5f);
                GameObject Hole = Instantiate(HitHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(Hole, 5f);
            }
            anim.CrossFadeInFixedTime("Fire", 0.01f);
            // �ݹ� ��
            Particle.gameObject.transform.rotation = Camera.main.transform.rotation;
            Particle.Play();
            // �ݹ� ����
            Audio.Play();

            // �Ѿ� ���ҿ� ���� �͵�
            currBullet--;
            fireTimer = 0f;
        }
        else
        {
            Audio.clip = EmptyFireSound;

            Audio.Play();
            fireTimer = 0f;
        }
        
    }

    void Reload()
    {
        // ���� �Ѿ� ������ 7�϶��� ��ü �Ѿ� ������ 0�϶��� �ȵǰ�
        if(!isReloading && currBullet < availableBullet && allBullet > 0)
        {
            isReloading = true;
            anim.CrossFadeInFixedTime("Reload", 0.01f);
            if(currBullet == 0)
            {
                Audio.clip = EmptyReloadSound;
            }
            else
            {
                Audio.clip = ReloadSound;
            }
            Audio.Play();
        }
    }

    public void ReloadText()
    {
        int temp = availableBullet - currBullet;
        if(temp > allBullet)
        {
            temp = allBullet;
        }
        currBullet += temp;
        allBullet -= temp;
        BulletCount.text = currBullet + " / " + allBullet;
    }
}
