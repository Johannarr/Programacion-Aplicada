﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerCannon : MonoBehaviour
{
    float _barValue;
    public GameObject ProgressBar;
    const float MINX = -8f, MAXX = 8f;
    Vector3 _deltaPos, _mousePosition;

    float _speedX = 20f;
    LineRenderer _trajectory;
    float _triggerSpeed = 30f, _triggerAngle;
    public GameObject CannonBallPrefab;
    const int _trajectoryVertices = 20;
    // Start is called before the first frame update

    private void Awake()
    {
        _trajectory = GetComponent<LineRenderer>();
    }
        void Start()
    {
        _deltaPos = new Vector3();
        //_trajectory.positionCount= _trajectoryVertices;

    }

    // Update is called once per frame
    void Update()
    {
        //Position changing
        _deltaPos.y = 0;
        _deltaPos.z= 0;
        _deltaPos.x = Input.GetAxis("Horizontal") *_speedX * Time.deltaTime;
        gameObject.transform.Translate(_deltaPos);
        gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, MINX, MAXX),
        gameObject.transform.position.y,
        gameObject.transform.position.z);


        //Calculating angle:
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _deltaPos.y = _mousePosition.y - gameObject.transform.position.y;
        _deltaPos.x = _mousePosition.x - gameObject.transform.position.x;
        


        if (_deltaPos.x < 0)
            _triggerAngle = Mathf.PI / 2;
    

        else if (_deltaPos.y < 0)
            _triggerAngle=0;
        
        else 
            _triggerAngle= Mathf.Atan(_deltaPos.y / _deltaPos.x);// * Mathf.Rad2Deg;

        if (Input.GetButton("Fire1"))
        {
           _barValue = Mathf.PingPong(Time.time, 1) * 100f;
            ProgressBar.GetComponent<ProgressBar>().BarValue = Mathf.PingPong(Time.time, 1)*100f;
            

        }

        else if (Input.GetButtonUp("Fire1"))
        {
            Instantiate(CannonBallPrefab, gameObject.transform.position, 
            Quaternion.identity).GetComponent<CannonBallBehaviour>().Shoot(_triggerSpeed * (_barValue/100),
             _triggerAngle);
        }

        /*for (int i =0; i<_trajectoryVertices; i++ )
        {
            _trajectory.SetPosition(i, GetPosition((float)i/_trajectoryVertices,Mathf.Pow(_triggerSpeed, 2)*
             Mathf.Sin(_triggerAngle * 2) / Mathf.Abs(Physics.gravity.y)));
        }*/

        
        //Debug.Log((_triggerAngle)* Mathf.Rad2Deg);
        
    }

    Vector3 GetPosition( float resolutionProportion, float xMax)
    {
        float xRelative =  resolutionProportion * xMax;
        float yRelative = xRelative * Mathf.Tan(_triggerAngle) - (Mathf.Abs(Physics.gravity.y) * 
        Mathf.Pow(xRelative,2)) / (2*(_triggerSpeed * (_barValue / 100))*(_triggerSpeed* (_barValue/100))
        * Mathf.Cos(Mathf.Pow(_triggerAngle, 2)));

        return new Vector3(xRelative, yRelative);
    }
}
