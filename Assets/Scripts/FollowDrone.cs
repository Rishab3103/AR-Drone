using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDrone : MonoBehaviour
{
    public GameObject drone;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = drone.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = drone.transform.position + new Vector3(0,0.7f,0);
        
    }
}
