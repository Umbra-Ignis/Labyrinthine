﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{


    public void GameScene(int nIndexScene)
    {
        SceneManager.LoadScene(nIndexScene);
    }

    public void Awake()
    {
        
    }

    public void Update()
    {
        
    }
}
