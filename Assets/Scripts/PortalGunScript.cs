using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGunScript : MonoBehaviour
{
    [SerializeField] Transform ShootPoint;
    [Range(5,100)]
    [SerializeField] float shootDist;
    [SerializeField] float portalSpawnOffeset = 0.01f; //distance from wall portal spawns, prevents z-fighting

    [SerializeField] GameObject leftPortalPrefab;
    [SerializeField] GameObject rightPortalPrefab;

    Camera cam;
    InputAction leftPortalButton;
    InputAction rightPortalButton;

    [SerializeField] GameObject currentLeftPortal;
    [SerializeField] GameObject currentRightPortal;

    private void Start()
    {
        cam = Camera.main;
        leftPortalButton = InputSystem.actions.FindAction("LeftPortal");
        rightPortalButton = InputSystem.actions.FindAction("RightPortal");
    }

    private void Update()
    {
        //Destroy any existing portal before spawning a new one
        if (leftPortalButton.WasCompletedThisFrame() && currentLeftPortal)
        {
            Destroy(currentLeftPortal);
            currentLeftPortal = null;
        }    
        if (rightPortalButton.WasCompletedThisFrame() && currentRightPortal)
        {
            Destroy(currentRightPortal);
            currentRightPortal = null;
        }

        //spawn portal based on button corresponding button press
        if (leftPortalButton.WasCompletedThisFrame() && !currentLeftPortal)
        {
            ShootPortal(true);
            Debug.Log("Left Portal Fired");
        }
        else if (rightPortalButton.WasCompletedThisFrame() && !currentRightPortal) 
        {
            ShootPortal(false);
            Debug.Log("Right Portal Fired");
        }
    }


    void ShootPortal(bool leftPortal)
    {
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shootDist, ~0, QueryTriggerInteraction.Ignore);
        if (hitSomething)
        {
            GameObject prefab = leftPortal ? leftPortalPrefab : rightPortalPrefab;
            var portal = Instantiate(prefab);
            portal.transform.position = hit.point + portalSpawnOffeset * hit.normal;
            portal.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

            PortalScript script = portal.GetComponent<PortalScript>();
            if (!script)
                script = portal.AddComponent<PortalScript>();

            script.OnPlayerCollision += PlayerPortalCollision;
            script.OnPortalCollision += PortalPortalCollision;
            script.OnObjectCollision += ObjectPortalCollison;

            if (leftPortal) currentLeftPortal = portal;
            else currentRightPortal = portal;
        }
    }

    void PlayerPortalCollision(GameObject portal, Collision collsion)
    {

    }

    void PortalPortalCollision(GameObject portal, Collision collsion)
    {
        if (portal == currentLeftPortal && currentRightPortal)
        {
            Destroy(currentLeftPortal);
            currentLeftPortal = null;
        }
        if(portal == currentRightPortal && currentLeftPortal)
        {
            Destroy(currentRightPortal);
            currentRightPortal = null;
        }

        
    }

    void ObjectPortalCollison(GameObject portal, Collision collsion)
    {
        GameObject obj = collsion.gameObject;

        GameObject targetPortal = portal == currentLeftPortal ? currentLeftPortal : currentRightPortal;
        if (targetPortal)
        {
            obj.transform.position = targetPortal.transform.position;
            obj.transform.rotation = targetPortal.transform.rotation;
        }
    }

}
