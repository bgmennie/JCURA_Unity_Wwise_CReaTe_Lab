using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class Headtracking : MonoBehaviour
{
    [Header("OSC Headtracker")]
    public int localPort;
    public OSCReceiver receiver;
    const string oscMessageFilter = "/gyrosc/GyrOSC/gyro";

    // Variables for 3DoF
    public float pitchOSC; 
    public float rollOSC;
    public float yawOSC;
    public float pitchDegrees;
    public float rollDegrees;
    public float yawDegrees;

    [Header("Player Body")]
    Vector3 playerRotation;
    Vector3 headRotation;
    public Transform playerBody;

    // Reads the messages received by extOSC
    protected void MessageReceived(OSCMessage message)
    {
        //Debug.LogFormat("Received message: {0}", message);
        var values = message.Values;

        pitchOSC = values[0].FloatValue;
        rollOSC = values[1].FloatValue;
        yawOSC = values[2].FloatValue;
    }

    // Scales the gyrOSC output (+/-pi) to Unity euler angles (0-360 degrees)
    public float gyrOSCToDegrees(float gyrOSCInput) 
    {
        float degrees;

        gyrOSCInput = gyrOSCInput / Mathf.PI;
        degrees = gyrOSCInput * 360f;

        return degrees;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Hide and lock cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Create an OSC receiver
        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = localPort;
        receiver.Bind(oscMessageFilter, MessageReceived);
    }

    // Update is called once per frame
    void Update()
    {
        pitchDegrees = -1.0f * gyrOSCToDegrees(pitchOSC);
        pitchDegrees = Mathf.Clamp(pitchDegrees, -50f, 50f); // This prevents the gimbal lock that seem to only happen to pitch...
        rollDegrees = gyrOSCToDegrees(rollOSC);
        yawDegrees = -1.0f * gyrOSCToDegrees(yawOSC);

        headRotation = new Vector3(pitchDegrees, yawDegrees, rollDegrees);
        transform.eulerAngles = headRotation;
    }
}

