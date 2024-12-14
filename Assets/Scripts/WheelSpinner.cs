using UnityEngine;
using UnityEngine.UI;

public class WheelSpinner : MonoBehaviour
{
    public float spinSpeed = 500f; // Fixed spin speed
    public float decelerationRate = 50f;
    public Button spinButton;
    public Button resetButton;
    public Button startTimerButton; // Button to start the timer
    private bool isSpinning = false;
    private float currentSpeed;
    private float targetAngle;
    private float angle;

    public string[] exercises = { "Push-Ups", "Bench Press", "Incline Press", "Dumbbell Flys", "Chest Dips", "Cable Crossovers" };
    public TMPro.TextMeshProUGUI selectedExerciseText;
    public TMPro.TextMeshProUGUI timerText; // Text to display the countdown timer

    private bool isTimerRunning = false; // Flag to track timer state
    private float timerDuration = 60f; // 60 seconds timer
    private float currentTime; // Current time left on the timer

    void Start()
    {
        spinButton.onClick.AddListener(SpinWheel);
        resetButton.onClick.AddListener(ResetWheel);
        startTimerButton.onClick.AddListener(StartTimer); // Add listener for the timer button
        angle = transform.eulerAngles.z;

        resetButton.interactable = true; // Enable reset button initially
        startTimerButton.interactable = false; // Disable timer button initially
        timerText.text = ""; // Clear timer text at start
    }

    public void SpinWheel()
    {
        if (!isSpinning)
        {
            targetAngle = Random.Range(1080f, 1800f); // Multiple rotations
            currentSpeed = spinSpeed; // Use a fixed spin speed
            isSpinning = true;
            resetButton.interactable = false; // Disable reset button during spin
            startTimerButton.interactable = false; // Disable timer button during spin
        }
    }

    void Update()
    {
        if (isSpinning)
        {
            // Decelerate the wheel
            currentSpeed = Mathf.Max(currentSpeed - decelerationRate * Time.deltaTime, 0);
            angle -= currentSpeed * Time.deltaTime;

            // Rotate the wheel
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // If the wheel is almost stopped
            if (currentSpeed <= 0)
            {
                angle = NormalizeAngle(angle); // Normalize angle after spinning
                transform.rotation = Quaternion.Euler(0, 0, angle); // Lock the wheel position
                isSpinning = false;
                OnWheelStop();
            }
        }

        // Timer countdown logic
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Ceil(currentTime) + "s";

            if (currentTime <= 0)
            {
                isTimerRunning = false;
                timerText.text = "Time's Up!";
                EnableAllButtons(); // Re-enable all buttons after the timer ends
            }
        }
    }

    // Normalize angle to ensure it is always between 0-360 degrees
    float NormalizeAngle(float angle)
    {
        return (angle % 360f + 360f) % 360f;
    }

    void OnWheelStop()
    {
        // Calculate which segment the wheel landed on
        float segmentAngle = 360f / exercises.Length;
        int selectedSegment = Mathf.FloorToInt((360f - (angle % 360f)) / segmentAngle) % exercises.Length;
        string selectedExercise = exercises[selectedSegment];

        // Display the selected exercise
        selectedExerciseText.text = selectedExercise;
        Debug.Log("Wheel stopped at: " + angle + " degrees. Selected Exercise: " + selectedExercise);

        // Enable reset and timer buttons after spin completes
        resetButton.interactable = true;
        startTimerButton.interactable = true;
    }

    public void ResetWheel()
    {
        // Reset the wheel to a random angle within a range
        angle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Clear timer text and disable timer button
        timerText.text = "";
        isTimerRunning = false;

        // Enable spin and reset buttons
        spinButton.interactable = true;
        resetButton.interactable = true;
        startTimerButton.interactable = false;
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            currentTime = timerDuration; // Reset the timer to 60 seconds
            isTimerRunning = true; // Start the timer
            DisableAllButtons(); // Disable all buttons while the timer is running
        }
    }

    private void DisableAllButtons()
    {
        spinButton.interactable = false;
        resetButton.interactable = false;
        startTimerButton.interactable = false;
    }

    private void EnableAllButtons()
    {
        spinButton.interactable = true;
        resetButton.interactable = true;
        startTimerButton.interactable = true;
    }
}
