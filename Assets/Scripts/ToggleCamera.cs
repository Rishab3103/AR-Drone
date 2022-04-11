using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam1;
    public Camera cam2;
    void Start()
    {
        //cam1 = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCam(int x)
    {
        DeactivateAll();
        if (x == 1)
        {
            cam1.enabled = true;
        }
        else if(x == 2)
        {
            cam2.enabled = true;
        }
    }

    public void DeactivateAll()
    {
        cam1.enabled = false;
        cam2.enabled = false;


    }
}
