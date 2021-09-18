using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Target : NetworkBehaviour
{
    
    public Vector3 Range;
    public float speed,errorMargin;
    Vector3 startPos, TargetPos;
    // Start is called before the first frame update
    void Start()
    {
        if (!isServer) enabled = false;
        startPos = transform.position;
        TargetPos = startPos + Range;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        transform.position -= (transform.position - TargetPos).normalized * speed*Time.fixedDeltaTime;
        if(Vector3.Distance( transform.position, TargetPos)<errorMargin)
        {
            if (Vector3.Distance(transform.position, startPos) < errorMargin) TargetPos = startPos + Range;
            else TargetPos = startPos;
        }
    }


}
