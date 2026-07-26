using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    Camera cam;
    InputAction interact;
    public InputAction toss;


    public Transform carryPoint;
    public float interactDistance;
    public ICarrayable carrying;
    public bool bIsCarryingObject = false;

    [SerializeField] public float tossForce = 20f;

    private void OnEnable()
    {
        cam = Camera.main;
        interact = InputSystem.actions.FindAction("Interact");
        toss = InputSystem.actions.FindAction("Toss");
    }

    void Update()
    {
        if (interact.WasPressedThisFrame())
        {
            Debug.Log("Try interacting");
            RaycastHit hit;
            bool hitSomething = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance, ~0, QueryTriggerInteraction.Ignore);

            if (!hitSomething)
            {
                return;
            }

            var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            var carrayable = hit.collider.gameObject.GetComponent<ICarrayable>();

            Debug.Log(interactable);
            if (interactable != null)
            { 
                interactable.Interact();
            }
            else if (carrayable != null && carrying == null)
            {
                bIsCarryingObject = true;
                carrying = carrayable;
                GameObject carryObj = carrying.Pickup();

                carryObj.transform.parent = carryPoint;
                carryObj.transform.SetPositionAndRotation(carryPoint.position, carryPoint.rotation);
                
            }
            else
            {
                if (carrying != null)
                {
                   
                    GameObject carryObj = carrying.PutDown();

                    carryObj.transform.parent = null;
                    
                    carryObj.transform.SetPositionAndRotation(hit.point + hit.normal * 0.6f, transform.rotation);
                    carrying = null; 
                    bIsCarryingObject = false;
                }
            }
        }

        TossObject();

    }


    void TossObject()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && bIsCarryingObject)
        {
            GameObject carryObj = carrying.PutDown();
            carryObj.transform.parent = null;
            carrying = null;
            Debug.Log("Toss Pressed!!!!!!!");
            bIsCarryingObject = false;

            var carryObjRigidBody = carryObj.GetComponent<Rigidbody>();
            carryObjRigidBody.AddForce(cam.transform.forward* 30.0f, ForceMode.Impulse);
        }
    }

    void PickUpItem()
    {

    }

    void PutDownItem()
    {

    }
}
