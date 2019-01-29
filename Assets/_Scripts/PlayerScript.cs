using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    [SerializeField] private float grabRange = 10.0f;
    [SerializeField] private GameObject controllerRight;
    [SerializeField] private GameObject controllerLeft;
    private GameObject activeController;
    private LineRenderer lineReticle;
    private LayerMask interactable;
    private bool triggerPressed = false;
    private bool objHeld = false;
    private GameObject currHeldObj = null;

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

    void handleInput()
    {
        Ray ray = new Ray(activeController.transform.position, activeController.transform.forward);
        RaycastHit hit;

        if(triggerPressed == true)
        {
            if (Physics.Raycast(ray, out hit, grabRange, interactable) && objHeld == false)
            {
                objHeld = true;
                currHeldObj = hit.transform.gameObject;
                currHeldObj.transform.parent = activeController.transform;
                currHeldObj.GetComponent<Rigidbody>().useGravity = false;
                currHeldObj.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if(triggerPressed == false)
        {
            objHeld = false;
            if (currHeldObj != null)
            {
                currHeldObj.transform.parent = null;
                currHeldObj.GetComponent<Rigidbody>().isKinematic = false;
                currHeldObj.GetComponent<Rigidbody>().useGravity = true;
                currHeldObj = null;
            }
        }
    }

    void getInput()
    {
        OVRInput.Update();
        triggerPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
    }

    void setLineRenderer()
    {
        lineReticle.SetPosition(0, activeController.transform.position);
        lineReticle.SetPosition(1, activeController.transform.forward * grabRange + activeController.transform.position);
    }

    void initLineReticle()
    {
        lineReticle = gameObject.GetComponent<LineRenderer>();
        lineReticle.useWorldSpace = true;
    }

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

    void initLayerMasks()
    {
        interactable = LayerMask.GetMask("Interactable");
    }
}
