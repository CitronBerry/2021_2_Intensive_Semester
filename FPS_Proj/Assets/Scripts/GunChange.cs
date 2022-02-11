using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChange : MonoBehaviour
{
    #region FIELDS

    public GameObject pistol;
    public GameObject ar;

    #endregion

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(gameObject.name == "Pistol Stand")
            {
                pistol.SetActive(true);
                ar.SetActive(false);
            }
            if (gameObject.name == "AR Stand")
            {
                pistol.SetActive(false);
                ar.SetActive(true);
            }
        }
    }

    void Start()
    {

    }

   
    void Update()
    {
        
    }
}
