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
<<<<<<< HEAD
    bool isReloading;               // 장전중
    public bool isEmpty;            // 총알 수 없는지 확인
=======
    bool isReloading;
>>>>>>> 1c241d4cf4a07f1c2afc7f54a96674cf5b078f36

    public int allBullet;           // 전체 탄 수
    public int availableBullet;     // 사용 가능한 탄 수
    public int currBullet;          // 현재 탄 수
    public float fireRate;          // 연속 발사 시간
    float fireTimer;

    public float reloadRate;

    public Text BulletCount;        // 탄 수 표시할 텍스트

    public GameObject HitHole;      // 탄 구멍
    public GameObject HitSplash;    // 탄 맞으면 튀기는 것
<<<<<<< HEAD

=======
>>>>>>> 1c241d4cf4a07f1c2afc7f54a96674cf5b078f36
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
<<<<<<< HEAD

        if (isReloading == true)
        {
            // 리로딩 끝났을때
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload")
                && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= reloadRate)
=======
        if(isReloading == true)
        {
            // 리로딩 끝났을때
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= reloadRate)
>>>>>>> 1c241d4cf4a07f1c2afc7f54a96674cf5b078f36
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
<<<<<<< HEAD
            isEmpty = true;
=======
>>>>>>> 1c241d4cf4a07f1c2afc7f54a96674cf5b078f36
        }

        // 마우스 왼쪽 클릭
        if (Input.GetButton("Fire1"))
        {
            Shot();
        }

        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

<<<<<<< HEAD
        BulletCount.text = currBullet + " / " + allBullet;

=======
>>>>>>> 1c241d4cf4a07f1c2afc7f54a96674cf5b078f36
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
<<<<<<< HEAD
            isEmpty = false;
=======
>>>>>>> 1c241d4cf4a07f1c2afc7f54a96674cf5b078f36
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
            // 격발 빛
            Particle.gameObject.transform.rotation = Camera.main.transform.rotation;
            Particle.Play();
            // 격발 사운드
            Audio.Play();

<<<<<<< HEAD
            // 총알 감소와 여러 것들
            currBullet--;
            fireTimer = 0f;
=======
            currBullet--;
            fireTimer = 0f;
            BulletCount.text = currBullet + " / " + allBullet;
>>>>>>> 1c241d4cf4a07f1c2afc7f54a96674cf5b078f36
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
        // 현재 총알 개수가 7일때와 전체 총알 개수가 0일때는 안되게
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
