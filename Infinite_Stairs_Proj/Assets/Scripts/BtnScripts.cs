using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnScripts : MonoBehaviour
{
    public void CharacterChangeScene()
    {
        SceneManager.LoadScene("Character");
    }
    public void PlayChangeScene()
    {
        SceneManager.LoadScene("Play");
    }

    //public Play play;
    //public void Climb()
    //{
    //    play.StairsMove(play.isDirection);
    //}

    //public void Reverse()
    //{
    //    if (play.isDirection == false) play.isDirection = true;
    //    else if (play.isDirection == true) play.isDirection = false;
    //    Climb();
    //}
}
