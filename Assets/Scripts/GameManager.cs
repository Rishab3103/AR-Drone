using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
public class GameManager : MonoBehaviour
{
    public DroneController Drone;
    public Button _FlyButton;
    public Button _LandButton;

    public GameObject Controls;

    public ARRaycastManager RaycastManager;
    public ARPlaneManager PlaneManager;
    public GameObject drone;
   
    List<ARRaycastHit> HitResult = new List<ARRaycastHit>();
    
    public Camera arCam;
    GameObject spawnedObject;
  
    


    struct DroneAnimationControls
    {
        public bool _moving;
        public bool _interpolatingAsc;
        public bool _interpolatingDesc;
        public float _axis;
        public float _direction;
        public bool x;
        public bool y;
        public bool z;
        public float rotValue;
        public bool clock;
        public bool anticlock;
       
    }

    DroneAnimationControls movingLeft;
    DroneAnimationControls movingBack;
    DroneAnimationControls movingUp;
    DroneAnimationControls rotateX;
    DroneAnimationControls rotateY;
    DroneAnimationControls rotateZ;
    // Start is called before the first frame update
    void Start()
    {
        _FlyButton.onClick.AddListener(OnClickFlyButton);
        _LandButton.onClick.AddListener(OnClickLandButton);
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
        spawnedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        //float speedX = Input.GetAxis("Horizontal");
        //float speedZ = Input.GetAxis("Vertical");

        UpdateControls(ref movingLeft);
        UpdateControls(ref movingBack);
        UpdateControls(ref movingUp);
        UpdateControls(ref rotateX);
        UpdateControls(ref rotateY);
        UpdateControls(ref rotateZ);
        Drone.Move(movingLeft._axis * movingLeft._direction, movingUp._axis * movingUp._direction, movingBack._axis * movingBack._direction); ;
        Drone.DroneRotate(rotateX.rotValue,rotateY.rotValue,rotateZ.rotValue);


        //Debug.Log("X is " + x);
        //Debug.Log("Y is " + y);
        //Debug.Log("Z is " + z);
        if (Drone.isIdle())
        {
            UpdateAR();
            
        }

     }
     

     void UpdateControls(ref DroneAnimationControls controls)
     {
         if (controls._interpolatingAsc || controls._moving || controls._interpolatingDesc)
         {
             if (controls._interpolatingAsc)
             {
                 controls._axis += 0.5f;
             }
             if (controls._axis >= 1.0f)
             {
                 controls._axis = 1.0f;
                 controls._interpolatingAsc = false;
                 controls._interpolatingDesc = true;
             }

             if (!controls._moving)
             {
                 controls._axis -= 0.5f;
                 if (controls._axis <= 0.0f)
                 {
                     controls._axis = 0.0f;
                     controls._interpolatingDesc = false;

                 }
             }
         }
         if(controls._moving)
         {


            if (rotateY.y == true && rotateY.x == false && rotateY.z == false && rotateY.clock == true && rotateY.anticlock == false)
          {
            rotateY.rotValue = 1.0f;
            Debug.Log(rotateY.rotValue);
          }
          else if(rotateY.y == true && rotateY.x == false && rotateY.z == false && rotateY.clock==false && rotateY.anticlock==true)
            {
                rotateY.rotValue = -1.0f;
            }
          else
            {
                rotateY.rotValue = 0.0f;
            }


           
        }
    }

     
    void OnClickFlyButton()
     {
         if (Drone.isIdle())
         {
             Drone.isTakeoff();
             _FlyButton.gameObject.SetActive(false);
             Controls.SetActive(true);
             
         }
     }

     void OnClickLandButton()
     {
         if (Drone.isFlying())
         {
             Drone.Land();
             _FlyButton.gameObject.SetActive(true);
            Controls.SetActive(false);
             
         }
     }

    public void EventOnLeftButtonPressed()
    {
        movingLeft._moving = true;
        movingLeft._interpolatingAsc = true;
        movingLeft._direction = -1.0f;
       
        
     }
     public void EventOnLeftButtonReleased()
     {
         movingLeft._moving = false;
        movingLeft._interpolatingAsc = false;

    }
     public void EventOnRightButtonPressed()
     {
         movingLeft._moving = true;
         movingLeft._interpolatingAsc = true;
         movingLeft._direction = 1.0f;
        
    }
     public void EventOnRightButtonReleased()
     {
         movingLeft._moving = false;
        movingLeft._interpolatingAsc = false;
    }

     public void EventOnBackButtonPressed()
     {
         movingBack._moving = true;
         movingBack._interpolatingAsc = true;
         movingBack._direction = -1.0f;
       
        
    }

    

     public void EventOnBackButtonReleased()
     {
         movingBack._moving = false;
        movingBack._interpolatingAsc = false;
    }

     public void EventOnForwardButtonPressed()
     {
        movingBack._moving = true;
         movingBack._interpolatingAsc = true;
         movingBack._direction = 1.0f;
        
    }

     public void EventOnForwardButtonReleased()
     {
         movingBack._moving = false;
        movingBack._interpolatingAsc = false;
    }

    public void EventOnUpButtonPressed()
    {
        movingUp._moving = true;
        movingUp._interpolatingAsc = true;
        movingUp._direction = 1.0f;

    }
    public void EventOnUpButtonReleased()
    {
        movingUp._moving = false;
        movingUp._interpolatingAsc = false;
    }

