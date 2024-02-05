using Core;
using Core.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> 
{
    private void Awake()
    {

        
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Init();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

    }

    public void Show()
    {
        Debug.Log("Show");
    }
}
