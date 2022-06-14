using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float lerpTime;
    public float xOffset;
    private bool canLerp;
    private float lerpXDist;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canLerp)
        {
            MoveLerp();
        }    
    }

    private void MoveLerp()
    {
        float xPos = transform.position.x;
        xPos = Mathf.Lerp(xPos, lerpXDist, lerpTime * Time.deltaTime);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        if (transform.position.x >= (lerpXDist - xOffset))
        {
            canLerp = false;
        }
    }


    public void lerpTrigger(float dist)
    {
        canLerp = true;
        lerpXDist = dist;
    }
}
