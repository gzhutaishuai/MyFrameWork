using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
       
    }


    private void OnDestroy()
    {
        if(GameManager.isExisted)
        {
            GameManager.Instance.Show();
        }
    }
}
