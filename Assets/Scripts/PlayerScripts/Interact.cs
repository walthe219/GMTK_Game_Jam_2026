using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    Camera cam;
    InputAction interact;

    public Transform carryPoint;
    public float interactDistance;
    public bool showPutDownGhost;
    public GameObject putDownGhostPrefab;

    
    private class CarrayableRef
    {
        public ICarrayable script;
        public GameObject gameObject;
        public Transform transform;
        public Rigidbody rb;

        public CarrayableRef(ICarrayable carry, GameObject obj)
        {
            script = carry;
            gameObject = obj;
            transform = obj.transform;
            rb = obj.GetComponent<Rigidbody>();
        }
    }

    private CarrayableRef carrying;
    private GameObject putDownGhostInstance;

    private void OnEnable()
    {
        cam = Camera.main;
        interact = InputSystem.actions.FindAction("Interact");

        putDownGhostInstance = Instantiate(putDownGhostPrefab,transform);
        putDownGhostInstance.SetActive(false);
    }

    void Update()
    {
        putDownGhostInstance.SetActive(false);

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance, ~0, QueryTriggerInteraction.Ignore);

        if (!hitSomething)
        {
            return;
        }

        var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
        var carrayable = hit.collider.gameObject.GetComponent<ICarrayable>();

        if (interact.WasPressedThisFrame())
        {
            Debug.Log("Try interacting");

            // interact if it is interactable
            if (interactable != null)
            { 
                interactable.Interact();
            }
            // pickup object
            else if (carrayable != null && carrying == null)
            {
                PickUpItem(carrayable);
            }
            else
            {
                // put down object
                if (carrying != null)
                {
                    PutDownItem(hit);
                }
            }
        }

        if (carrying != null && showPutDownGhost)
        {
            putDownGhostInstance.SetActive(true);
            putDownGhostInstance.transform.SetPositionAndRotation(hit.point + hit.normal * 0.6f, transform.rotation);
        }
    }

    void PickUpItem(ICarrayable carrayable)
    {
        carrying = new CarrayableRef(carrayable, carrayable.Pickup());

        carrying.transform.parent = carryPoint;
        carrying.transform.SetPositionAndRotation(carryPoint.position, carryPoint.rotation);
    }

    void PutDownItem(RaycastHit hit)
    {
        carrying.transform.parent = null;

        carrying.script.PutDown();

        carrying.rb.position = hit.point + hit.normal * 0.6f;
        carrying.rb.rotation = transform.rotation;
        carrying.transform.SetPositionAndRotation(carrying.rb.position, carrying.rb.rotation);

        carrying = null;
    }
}
