using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceValue : MonoBehaviour
{
    DiceRollBasic dice;

    [SerializeField]
    TextMeshProUGUI diceValue;

    private void Awake()
    {
        dice = FindObjectOfType<DiceRollBasic>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dice != null)
        {
            diceValue.text = dice.diceSideNumb.ToString();
        }
    }
}
