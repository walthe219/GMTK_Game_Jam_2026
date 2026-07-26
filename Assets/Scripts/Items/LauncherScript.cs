using UnityEngine;

public class LauncherScript : MonoBehaviour
{
    [Range(0f, 90f)]

  
    [SerializeField] public float launchAngle;
    [SerializeField] public float launchPower;

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            LaunchRigidbody(rb);
        }
        else if (false)
        {

        }
    }

    void LaunchPlayer(SimpleMovement player)
    {
        Debug.Log("Launching " + player.gameObject.name);
        Vector3 launchDir = new Vector3(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle), 0);
        player.ApplyExternalVelocity(launchDir * 1000 * launchPower);
    }

    void LaunchRigidbody(Rigidbody rb)
    {
        ClearForces(rb);
        Debug.Log("Launching " + rb.gameObject.name);
        Vector3 launchDir = transform.forward * Mathf.Cos(launchAngle * Mathf.Deg2Rad) + transform.up * Mathf.Sin(launchAngle * Mathf.Deg2Rad);
        rb.AddForce(launchDir * launchPower, ForceMode.Impulse);
    }

    void ClearForces(Rigidbody rb)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }
}
