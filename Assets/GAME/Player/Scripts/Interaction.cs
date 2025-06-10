using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    Inventory inventory;
    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "InteractableObj")
        {
            IInteractable intObj = other.GetComponent<IInteractable>();
            intObj.Interect();
        }

    }
}
