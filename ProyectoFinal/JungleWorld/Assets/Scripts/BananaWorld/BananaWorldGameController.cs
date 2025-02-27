﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaWorldGameController : MonoBehaviour
{
    
    public int CurrentScore;
    public int CurrentLives;
    public TextMesh ScoreText;
    public GameObject GameOverText;
    public GameObject GoBackText;
    public GameObject RetryText;
    public GameObject WinText;
    public TextMesh LivesText;
    public GameObject BananaPrefab;
    const float MINX = -7.8f, MAXX = 7.8f;
    // Start is called before the first frame update
    void Start()
    {
        
        BananaWorldAudioManager.Instance.PlaySoundEffect(BananaWorldAudioManager.SoundEffect.Start);
        CurrentScore = 0;
        CurrentLives = 3;
        LivesText = GameObject.Find("LivesText").GetComponent<TextMesh>();
        GameOverText = GameObject.Find("GameOverText");
        WinText = GameObject.Find("WinText");
        RetryText = GameObject.Find("RetryText");
        GoBackText = GameObject.Find("GoBackText");


        InvokeRepeating("InstantiateBanana", 0, 1.5f);
        GameOverText.SetActive(false);
        WinText.SetActive(false);
        RetryText.SetActive(false);
        GoBackText.SetActive(false);


    }

    // Update is called once per frame
    void InstantiateBanana()
    {
       if (CurrentLives <= 0)
        {
            GameOverText.SetActive(true);
            RetryText.SetActive(true);
            GoBackText.SetActive(true);
            BananaWorldAudioManager.Instance.PlaySoundEffect(BananaWorldAudioManager.SoundEffect.GameOver);
            return;
        }  
        else if (CurrentScore==5)
        {
            WinText.SetActive(true);
            RetryText.SetActive(true);
            GoBackText.SetActive(true);
            BananaWorldAudioManager.Instance.PlaySoundEffect(BananaWorldAudioManager.SoundEffect.Win);
            return;
        }    
        Instantiate(BananaPrefab, new Vector3 (Random.Range (MINX, MAXX),10,0), Quaternion.identity);

    }

    public int IncrementScore()
    {    

       CurrentScore = CurrentScore + 1;
       ScoreText.text = CurrentScore.ToString();
        return CurrentScore;
    }

    public int DecrementLives()
    {
       CurrentLives = CurrentLives > 0 ? CurrentLives - 1 : 0;
        LivesText.text = $"{CurrentLives}"; 

        if (CurrentLives == 0)
        {
            StartCoroutine("SendScore");
            GameOverText.SetActive(true);
        }

        return CurrentLives;
    }

    IEnumerator SendScore()
    {
        yield return gameObject.GetComponent<BananaWorldWebServiceClient>().SendWebRequest(CurrentScore);
    }
}
