﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviour
{
    public void OnLoadTitleScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
