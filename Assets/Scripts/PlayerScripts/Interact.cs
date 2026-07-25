using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    Camera cam;
    InputAction interact;

    public Transform carryPoint;
    public float interactDistance;
    public ICarrayable carrying;

    private void OnEnable()
    {
        cam = Camera.main;
        interact = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (interact.WasPressedThisFrame())
        {
            Debug.Log("Try interacting");
            RaycastHit hit;
            bool hitSomething = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance);

            if (!hitSomething)
            {
                return;
            }
                var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                var carrayable = hit.collider.gameObject.GetComponent<ICarrayable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
            else if (carrayable != null && carrying == null)
            {
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
                }
            }
        }

    }

    void PickUpItem()
    {

    }

    void PutDownItem()
    {

    }
}
