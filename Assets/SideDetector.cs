using UnityEngine;

public class SideDetector : MonoBehaviour
{
    DiceRollBasic dice;

    private void Awake()
    {
        dice = FindObjectOfType<DiceRollBasic>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(dice != null)
        {
            //if(dice.GetComponent<Rigidbody>().velocity == Vector3.zero)
            //{
            //    dice.diceSideNumb = int.Parse(other.name);
            //}

            dice.diceSideNumb = int.Parse(other.name);
        }
    }
}
