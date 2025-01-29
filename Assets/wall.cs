using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "wall")
        {
            print("ENTER");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            print("STAY");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            print("EXIT");
        }
    }
}
