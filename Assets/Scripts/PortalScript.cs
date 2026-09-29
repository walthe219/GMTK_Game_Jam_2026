using UnityEngine;
using System;

public class PortalScript : MonoBehaviour
{
    public Action<GameObject, Collision> OnPlayerCollision;
    public Action<GameObject, Collision> OnPortalCollision;
    public Action<GameObject, Collision> OnObjectCollision;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnPlayerCollision?.Invoke(gameObject, collision);

        }
        else if (collision.gameObject.TryGetComponent<PortalScript>(out PortalScript portalScript))
        {
            OnPortalCollision?.Invoke(gameObject, collision);
        }
        else
        {
            OnObjectCollision?.Invoke(gameObject, collision);
        }
    }
}
