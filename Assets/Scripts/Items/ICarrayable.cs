using UnityEngine;

public interface ICarrayable
{
    public GameObject Pickup()
    {
        Debug.Log("You pickup an object");
        return null;
    }

    public GameObject PutDown()
    {
        Debug.Log("You putdown an object");
        return null;
    }

}
