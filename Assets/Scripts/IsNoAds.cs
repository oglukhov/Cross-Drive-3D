using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Awake(){
        if(PlayerPrefs.GetString("NoAds")=="Yes")
        {
            Destroy(gameObject);
        }
    }
}