    public void EventOnDownButtonPressed()
    {
        movingUp._moving = true;
        movingUp._interpolatingAsc = true;
        movingUp._direction = -1.0f;

    }
    public void EventOnDownButtonReleased()
    {
        movingUp._moving = false;
        movingUp._interpolatingAsc = false;
    }

    public void EventOnForwardLeftPressed()
    {
        movingBack._moving = true;
        movingLeft._moving = true;
        movingBack._interpolatingAsc = true;
        movingLeft._interpolatingAsc = true;
        movingBack._direction = 1.0f;
        movingLeft._direction = -1.0f;
    }

    public void EventOnForwardLeftReleased()
    {
        movingBack._moving = false;
        movingLeft._moving = false;
        movingBack._interpolatingAsc = false;
        movingLeft._interpolatingAsc = false;
        
    }

    public void EventOnForwardRightPressed()
    {
        movingBack._moving = true;
        movingLeft._moving = true;
        movingBack._interpolatingAsc = true;
        movingLeft._interpolatingAsc = true;
        movingBack._direction = 1.0f;
        movingLeft._direction = 1.0f;
    }

    public void EventOnForwardRightReleased()
    {
        movingBack._moving = false;
        movingLeft._moving = false;
        movingBack._interpolatingAsc = false;
        movingLeft._interpolatingAsc = false;

    }

    public void EventOnBackLeftPressed()
    {
        movingBack._moving = true;
        movingLeft._moving = true;
        movingBack._interpolatingAsc = true;
        movingLeft._interpolatingAsc = true;
        movingBack._direction = -1.0f;
        movingLeft._direction = -1.0f;
    }

    public void EventOnBackLeftReleased()
    {
        movingBack._moving = false;
        movingLeft._moving = false;
        movingBack._interpolatingAsc = false;
        movingLeft._interpolatingAsc = false;

    }

    public void EventOnBackRightPressed()
    {
        movingBack._moving = true;
        movingLeft._moving = true;
        movingBack._interpolatingAsc = true;
        movingLeft._interpolatingAsc = true;
        movingBack._direction = -1.0f;
        movingLeft._direction = 1.0f;
    }

    public void EventOnBackRightReleased()
    {
        movingBack._moving = false;
        movingLeft._moving = false;
        movingBack._interpolatingAsc = false;
        movingLeft._interpolatingAsc = false;

    }

    public void EventOnUpDiagonalPressed()
    {
        movingUp._moving = true;
        movingBack._moving = true;
        movingUp._interpolatingAsc = true;
        movingBack._interpolatingAsc = true;
        movingUp._direction = 1.0f;
        movingBack._direction = 1.0f;
    }

    public void EventOnUpDiagonalReleased()
    {
        movingUp._moving = false;
        movingBack._moving = false;
        movingUp._interpolatingAsc = false;
        movingBack._interpolatingAsc = false;

    }
    public void EventOnDownDiagonalPressed()
    {
        movingUp._moving = true;
        movingBack._moving = true;
        movingUp._interpolatingAsc = true;
        movingBack._interpolatingAsc = true;
        movingUp._direction = -1.0f;
        movingBack._direction = -1.0f;
    }

    public void EventOnDownDiagonalReleased()
    {
        movingUp._moving = false;
        movingBack._moving = false;
        movingUp._interpolatingAsc = false;
        movingBack._interpolatingAsc = false;

    }

    public void EventOnRotateXPressed()
    {
        rotateX.x = true;
        rotateX.y = false;
        rotateX.z = false;
        rotateX._moving = true;
        

        Debug.Log("X is" + rotateX.x);
        Debug.Log("Y is" + rotateX.y);
        Debug.Log("Z is" + rotateX.z);



    }

    public void EventOnRotateXReleased()
    {
        rotateX.x = false;
        //rotateX._moving = false;

    }

    public void EventOnRotateYClockPressed()
    {
        rotateY.y = true;
        rotateY.x = false;
        rotateY.z = false;
        rotateY._moving = true;
        rotateY.clock = true;
    }

    public void EventOnRotateYClockReleased()
    {
        rotateY.y = false;
        //rotateY._moving = false;
        rotateY.clock = false;
    }

    public void EventOnRotateYAntiClockPressed()
    {
        rotateY.y = true;
        rotateY.x = false;
        rotateY.z = false;
        rotateY._moving = true;
        rotateY.anticlock = true;

        
    }

    public void EventOnRotateYAntiClockReleased()
    {
        rotateY.y = false;
        //rotateY._moving = false;
        rotateY.anticlock = false;
        
    }


    public void EventOnRotateZPressed()
    {
        rotateZ.z = true;
        rotateZ.x = false;
        rotateZ.y = false;
        rotateZ._moving = true;
    }

    public void EventOnRotateZReleased()
    {
        rotateZ.z = false;
        //rotateZ._moving = false;
    }

    public void UpdateAR()
    {
        Vector2 positionScreenSpace = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        RaycastManager.Raycast(positionScreenSpace, HitResult, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds);

        if (HitResult.Count > 0)
        {
            if (PlaneManager.GetPlane(HitResult[0].trackableId).alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                Pose pose = HitResult[0].pose;
                drone.transform.position = pose.position;
                drone.SetActive(true);
                
            }
        }
    }




    
}
