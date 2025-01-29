using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DiceSides))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]

public class DiceRoll : MonoBehaviour
{
    [SerializeField] float rollForce = 50f;
    [SerializeField] float torqueAmount = 5f;
    [SerializeField] float maxRollTime = 3f;
    [SerializeField] float minAngularVelocity = 0.1f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxSpeed = 15f;

    [SerializeField] TMPro.TextMeshProUGUI resultText;

    DiceSides diceSides;
    Rigidbody rigidBody;

    CountdownTimer rollTimer;

    Vector3 defaultPosition;
    Vector3 currentVelocity;
    bool finalize;

    /// <summary>
    /// Vairables for gyroscope
    /// </summary>
    private Gyroscope gyroscope;
    private const float Threshold = 1.0f;

    private void Start()
    {

        if (SystemInfo.supportsGyroscope)
        {
            gyroscope = Input.gyro;
            Input.gyro.enabled = true;
        }
    }

    public void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Vector3 rotationRate = gyroscope.rotationRate;

            // calc the strength of movement (vector length)
            float magnitude = rotationRate.magnitude;

            // check if movement is above threshold
            if (magnitude > Threshold)
            {
                if (rollTimer.IsRunning) return;
                rollTimer.Start();

                OnStrongMotionDetected();
            }           

            if (finalize)
            {
                MoveDiceToCenter();
            }
        }

        //if (rollTimer.IsRunning)
        //{
        //    transform.rotation = GyroToUnity(Input.gyro.attitude);
        //}
    }

    void OnStrongMotionDetected()
    {
        //Debug.Log("Starke Bewegung erkannt!");

        GameObject obj = GameObject.Find("Dice_d20"); // Ersetze "YourObjectName" mit dem Namen deines Objekts
        if (obj != null)
        {
            rollTimer.Tick(Time.deltaTime);
            //Renderer renderer = obj.GetComponent<Renderer>();
            //renderer.material.color = Color.red;
        }
    }

    //private Quaternion GyroToUnity(Quaternion q)
    //{
    //    return new Quaternion(q.x, q.y, -q.z, -q.w);
    //}

    private void Awake()
    {
        diceSides = GetComponent<DiceSides>();
        rigidBody = GetComponent<Rigidbody>();

        resultText.text = "Ready to roll";

        defaultPosition = transform.position;

        rollTimer = new CountdownTimer(maxRollTime);
        rollTimer.OnTimerStart += PerformInitialRoll;
        rollTimer.OnTimerStop += () => finalize = true;
    }

    void MoveDiceToCenter()
    {
        transform.position = Vector3.SmoothDamp(transform.position, defaultPosition, ref currentVelocity, smoothTime, maxSpeed);

        if (defaultPosition.InRangeOf(transform.position, 0.1f))
        {
            FinalizeRoll();
        }
    }

    void PerformInitialRoll()
    {
        ResetDiceState();
        resultText.text = "";

        Vector3 targetPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        rigidBody.AddForce(targetPosition * rollForce, ForceMode.Impulse);
        rigidBody.AddTorque(Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);

    }

    void ResetDiceState()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        transform.position = defaultPosition;
    }

    void FinalizeRoll()
    {
        rollTimer.Stop();
        finalize = false;
        ResetDiceState();

        //var particles = InstantiateFX(finalResultEffect, transform.position, 5f);
        //Destroy(particles, 3f);

        int result = diceSides.GetMatch();
        Debug.Log($"Dice landed on {result}");
        resultText.text = result.ToString();
    }

    void OnCollisionEnter(Collision col)
    {
        if (rollTimer.IsRunning && rollTimer.Progress < 0.5f && rigidBody.angularVelocity.magnitude < minAngularVelocity)
        {
            finalize = true;
        }
    }
}
