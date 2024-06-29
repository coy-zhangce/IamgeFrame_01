using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float camFocus = 5;

    public float turntime = 0;

    public Vector3 fromPos = Vector3.zero;
    public Vector3 toPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        turntime += Time.deltaTime;
        transform.position = Vector3.Lerp(fromPos, toPos, 0.5f * (Mathf.Sin(turntime) + 1));
        
    }
}
