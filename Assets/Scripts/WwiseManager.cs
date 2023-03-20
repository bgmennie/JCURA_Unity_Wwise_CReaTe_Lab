using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class WwiseManager : MonoBehaviour
{
    public static WwiseManager wwiseManagerSingleton { get; private set; }
    public string testType;
    public int testNumber;
    public string testEvent;
    public int stereoSpatialFlag;

    // Path for recorded results
    public string resultsPath;

    [Header("Spatial WwiseEvents")]
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_L;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_R;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_Sub;

    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose_L;
    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose_R;
    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose_Sub;

    // 0
    public AK.Wwise.Event Play_Reference_AG_L;
    public AK.Wwise.Event Play_Reference_AG_R;
    public AK.Wwise.Event Play_Reference_AG_Sub;

    public AK.Wwise.Event Play_User_AG_L;
    public AK.Wwise.Event Play_User_AG_R;
    public AK.Wwise.Event Play_User_AG_Sub;
    // 1
    public AK.Wwise.Event Play_Reference_Bass_L;
    public AK.Wwise.Event Play_Reference_Bass_R;
    public AK.Wwise.Event Play_Reference_Bass_Sub;
    
    public AK.Wwise.Event Play_User_Bass_L;
    public AK.Wwise.Event Play_User_Bass_R;
    public AK.Wwise.Event Play_User_Bass_Sub;
    // 2
    public AK.Wwise.Event Play_Reference_Bassoon_L;
    public AK.Wwise.Event Play_Reference_Bassoon_R;
    public AK.Wwise.Event Play_Reference_Bassoon_Sub;
    
    public AK.Wwise.Event Play_User_Bassoon_L;
    public AK.Wwise.Event Play_User_Bassoon_R;
    public AK.Wwise.Event Play_User_Bassoon_Sub;
    // 3
    public AK.Wwise.Event Play_Reference_Kick_and_Snare_L;
    public AK.Wwise.Event Play_Reference_Kick_and_Snare_R;
    public AK.Wwise.Event Play_Reference_Kick_and_Snare_Sub;
    
    public AK.Wwise.Event Play_User_Kick_and_Snare_L;
    public AK.Wwise.Event Play_User_Kick_and_Snare_R;
    public AK.Wwise.Event Play_User_Kick_and_Snare_Sub;
    // 4
    public AK.Wwise.Event Play_Reference_Voice_L;
    public AK.Wwise.Event Play_Reference_Voice_R;
    public AK.Wwise.Event Play_Reference_Voice_Sub;
    
    public AK.Wwise.Event Play_User_Voice_L;
    public AK.Wwise.Event Play_User_Voice_R;
    public AK.Wwise.Event Play_User_Voice_Sub;

    [Header("Spatial WwiseEmitters")]
    public GameObject leftEmitter;
    public GameObject rightEmitter;
    public GameObject subEmitter;

    [Header("Stereo WwiseEvents")]
    //public AK.Wwise.Event[] stereoReferenceWwiseEvents;
    //public AK.Wwise.Event[] stereoUserWwiseEvents;
    public int stereoEventIndex;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose;
    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose;
    
    // 0
    public AK.Wwise.Event Play_Reference_AG;
    public AK.Wwise.Event Play_User_AG;
    // 1
    public AK.Wwise.Event Play_Reference_Bass;
    public AK.Wwise.Event Play_User_Bass;
    // 2
    public AK.Wwise.Event Play_Reference_Bassoon;
    public AK.Wwise.Event Play_User_Bassoon;
    // 3
    public AK.Wwise.Event Play_Reference_Kick_and_Snare;
    public AK.Wwise.Event Play_User_Kick_and_Snare;
    // 4
    public AK.Wwise.Event Play_Reference_Voice;
    public AK.Wwise.Event Play_User_Voice;


    [Header("Stereo WwiseEmitters")]
    public GameObject stereoUserEmitter;
    //public GameObject stereoReferenceEmitter;
   
    [Header("Test Signal WwiseEvents")]
    public AK.Wwise.Event Play_Pink_Noise;

    [Header("Utility Events")]
    public AK.Wwise.Event Stop_All;

    //// RTPCs

    // Stereo
    public AK.Wwise.RTPC Bus_Switch;
    
    // Reference
    public AK.Wwise.RTPC Stereo_Reference_Pan;
    [SerializeField]
    private float referencePan;

    public AK.Wwise.RTPC Stereo_Reference_Gain;
    [SerializeField]
    private float referenceGain;

    public AK.Wwise.RTPC Stereo_Reference_Reverb_Length;
    [SerializeField]
    private float referenceReverbLength;

    public AK.Wwise.RTPC Stereo_Reference_Wet_Level;
    [SerializeField]
    private float referenceWetLevel;

    // User
    public AK.Wwise.RTPC Stereo_User_Pan;
    public AK.Wwise.RTPC Stereo_User_Wet_Level;
    public AK.Wwise.RTPC Stereo_User_Reverb_Length;
    public AK.Wwise.RTPC Stereo_User_Gain;
    
    // Master volume
    public AK.Wwise.RTPC Output_Bus_Volume;

    // Output Bus Volume tracker
    public float outputBusVolumeValue;
    
    // Timer
    private float switchBusDelayTimer;
    public float screenLoadDelay = 0.1f;
    private float timeToCompleteTimer;

    // MIDI Pads and Knobs
    #region

    // MIDI Pads
    public int mk3MidiPad1 = 40;
    private float mk3MidiPad1Value;

    public int mk3MidiPad2 = 41;
    private float mk3MidiPad2Value;

    public int mk3MidiPad3 = 42;

    public int mk3MidiPad4 = 43;

    public int mk3MidiPad5 = 36;

    public int mk3MidiPad6 = 37;

    public int mk3MidiPad7 = 38;

    public int mk3MidiPad8 = 39;

    // MIDI Knobs
    public int mk3MidiKnob1 = 70;
    private float mk3MidiKnob1Value;

    public int mk3MidiKnob2 = 71;
    private float mk3MidiKnob2Value;

    public int mk3MidiKnob3 = 72;
    private float mk3MidiKnob3Value;

    public int mk3MidiKnob4 = 73;
    private float mk3MidiKnob4Value;

    public int mk3MidiKnob5 = 74;
    private float mk3MidiKnob5Value;

    public int mk3MidiKnob6 = 75;
    private float mk3MidiKnob6Value;

    public int mk3MidiKnob7 = 76;
    private float mk3MidiKnob7Value;

    public int mk3MidiKnob8 = 77;
    private float mk3MidiKnob8Value;

    #endregion

   

    private void Awake()
    {
        GameObject temp = gameObject;
        if (wwiseManagerSingleton != null)
        {
            Debug.Log("There's a problem, more than one singleton");
        }
        wwiseManagerSingleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        switchBusDelayTimer = 0.0f;
        timeToCompleteTimer = 0.0f;
        testNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switchBusDelayTimer += Time.deltaTime;
        timeToCompleteTimer += Time.deltaTime;
        updateWwiseRTPCs();
    }

    public void setMIDIPadsAndKnobs(int[] pads, int[] knobs)
    {

    }

    // There are three types of tests:
    // 1. Pan
    // 2. Reverb
    // 3. Gain
    public void setTestType(string inputTestType, int inputStereoSpatialFlag)
    {
        timeToCompleteTimer = 0.0f;

        stereoSpatialFlag = inputStereoSpatialFlag;

        if (inputTestType == "Pan")
        { 
            testType = inputTestType;

            //// RTPC setting

            // Reference Pan RTPC setting (random)
            referencePan = Random.Range(0.0f, 1.0f);
            Stereo_Reference_Pan.SetGlobalValue(referencePan);
            //Debug.Log("WwiseManager - referencePan value: " + referencePan);

            // User Pan RTPC setting (center)
            Stereo_User_Pan.SetGlobalValue(0.5f);

            // Reverb (reverb length and wet level at min)
            Stereo_Reference_Reverb_Length.SetGlobalValue(0.0f);
            Stereo_Reference_Wet_Level.SetGlobalValue(0.0f);
            Stereo_User_Reverb_Length.SetGlobalValue(0.0f);
            Stereo_User_Wet_Level.SetGlobalValue(0.0f);

            // Gain (max)
            Stereo_Reference_Gain.SetGlobalValue(1.0f);
            Stereo_User_Gain.SetGlobalValue(1.0f);

            // Bus Switch (Reference)
            Bus_Switch.SetGlobalValue(0.0f);

            // Master Bus Volume (volume level at max)
            Output_Bus_Volume.SetGlobalValue(1.0f);
        }
        else if (inputTestType == "Reverb")
        {
            testType = inputTestType;

            //// RTPC setting
            // Reference Pan RTPC setting (center)
            Stereo_Reference_Pan.SetGlobalValue(0.5f);

            // User Pan RTPC setting (center)
            Stereo_User_Pan.SetGlobalValue(0.5f);

            // Reference Reverb Length (random)
            referenceReverbLength = Random.Range(0.3f, 1.0f);
            Stereo_Reference_Reverb_Length.SetGlobalValue(referenceReverbLength);
            Debug.Log("WwiseManager - referenceReverbLength: " + referenceReverbLength);

            // Reference Wet Level (random)
            referenceWetLevel = Random.Range(0.3f, 1.0f);
            Stereo_Reference_Wet_Level.SetGlobalValue(referenceWetLevel);
            Debug.Log("WwiseManager - referenceWetLevel: " + referenceWetLevel);

            // User Reverb (wet level at 0%)
            Stereo_User_Wet_Level.SetGlobalValue(0.0f);

            // Gain (max)
            Stereo_Reference_Gain.SetGlobalValue(1.0f);
            Stereo_User_Gain.SetGlobalValue(1.0f);

            // Bus Switch (Reference)
            Bus_Switch.SetGlobalValue(0.0f);

            // Master Bus Volume (volume level at max)
            Output_Bus_Volume.SetGlobalValue(1.0f);
        }
        else if (inputTestType == "Gain")
        {
            testType = inputTestType;

            // Reference Gain (random)
            referenceGain = Random.Range(0.1f, 1.0f);
            Stereo_Reference_Gain.SetGlobalValue(referenceGain);
            //Debug.Log("WwiseManager - referenceGain value: " + referenceGain);

            // User Gain 50%
            Stereo_User_Gain.SetGlobalValue(0.5f);

            // Panning (50% each)
            Stereo_Reference_Pan.SetGlobalValue(0.5f);
            Stereo_User_Pan.SetGlobalValue(0.5f);

            // Reverb (reverb length and wet level at min)
            Stereo_Reference_Reverb_Length.SetGlobalValue(0.0f);
            Stereo_Reference_Wet_Level.SetGlobalValue(0.0f);
            Stereo_User_Reverb_Length.SetGlobalValue(0.0f);
            Stereo_User_Wet_Level.SetGlobalValue(0.0f);

            // Bus Switch (Reference)
            Bus_Switch.SetGlobalValue(0.0f);

            // Master Bus Volume (volume level at max)
            Output_Bus_Volume.SetGlobalValue(1.0f);
        }
        else if (inputTestType == "VolumeSetup") 
        {
            testType = inputTestType;
            outputBusVolumeValue = 1.0f;
            Output_Bus_Volume.SetGlobalValue(outputBusVolumeValue);
        }
        else
        {
            Debug.LogWarning("WwiseManager - There was an invalid test type passed to setTestType");
        }
    }

    public void updateWwiseRTPCs()
    {
        mk3MidiPad1Value = MidiMaster.GetKey(mk3MidiPad1);
        mk3MidiPad2Value = MidiMaster.GetKey(mk3MidiPad2);

        // Delay when loading new screen such that MIDI messages don't
        // start the events multiple times
        if (switchBusDelayTimer >= screenLoadDelay)
        {
            if (mk3MidiPad1Value > 0.0 && mk3MidiPad2Value > 0.0)
            {
                // Do nothing if both pads have been pressed
            }
            else if (mk3MidiPad1Value > 0.0 && mk3MidiPad2Value == 0.0) 
            {
                // Reference
                Bus_Switch.SetGlobalValue(0.0f);
                switchBusDelayTimer = 0.0f;
                if (testType == "VolumeSetup") // Volume Setup mute button
                {
                    outputBusVolumeValue = (outputBusVolumeValue + 1.0f) % 2.0f;
                    Output_Bus_Volume.SetGlobalValue(outputBusVolumeValue);
                }
            }
            else if (mk3MidiPad1Value == 0.0 && mk3MidiPad2Value > 0.0) 
            {
                // User
                Bus_Switch.SetGlobalValue(1.0f);
                switchBusDelayTimer = 0.0f;
            }
            else if (mk3MidiPad1Value == 0.0 && mk3MidiPad2Value == 0.0)
            {
                // For testing purposes
            }
        }

        mk3MidiKnob1Value = MidiMaster.GetKnob(mk3MidiKnob1);
        mk3MidiKnob2Value = MidiMaster.GetKnob(mk3MidiKnob2);

        Debug.Log("WwiseManager - TESTTYPE: " + testType);

        if (testType == "Pan")
        {
            // Pan settings
            Stereo_Reference_Pan.SetGlobalValue(referencePan);
            Stereo_User_Pan.SetGlobalValue(mk3MidiKnob1Value);

            // Other RTPC settings
            //Stereo_User_Wet_Level.SetGlobalValue(0.0f);

            //Stereo_Reference_Gain.SetGlobalValue(1.0f);
            //Stereo_User_Gain.SetGlobalValue(1.0f);

            //Output_Bus_Volume.SetGlobalValue(1.0f);

            Debug.Log("WwiseManager - PAN mk3MidiKnob1Value: " + mk3MidiKnob1Value);
        }
        else if (testType == "Reverb")
        {
            // Reverb settings
            Stereo_User_Wet_Level.SetGlobalValue(mk3MidiKnob1Value);
            Stereo_User_Reverb_Length.SetGlobalValue(mk3MidiKnob2Value);

            // Other RTPC settings
            //Stereo_Reference_Pan.SetGlobalValue(0.5f);
            //Stereo_User_Pan.SetGlobalValue(0.5f);

            //Stereo_Reference_Gain.SetGlobalValue(1.0f);
            //Stereo_User_Gain.SetGlobalValue(1.0f);

            //Output_Bus_Volume.SetGlobalValue(1.0f);

            Debug.Log("WwiseManager - REVERB mk3MidiKnob1Value: " + mk3MidiKnob1Value);
            Debug.Log("WwiseManager - REVERB mk3MidiKnob2Value: " + mk3MidiKnob2Value);
        }
        else if (testType == "Gain")
        {
            // Gain settings
            Stereo_Reference_Gain.SetGlobalValue(referenceGain);
            Stereo_User_Gain.SetGlobalValue(mk3MidiKnob1Value);

            // Other RTPC settings
            //Stereo_Reference_Pan.SetGlobalValue(0.5f);
            //Stereo_User_Pan.SetGlobalValue(0.5f);

            //Stereo_User_Wet_Level.SetGlobalValue(0.0f);
            
            //Output_Bus_Volume.SetGlobalValue(1.0f);

            Debug.Log("WwiseManager - GAIN mk3MidiKnob1Value: " + mk3MidiKnob1Value);
        }
    }

    public void closeWwiseScene()
    {
        Stereo_User_Pan.SetGlobalValue(0.5f);
        Stereo_User_Wet_Level.SetGlobalValue(0.5f);
        Stereo_User_Reverb_Length.SetGlobalValue(0.5f);

        // Creating path for results text file in MyDocuments/JCURA
        string userResultsFile = "userResultsTest" + testNumber + ".txt";
        string userResultsFolder = "userResults";
        string jcuraFolder = "JCURA";
        string myDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        resultsPath = Path.Combine(myDocuments, jcuraFolder, userResultsFolder, userResultsFile);

        if (testNumber < 54)
        {
            if (testType == "Pan" || testType == "Reverb" || testType == "Gain")
            {
                writeResults();
            }
        }

        // Iterate testNumber
        testNumber++;
    }

    public void writeResults()
    {
        string[] userOutput = new string[9];

        // Test Number
        userOutput[0] = "TestNumber: " + testNumber.ToString();

        // Modality
        if (testNumber < 18)
        {
            userOutput[1] = "Modality: Speakers";
        }
        else if (testNumber >= 18 && testNumber < 36)
        {
            userOutput[1] = "Modality: Stereo Headphones";
        }
        else if (testNumber >= 36 && testNumber < 54)
        {
            userOutput[1] = "Modality: Virtual Mixing Environment";
        }

        // Track Name
        userOutput[2] = "Track name: " + testEvent;

        // Test Type
        userOutput[3] = "Test type: " + testType;

        // Expected + User Value
        // TODO: Add reverb
        if (testType == "Pan")
        {
            userOutput[4] = "Reference Pan Value: " + referencePan;
            userOutput[5] = "User Pan Value: " + mk3MidiKnob1Value;
            userOutput[6] = "-";
            userOutput[7] = "-";
        }
        else if (testType == "Reverb")
        {
            userOutput[4] = "Reference Reverb Length Value: " + referenceReverbLength;
            userOutput[5] = "User Reverb Length Value: " + mk3MidiKnob1Value;
            userOutput[6] = "Reference Wet Level Value: " + referenceWetLevel;
            userOutput[7] = "User Wet Level Value: " + mk3MidiKnob2Value;
        }
        else if (testType == "Gain")
        {
            userOutput[4] = "Reference Gain Value: " + referenceGain;
            userOutput[5] = "User Gain Value: " + mk3MidiKnob1Value;
            userOutput[6] = "-";
            userOutput[7] = "-";
        }

        // Time to complete
        userOutput[8] = "Time to complete (seconds): " + timeToCompleteTimer.ToString();

        // Write to file
        File.WriteAllLines(resultsPath, userOutput);
    }

    public void postStereoWwiseEvent(string eventName)
    {
        testEvent = eventName;

        if (eventName == "Stop_All")
        {
            Stop_All.Post(stereoUserEmitter);
            //Stop_All.Post(stereoReferenceEmitter);
        } 
        else if (eventName == "Jethro_Tull")
        {
            Play_Reference_Jethro_Tull_Mother_Goose.Post(stereoUserEmitter);
            Play_User_Jethro_Tull_Mother_Goose.Post(stereoUserEmitter);
            //Play_Jethro_Tull_Mother_Goose.Post(stereoReferenceEmitter);
        }
        else if (eventName == "AG")
        {
            Play_Reference_AG.Post(stereoUserEmitter);
            Play_User_AG.Post(stereoUserEmitter);
        }
        else if (eventName == "Bass")
        {
            Play_Reference_Bass.Post(stereoUserEmitter);
            Play_User_Bass.Post(stereoUserEmitter);
        }
        else if (eventName == "Bassoon")
        {
            Play_Reference_Bassoon.Post(stereoUserEmitter);
            Play_User_Bassoon.Post(stereoUserEmitter);
        }
        else if (eventName == "Kick and Snare")
        {
            Play_Reference_Kick_and_Snare.Post(stereoUserEmitter);
            Play_User_Kick_and_Snare.Post(stereoUserEmitter);
        }
        else if (eventName == "Voice")
        {
            Play_Reference_Voice.Post(stereoUserEmitter);
            Play_User_Voice.Post(stereoUserEmitter);
        }
        else if (eventName == "Pink_Noise")
        {
            Play_Pink_Noise.Post(stereoUserEmitter);
        }
        else
        {
            Debug.LogWarning("WwiseManager - Invalid eventName: " + eventName);
        }
    }
    
    public void postSpatialWwiseEvent(string eventName)
    {
        testEvent = eventName;

        if (eventName == "Stop_All")
        {
            Stop_All.Post(leftEmitter);
            Stop_All.Post(rightEmitter);
            Stop_All.Post(subEmitter);
        }
        else if (eventName == "Jethro_Tull")
        {
            // Reference
            Play_Reference_Jethro_Tull_Mother_Goose_L.Post(leftEmitter);
            Play_Reference_Jethro_Tull_Mother_Goose_R.Post(rightEmitter);
            Play_Reference_Jethro_Tull_Mother_Goose_Sub.Post(subEmitter);

            // User
            Play_User_Jethro_Tull_Mother_Goose_L.Post(leftEmitter);
            Play_User_Jethro_Tull_Mother_Goose_R.Post(rightEmitter);
            Play_User_Jethro_Tull_Mother_Goose_Sub.Post(subEmitter);
        }
        else if (eventName == "AG")
        {
            // Reference
            Play_Reference_AG_L.Post(leftEmitter);
            Play_Reference_AG_R.Post(rightEmitter);
            Play_Reference_AG_Sub.Post(subEmitter);

            // User
            Play_User_AG_L.Post(leftEmitter);
            Play_User_AG_R.Post(rightEmitter);
            Play_User_AG_Sub.Post(subEmitter);
        }
        else if (eventName == "Bass")
        {
            // Reference
            Play_Reference_Bass_L.Post(leftEmitter);
            Play_Reference_Bass_R.Post(rightEmitter);
            Play_Reference_Bass_Sub.Post(subEmitter);

            // User
            Play_User_Bass_L.Post(leftEmitter);
            Play_User_Bass_R.Post(rightEmitter);
            Play_User_Bass_Sub.Post(subEmitter);
        }
        else if (eventName == "Bassoon")
        {
            // Reference
            Play_Reference_Bassoon_L.Post(leftEmitter);
            Play_Reference_Bassoon_R.Post(rightEmitter);
            Play_Reference_Bassoon_Sub.Post(subEmitter);

            // User
            Play_User_Bassoon_L.Post(leftEmitter);
            Play_User_Bassoon_R.Post(rightEmitter);
            Play_User_Bassoon_Sub.Post(subEmitter);
        }
        else if (eventName == "Kick and Snare")
        {
            // Reference
            Play_Reference_Kick_and_Snare_L.Post(leftEmitter);
            Play_Reference_Kick_and_Snare_R.Post(rightEmitter);
            Play_Reference_Kick_and_Snare_Sub.Post(subEmitter);

            // User
            Play_User_Kick_and_Snare_L.Post(leftEmitter);
            Play_User_Kick_and_Snare_R.Post(rightEmitter);
            Play_User_Kick_and_Snare_Sub.Post(subEmitter);
        }
        else if (eventName == "Voice")
        {
            // Reference
            Play_Reference_Voice_L.Post(leftEmitter);
            Play_Reference_Voice_R.Post(rightEmitter);
            Play_Reference_Voice_Sub.Post(subEmitter);

            // User
            Play_User_Voice_L.Post(leftEmitter);
            Play_User_Voice_R.Post(rightEmitter);
            Play_User_Voice_Sub.Post(subEmitter);
        }
    }
}
