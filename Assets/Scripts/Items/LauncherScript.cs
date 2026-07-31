using UnityEngine;

public class LauncherScript : MonoBehaviour
{
    [Range(0f, 90f)]
    public float launchAngle;
    public float launchPower;

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        var mov = other.GetComponent<SimpleMovement>();
        if(rb != null)
        {
            LaunchRigidbody(rb);
        }
        else if (mov)
        {
            LaunchPlayer(mov);
        }
    }

    void LaunchPlayer(SimpleMovement player)
    {
        Debug.Log("Launching " + player.gameObject.name);
        Vector3 launchDir = transform.forward * Mathf.Cos(launchAngle * Mathf.Deg2Rad) + transform.up * Mathf.Sin(launchAngle * Mathf.Deg2Rad); ;
        player.ApplyExternalVelocity(launchDir * launchPower/2);
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
