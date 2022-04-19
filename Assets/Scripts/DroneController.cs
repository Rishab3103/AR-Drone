using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DroneController : MonoBehaviour
{
    public GameManager gameManager;
    public SpawnManager spawnManager;
    private TextMeshProUGUI score_Text;
    private int scoreCount;
    public GameObject Camera;

   

    enum DroneState
    {
        DRONE_STATE_IDLE,
        DRONE_STATE_START_TAKINGOFF,
        DRONE_STATE_TAKINGOFF,
        DRONE_STATE_MOVING_UP,
        DRONE_STATE_FLYING,
        DRONE_STATE_START_LANDING,
        DRONE_STATE_LANDING,
        DRONE_STATE_LANDED,
        DRONE_STATE_WAIT_ENGINE_STOP,

    }

    DroneState State;
    Animator Anim;
    Vector3 Speed = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 Rotation= new Vector3(0.0f, 0.0f, 0.0f);
    public float SpeedMultiplier;

    public bool isIdle()
    {
        return (State == DroneState.DRONE_STATE_IDLE);
    }

    public void isTakeoff()
    {
        State = DroneState.DRONE_STATE_START_TAKINGOFF;
    }

    public bool isFlying()
    {
        return (State == DroneState.DRONE_STATE_FLYING);
    }

    public void Land()
    {
        State = DroneState.DRONE_STATE_START_LANDING;
    }
    void Start()
    {
        Anim = GetComponent<Animator>();
        gameManager = GetComponent<GameManager>();
        score_Text = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreCount = 0;

        State = DroneState.DRONE_STATE_IDLE;
        score_Text.text = "Score: " + scoreCount;
    }

    
    public void Move(float speedX,float SpeedY, float speedZ)
    {
        Speed.x = speedX;
        Speed.z = speedZ;
        Speed.y = SpeedY;
        UpdateDrone();
        
    }

    public void DroneRotate(float RotationX, float RotationY, float RotationZ)
    {
        Rotation.x = RotationX;
        Rotation.y = RotationY;
        Rotation.z = RotationZ;
        
        UpdateDrone();


    }

    void UpdateDrone()
    {
        switch (State)
        {
            case DroneState.DRONE_STATE_IDLE:
                break;

            case DroneState.DRONE_STATE_START_TAKINGOFF:
                Anim.SetBool("TakeOff", true);
                State = DroneState.DRONE_STATE_TAKINGOFF;
                break;

            case DroneState.DRONE_STATE_TAKINGOFF:
                if (Anim.GetBool("TakeOff") == false)
                {
                    State = DroneState.DRONE_STATE_MOVING_UP;

                }
                break;
            case DroneState.DRONE_STATE_MOVING_UP:
                if(Anim.GetBool("MoveUp")==false)
                {
                    State = DroneState.DRONE_STATE_FLYING;
                }
                break;

            case DroneState.DRONE_STATE_FLYING:
                float angleZ = -30.0f * Speed.x * 60.0f * Time.deltaTime;
                float angleX = 30.0f * Speed.z * 60.0f * Time.deltaTime;
                float angleY= 30.0f * Speed.y * 60.0f * Time.deltaTime;
                Vector3 rotation = transform.localRotation.eulerAngles;
                transform.localPosition += Speed * SpeedMultiplier * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(angleX, rotation.y, angleZ);
                
                transform.Rotate(Rotation* SpeedMultiplier*Time.deltaTime);
                Camera.transform.Rotate(Rotation * SpeedMultiplier * Time.deltaTime);
                Debug.Log(Rotation);

                //gameManager.EventOnRotateXPressed();
                //gameManager.EventOnRotateYPressed();
                //gameManager.EventOnRotateZPressed();
                break;

            case DroneState.DRONE_STATE_START_LANDING:
                Anim.SetBool("MoveDown", true);
                State = DroneState.DRONE_STATE_LANDING;
                break;

            case DroneState.DRONE_STATE_LANDING:
                if(Anim.GetBool("MoveDown")== false)
                {
                    State = DroneState.DRONE_STATE_LANDED;
                }
                break;

            case DroneState.DRONE_STATE_LANDED:
                Anim.SetBool("Land", true);
                State = DroneState.DRONE_STATE_WAIT_ENGINE_STOP;
                break;

            case DroneState.DRONE_STATE_WAIT_ENGINE_STOP:
                if(Anim.GetBool("Land") == false)
                {
                    State = DroneState.DRONE_STATE_IDLE;
                }
                break;
        }
        
        
    }

   
 

   

  

}
