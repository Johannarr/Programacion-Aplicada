﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("BananaWorld");
    }
}
