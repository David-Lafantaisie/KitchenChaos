//------------------------------------------------------------//
//---------------------- KITCHEN CHAOS -----------------------//
//-------------------- PLAYER CONTROLLER ---------------------//
//------------------- By David Lafantaisie -------------------//
//------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    //-------------------------------------------------------//
    //---------------------- VARIABLES ----------------------//
    //-------------------------------------------------------//

    [SerializeField] private float grabRange = 10.0f;
    [SerializeField] private float moveObjectSpeed = 1.0f;
    [SerializeField] private float rotObjSpeed = 20.0f;
    [SerializeField] private GameObject controllerRight;
    [SerializeField] private GameObject controllerLeft;
    private GameObject activeController;
    private GameObject currHeldObj = null;
    private LineRenderer lineReticle;
    private LayerMask interactable;

    //Input variables
    private Vector2 touchPosition;
    private Vector2 clickPosition;
    private bool triggerPressed = false;
    private bool touchClicked = false;
    private bool moveObjCloser = false;
    private bool moveObjFarther = false;
    private bool rotateObjRight = false;
    private bool rotateObjLeft = false;
    private bool rotateObjBack = false;
    private bool rotateObjFwd = false;
    private bool objHeld = false;

    // Use this for initialization
    void Start () {
        initControllerHand();
        initLineReticle();
        initLayerMasks();
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
        if(currHeldObj != null)
        {
            if (moveObjFarther)//Moves object farther away from controller, stopping at max reach
            {
                currHeldObj.transform.position = 
                    Vector3.MoveTowards(currHeldObj.transform.position, 
                    activeController.transform.forward * grabRange + activeController.transform.position, 
                    moveObjectSpeed * Time.deltaTime);
            }
            else if (moveObjCloser)//Moves object closer to controller, stopping at controller position
            {
                currHeldObj.transform.position = 
                    Vector3.MoveTowards(currHeldObj.transform.position, 
                    activeController.transform.position, 
                    moveObjectSpeed*Time.deltaTime);
            }

            if (rotateObjLeft)
                rotLeft();
            else if (rotateObjRight)
                rotRight();
            else if (rotateObjBack)
                rotBackward();
            else if (rotateObjFwd)
                rotForward();
        }
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
                currHeldObj = null;
            }
        }
    }


    //----------------------------------------------------------//
    //---------------------- TRANSFORMERS ----------------------//
    //----------------------------------------------------------//

    //Use pythagoreans theorm to determine a Vector3 point directly 90 degrees to the side of the object and use that as the axis
    //Both adjacent side and opposite side will be the same length
    void rotForward()
    {
        float a = Vector3.Distance(activeController.transform.position, currHeldObj.transform.position);//Calculate side, both opp & adj are the same
        float c = Mathf.Sqrt((a * a) * 2);// Calculate hypoteneuse   c*c = a*a + b*b   or in our case,    c*c = (a*a)*2
        Vector3 rightSpot = activeController.transform.position + new Vector3(c, 0.0f, c);//x is right, z is forward, add our 2d triangle to our vec3
        Vector3 rotAxis = rightSpot - currHeldObj.transform.position;//Actually calculates the new axis
        currHeldObj.transform.Rotate(-rotAxis * Time.deltaTime * 10);//Rotates the object
    }

    void rotBackward()
    {
        float a = Vector3.Distance(activeController.transform.position, currHeldObj.transform.position);
        float c = Mathf.Sqrt((a * a) * 2);
        Vector3 rightSpot = activeController.transform.position + new Vector3(c, 0.0f, c);
        Vector3 rotAxis = rightSpot - currHeldObj.transform.position;
        currHeldObj.transform.Rotate(rotAxis * Time.deltaTime * rotObjSpeed);
    }

    void rotLeft()
    {
        Vector3 rotAxis = activeController.transform.position - currHeldObj.transform.position;
        currHeldObj.transform.Rotate(rotAxis * Time.deltaTime * rotObjSpeed);
    }

    void rotRight()
    {
        Vector3 rotAxis = activeController.transform.position - currHeldObj.transform.position;
        currHeldObj.transform.Rotate(-rotAxis * Time.deltaTime * rotObjSpeed);
    }


    //-----------------------------------------------------//
    //---------------------- GETTERS ----------------------//
    //-----------------------------------------------------//

    //Function for recieving and storing input for use later on
    void getInput()
    {
        OVRInput.Update();
        triggerPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        touchClicked = OVRInput.Get(OVRInput.Button.PrimaryTouchpad);
        touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        moveObjFarther = touchPosition.y > 0.5 && touchPosition.x > -0.5 && touchPosition.x < 0.5;
        moveObjCloser = touchPosition.y < -0.5 && touchPosition.x > -0.5 && touchPosition.x < 0.5;
        rotateObjLeft = touchPosition.x < -0.5 && touchPosition.y < 0.5 && touchPosition.y > -0.5;
        rotateObjRight = touchPosition.x > 0.5 && touchPosition.y < 0.5 && touchPosition.y > -0.5;
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
