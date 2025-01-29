using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody))]
public class AutoRoll : MonoBehaviour
{
    int maxRandomForceValue = 500;

    public static event Action OnButtonPressed; // Event
    public Rigidbody rigidBody; //need to get the Dice body
    Random rng = new Random();

    Vector3 impulseForce;
    int vectorValue1;
    int vectorValue2;
    int vectorValue3;

    /// <summary>
    /// auto roll
    /// </summary>
    public void RollDice()
    {
        if(rigidBody != null)
        {
            vectorValue1 = rng.Next(0, maxRandomForceValue);
            vectorValue2 = rng.Next(0, maxRandomForceValue);
            vectorValue3 = rng.Next(0, maxRandomForceValue);

            impulseForce = new Vector3(vectorValue1, 400, vectorValue3);

            rigidBody.AddForce(impulseForce, ForceMode.Impulse);

            // Trigger event
            OnButtonPressed?.Invoke();
        }
        else
        {
            Debug.LogWarning("No rigidbody found!");
        }

    }
}
