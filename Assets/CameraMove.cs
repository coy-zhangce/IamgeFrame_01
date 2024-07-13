using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float camFocus = 5;

    public float turntime = 0;

    public Vector3 fromPos = Vector3.zero;
    public Vector3 toPos = Vector3.zero;

    public GameObject target = null;
    public GameObject cameraObj = null;

    public List<Vector3> cameraPos = new List<Vector3>();
    public int cameraIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        cameraPos.Add(fromPos);
        cameraPos.Add(toPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraIndex == -1)
        {
            UpdateMoving();
        }
        else
        {
            UpdatePos(cameraIndex);
        }
    }
    void UpdatePos(int cameraIndex)
    {
        turntime += Time.deltaTime;
        transform.position = cameraPos[cameraIndex];
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
    }

    void UpdateMoving()
    {
        turntime += Time.deltaTime;
        transform.position = Vector3.Lerp(fromPos, toPos, 0.5f * (Mathf.Sin(turntime) + 1));
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(20, 100, 100, 25), string.Format("CI:{0}", cameraIndex)))
        {
            cameraIndex++;
            if (cameraIndex == cameraPos.Count)
                cameraIndex = -1;
        }
    }
}
