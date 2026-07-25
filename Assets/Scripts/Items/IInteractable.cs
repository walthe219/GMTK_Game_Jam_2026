using UnityEngine;

public interface IInteractable 
{
    public void Interact()
    {
        Debug.Log("You interacted with an object");
    }
}
