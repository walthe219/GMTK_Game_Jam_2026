using UnityEngine;
using System;

public interface ICarrayable
{
    public event Action OnPickup;
    public event Action OnPutDown;
    public GameObject Pickup();

    public GameObject PutDown();

}
