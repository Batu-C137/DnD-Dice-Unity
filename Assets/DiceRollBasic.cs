/*todo:
 * 
 * - Deactivate autoroll button in sensor mode
 * - More smoother rolling animation(to much gliding etc.)
 * - Animations if dice hits something
 * - Animations candle etc.
 * 
 */


using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

[RequireComponent(typeof(Rigidbody))]
public class DiceRollBasic : MonoBehaviour
{
    public Rigidbody rigidBody;
    GameObject rollDiceButton;

    [SerializeField] private float maxRandomForceValue = 50f, startRollingForce = 10f;
    [SerializeField] float maxRollTime = 5f;
    [SerializeField] float customGravity = -25f;
    //[SerializeField] float maxRollTime = 3f;

    private float forceX, forceY, forceZ;

    public int diceSideNumb;
    CountdownTimer rollTimer;

    float magnitude;

    private Vector3 rotationRate;
    private Vector3 velocity;
    private Vector3 tilt;
    private Vector3 lastAcceleration;
    private Vector3 currentAcceleration;
    private Vector3 accelerationDelta;
    private Vector3 forceDirection;

    //CountdownTimer rollTimer;

    bool sensor = false;
    bool isAtRest = false;

    /// <summary>
    /// Vairables for gyroscope
    /// </summary>
    private Gyroscope gyroscope;
    private const float thresholdMove = 5.0f;
    private const float thresholdZero = 0.1f;

    private void Awake()
    {
        //rollTimer = new CountdownTimer(maxRollTime);
        //rollTimer.OnTimerStart += RollDice;
        //rollTimer.OnTimerStop += () => finalize = true;

        rollDiceButton = GameObject.Find("RollDiceButton");

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody != null)
        {
            if(sensor)
            {
                rotationRate = gyroscope.rotationRate;
                velocity = gyroscope.userAcceleration;

                currentAcceleration = Input.acceleration;
                accelerationDelta = currentAcceleration - lastAcceleration;

                // calc the strength of movement (vector length)
                magnitude = rotationRate.magnitude;

                // check if movement is above threshold
                if (accelerationDelta.magnitude > thresholdMove) //&& rigidBody.velocity == Vector3.zero
                {
                    //if (rollTimer.IsRunning)
                    //{
                    //    rigidBody.AddForce(0, 0, 0);
                    //    rollTimer.Stop();
                    //    return;
                    //}
                    //else
                    //{
                    //    rollTimer.Start();
                    //    RollDice();
                    //    customGravity++;
                    //}

                    RollDice();
                }
            }
            else
            {
                //currentAcceleration = Input.acceleration;
                //accelerationDelta = currentAcceleration - lastAcceleration;

                //// Überprüfe, ob die Bewegung stark genug ist
                //if (accelerationDelta.magnitude > thresholdZero) //&& rigidBody.velocity == Vector3.zero
                //{
                //    isAtRest = false;
                //}
            }

        }
        else
        {
            return;
        }
    }
    void FixedUpdate()
    {
        if(rigidBody != null)
        {
            // Anwenden der benutzerdefinierten Gravitationskraft
            rigidBody.AddForce(new Vector3(0, customGravity, 0), ForceMode.Acceleration);

            if (accelerationDelta.magnitude < thresholdZero && !isAtRest)
            {
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
                isAtRest = true;
            }
        }
    }

    private void RollDice()
    {
        rigidBody.isKinematic = false;

        //forceX = Random.Range(0, maxRandomForceValue);
        //forceY = Random.Range(0, maxRandomForceValue);
        //forceZ = Random.Range(0, maxRandomForceValue);

        //rigidBody.AddForce(rotationRate * startRollingForce);
        //rigidBody.AddTorque(forceX, forceY, forceZ);

        transform.rotation = GyroToUnity(Input.gyro.attitude);

        forceDirection = accelerationDelta.normalized; // Richtung der Kraft

        if (rigidBody.velocity.magnitude < maxRandomForceValue)
        {
            /*rigidBody.AddForce(tilt * velocity.magnitude);*/ // AddForceAtPosition(Vector3 force, Vector3 position)
            rigidBody.AddForce(forceDirection * accelerationDelta.magnitude * startRollingForce, ForceMode.Impulse);
        }

        lastAcceleration = currentAcceleration;

        isAtRest = false;
        //rollTimer.Tick(Time.deltaTime);

    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    private void AutoRollFire()
    {
        isAtRest = false;
    }

    //private void AccelerateValue()
    //{
    //    tilt = Input.acceleration;

    //    tilt = Quaternion.Euler(90, 0, 0) * tilt;
    //}

    private void Initialize()
    {
        if (SystemInfo.supportsGyroscope && SystemInfo.supportsAccelerometer)
        {
            gyroscope = Input.gyro;
            Input.gyro.enabled = true;
            sensor = true;

            rigidBody = GetComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
            lastAcceleration = Input.acceleration;

            rigidBody.useGravity = false;

            //deactivate roll button
            var color = rollDiceButton.GetComponent<Button>().targetGraphic.color;
            color.a = 0;
            rollDiceButton.GetComponent<Button>().enabled = true;
            rollDiceButton.GetComponent<Button>().targetGraphic.color = color;
            rollDiceButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

            rollTimer = new CountdownTimer(maxRollTime);
            //rollTimer.OnTimerStart += PerformInitialRoll;
            //rollTimer.OnTimerStop += () => finalize = true;
        }
        else
        {
            sensor = false;

            //activate roll button
            var color = rollDiceButton.GetComponent<Button>().targetGraphic.color;
            color.a = 255;
            //rollDiceButton.GetComponent<Image>().material.color = new Color(255, 255, 255, 0);
            rollDiceButton.GetComponent<Button>().enabled = true;
            rollDiceButton.GetComponent<Button>().targetGraphic.color = color;
            rollDiceButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

            AutoRoll.OnButtonPressed += AutoRollFire;

            rollTimer = new CountdownTimer(maxRollTime);
        }
    }
}
