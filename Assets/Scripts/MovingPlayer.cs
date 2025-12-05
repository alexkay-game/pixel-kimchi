using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlayer : MonoBehaviour
{
    [Header("Speed")]
    public float baseSpeed;

    [Header("Acceleration")]
    [SerializeField] float accelMultiplicator;
    [SerializeField] float boostDuration;
    [SerializeField] float accelTime;
    [SerializeField] float returnTimeDown;

    [Header("Deceleration")]
    [SerializeField] float decelMultiplicator;
    [SerializeField] float declineDuration;
    [SerializeField] float decelTime;
    [SerializeField] float returnTimeUp;
    
    [Header("Debug")]
    [SerializeField] float currentSpeed;
    [SerializeField] float targetSpeed;
    [SerializeField] bool isBoosting;
    [SerializeField] float boostTimer;
    [SerializeField] bool isReturningDown;

    [SerializeField] bool isDeclining;
    [SerializeField] float declineTimer;
    [SerializeField] bool isReturningUp;

    float epsilon = 0.01f;

    Vector2 direction;
    Rigidbody2D rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;               //vypnutí gravitace
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        currentSpeed = baseSpeed;

        //pro budoucí případy, abychom nemohli nikdy dělit nulou
        if (accelTime <=0)  accelTime = 0.001f;
        if (returnTimeDown <=0)  returnTimeDown = 0.001f;
        if (decelTime <=0)  decelTime = 0.001f;
        if (returnTimeUp <=0)  returnTimeUp = 0.001f;
    }

    void Update()
    {
        float xInput = 0;
        float yInput = 0;
        
        if (Keyboard.current != null)
        {
            UseKeyboardKeys(ref xInput, ref yInput);
        }

        NormalizeDirection(xInput, yInput);

        // ------- BOOSTING -------
        if (isBoosting)
        {
            float maxDelta = Mathf.Abs(targetSpeed - currentSpeed) / accelTime * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, maxDelta);
            
            if (Mathf.Abs(targetSpeed - currentSpeed) < epsilon)      //bez tohohle nedocházelo ke konci procesu –> zrychlovalo se do nekonečna
            {
                currentSpeed = targetSpeed;
                isBoosting = false;
                StartBoostCountDown();
            }

            return;
        }

        // ------- BOOST RETURNING -------
        else if (isReturningDown)
        {
            float maxDelta = Mathf.Abs(currentSpeed - baseSpeed) / returnTimeDown * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, maxDelta);
            
            if (Mathf.Abs(currentSpeed - baseSpeed) < epsilon)      //bez tohohle nedocházelo ke konci procesu
            {
                currentSpeed = baseSpeed;
                isReturningDown = false;
            }

            return;
        }

        // ------- BOOST TIME COUNTDOWN -------
         else if (boostTimer > 0f)
        {
            boostTimer -= Time.deltaTime;
            
            if (boostTimer <= 0)    StartReturnDown();
        }

        // ------- SLOWING DOWN SPEED -------
        if (isDeclining)
        {
            float maxDelta = Mathf.Abs(currentSpeed - targetSpeed) / accelTime * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, maxDelta);
            
            if (Mathf.Abs(currentSpeed - targetSpeed) < epsilon)      //bez tohohle nedocházelo ke konci procesu –> zrychlovalo se do nekonečna
            {
                currentSpeed = targetSpeed;
                isDeclining = false;
                StartDeclineCountDown();
            }

            return;
        }

        // ------- RETURNING SPEED UP -------
        else if (isReturningUp)
        {
           float maxDelta = Mathf.Abs(baseSpeed - currentSpeed) / returnTimeDown * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, maxDelta);
            
            if (Mathf.Abs(baseSpeed - currentSpeed) < epsilon)      //bez tohohle nedocházelo ke konci procesu
            {
                currentSpeed = baseSpeed;
                isReturningUp = false;
            }

            return; 
        }

        // ------- DECLINING COUNTDOWN -------
        else if (declineTimer > 0f)
        {
            declineTimer -= Time.deltaTime;

            if (declineTimer <= 0)  StartReturnUp();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = currentSpeed * direction;
    }

    private static void UseKeyboardKeys(ref float xInput, ref float yInput)
    {
        if (Keyboard.current.wKey.isPressed) yInput += 1;
        if (Keyboard.current.sKey.isPressed) yInput -= 1;
        if (Keyboard.current.aKey.isPressed) xInput -= 1;
        if (Keyboard.current.dKey.isPressed) xInput += 1;
    }

    private void NormalizeDirection(float xInput, float yInput)
    {
        direction = new Vector2(xInput, yInput);
        if (direction != Vector2.zero)
        {
            direction = direction.normalized;
        }
    }

    public void StartBoost()
    {
        isReturningDown = false;
        targetSpeed = baseSpeed * accelMultiplicator;
        
        if (currentSpeed < targetSpeed) 
            isBoosting = true;
        else
            isBoosting = false;

        boostTimer = boostDuration;
    }

    private void StartBoostCountDown()
    {
       boostTimer = boostDuration;
    }

    private void StartReturnDown()
    { 
        isBoosting = false;
        isReturningDown = true;
        boostTimer = 0;
    }

    public void StartDecrease()
    {
        isReturningUp = false;
        targetSpeed = baseSpeed / decelMultiplicator;

        if (currentSpeed > targetSpeed)
            isDeclining = true;
        else
            isDeclining = false;

        declineTimer = declineDuration;
    }

    private void StartDeclineCountDown()
    {
        declineTimer = declineDuration;
    }

    private void StartReturnUp()
    {
        isDeclining = false;
        isReturningUp = true;
        declineTimer = 0;
    }
}
