//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//-------------------- PLAYER CONTROLLER ---------------------//
//------------------- By David Lafantaisie -------------------// ------>Add your name to this list if you contribute
//------------------ for PROMACHOS STUDIOS -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    //-------------------------------------------------------//
    //---------------------- VARIABLES ----------------------//
    //-------------------------------------------------------//

    enum Axis { ROLL, PITCH, YAW };

    [SerializeField] private GameObject ovrRig;
    [SerializeField] private GameObject hmd;
    [SerializeField] private GameObject maxPosRight;
    [SerializeField] private GameObject maxPosLeft;
    [SerializeField] private float grabRange = 10.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float moveObjectSpeed = 1.0f;
    [SerializeField] private float rotObjSpeed = 20.0f;
    [SerializeField] private GameObject controllerRight;
    [SerializeField] private GameObject controllerLeft;
    private GameObject activeController;
    private GameObject currHeldObj = null;
    private LineRenderer lineReticle;
    private LayerMask interactable;
    private Vector3 originalForward;
    private bool facingForward = true;

    //Input variables
    private Vector2 touchPosition;
    private Vector2 clickPosition;
    private bool triggerPressed = false;
    private bool touchClicked = false;
    private bool touchLeft = false;
    private bool touchRight = false;
    private bool touchUp = false;
    private bool touchDown = false;
    private bool objHeld = false;

    // Use this for initialization
    void Start () {
        initControllerHand();
        initLineReticle();
        initLayerMasks();
        originalForward = ovrRig.transform.TransformDirection(Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
        setLineRenderer();
        getInput();
    }

    private void FixedUpdate()
    {
        handleInput();
    }


    //------------------------------------------------------------//
    //---------------------- INPUT HANDLERS ----------------------//
    //------------------------------------------------------------//

    //Handles input, must get input first!
    void handleInput()
    {
        handleTrigger();
        handleTouch();
    }

    //Handles any touch related input
    void handleTouch()
    {
        //If player is currently holding an object they will be able to move it forward or backwards
        if(objHeld)
        {
            //Moving held object closer or farther
            if (touchUp)//Moves object farther away from controller, stopping at max reach
            {
                currHeldObj.transform.position = 
                    Vector3.MoveTowards(currHeldObj.transform.position, 
                    activeController.transform.forward * grabRange + activeController.transform.position, 
                    moveObjectSpeed * Time.deltaTime);
            }
            else if (touchDown)//Moves object closer to controller, stopping at controller position
            {
                currHeldObj.transform.position = 
                    Vector3.MoveTowards(currHeldObj.transform.position, 
                    activeController.transform.position,
                    moveObjectSpeed*Time.deltaTime);
            }
        }

        //Checks if player is facing forward
        if (Vector3.Angle(originalForward, hmd.transform.TransformDirection(Vector3.forward)) > 90.0f)
            facingForward = false;
        else
            facingForward = true;

        //Moves towards direction pressed down
        if (touchLeft && facingForward == true)
            ovrRig.transform.position = Vector3.MoveTowards(ovrRig.transform.position, maxPosLeft.transform.position, moveSpeed * Time.deltaTime);
        else if (touchRight && facingForward == true)
            ovrRig.transform.position = Vector3.MoveTowards(ovrRig.transform.position, maxPosRight.transform.position, moveSpeed * Time.deltaTime);
        else if (touchLeft && facingForward == false)
            ovrRig.transform.position = Vector3.MoveTowards(ovrRig.transform.position, maxPosRight.transform.position, moveSpeed * Time.deltaTime);
        else if (touchRight && facingForward == false)
            ovrRig.transform.position = Vector3.MoveTowards(ovrRig.transform.position, maxPosLeft.transform.position, moveSpeed * Time.deltaTime);
    }

    //Handling input for rear trigger on controller
    void handleTrigger()
    {
        Ray ray = new Ray(activeController.transform.position, activeController.transform.forward);
        RaycastHit hit;

        if (triggerPressed == true)
        {
            //Grabbing the object if pointed at by controller and trigger pressed
            if (Physics.Raycast(ray, out hit, grabRange, interactable) && objHeld == false)
            {
                objHeld = true;
                currHeldObj = hit.transform.gameObject;
                currHeldObj.transform.parent = activeController.transform;
                currHeldObj.GetComponent<Rigidbody>().useGravity = false;
                currHeldObj.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if (triggerPressed == false)
        {
            objHeld = false;
            //If holding an object, it will be released
            if (currHeldObj != null)
            {
                currHeldObj.transform.parent = null;
                currHeldObj.GetComponent<Rigidbody>().isKinematic = false;
                currHeldObj.GetComponent<Rigidbody>().useGravity = true;
                if (currHeldObj.tag == "Ingredient")
                {
                    currHeldObj.GetComponent<IngredientScript>().checkAttach();
                }
                currHeldObj = null;
            }
        }
    }


    //----------------------------------------------------------//
    //---------------------- TRANSFORMERS ----------------------//
    //----------------------------------------------------------//

    void rotateObject(Axis axis, bool positive)
    {
        float a = Vector3.Distance(activeController.transform.position, currHeldObj.transform.position);
        float c = Mathf.Sqrt((a * a) * 2);
        Vector3 thirdPoint = new Vector3(0.0f, 0.0f, 0.0f), rotAxis;
        switch(axis)
        {
            case Axis.ROLL:
                rotAxis = activeController.transform.position - currHeldObj.transform.position;
                break;
            case Axis.PITCH:
                thirdPoint = activeController.transform.position + new Vector3(c, 0.0f, c);
                rotAxis = thirdPoint - currHeldObj.transform.position;
                break;
            case Axis.YAW:
                thirdPoint = activeController.transform.position + new Vector3(0.0f, c, c);
                rotAxis = thirdPoint - currHeldObj.transform.position;
                break;
            default:
                rotAxis = activeController.transform.position - currHeldObj.transform.position;
                break;
        }
        if (positive == false)
            rotAxis = -rotAxis;
        currHeldObj.transform.Rotate(rotAxis * Time.deltaTime * rotObjSpeed);
    }


    //-----------------------------------------------------//
    //---------------------- GETTERS ----------------------//
    //-----------------------------------------------------//

    //Function for recieving and storing input for use later on
    void getInput()
    {
        OVRInput.Update();//need to call this first to get input data
        //Trigger
        triggerPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    triggerPressed = !triggerPressed;
        //}

        //Touchpad click
        touchClicked = OVRInput.Get(OVRInput.Button.PrimaryTouchpad);

        //Touchpad
        touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        touchUp = touchPosition.x < 0.5 && touchPosition.x > -0.5 && touchPosition.y > 0.1;
        touchDown = touchPosition.x < 0.5 && touchPosition.x > -0.5 && touchPosition.y < -0.1;
        //touchLeft = Input.GetKey(KeyCode.A);
        //touchRight = Input.GetKey(KeyCode.D);
        touchLeft = touchPosition.y < 0.5 && touchPosition.y > -0.5 && touchPosition.x < -0.5;
        touchRight = touchPosition.y < 0.5 && touchPosition.y > -0.5 && touchPosition.x > 0.5;
    }


    //-----------------------------------------------------//
    //---------------------- SETTERS ----------------------//
    //-----------------------------------------------------//
    
    //Updates the positions of the line reticle
    void setLineRenderer()
    {
        lineReticle.SetPosition(0, activeController.transform.position);
        lineReticle.SetPosition(1, activeController.transform.forward * grabRange + activeController.transform.position);
    }


    //----------------------------------------------------------//
    //---------------------- INITIALIZERS ----------------------//
    //----------------------------------------------------------//

    //Intializes the line reticle
    void initLineReticle()
    {
        lineReticle = gameObject.GetComponent<LineRenderer>();
        lineReticle.useWorldSpace = true;
    }

    //Finds which hand is using the controller based on oculus settings
    void initControllerHand()
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            activeController = controllerLeft;
        }
        else
        {
            activeController = controllerRight;
        }
    }

    //Finds layer masks that will be used in this code
    void initLayerMasks()
    {
        interactable = LayerMask.GetMask("Interactable");
    }
}
