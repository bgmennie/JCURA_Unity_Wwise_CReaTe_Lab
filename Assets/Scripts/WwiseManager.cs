using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class WwiseManager : MonoBehaviour
{
    public static WwiseManager wwiseManagerSingleton { get; private set; }
    public string testType;

    [Header("Spatial WwiseEvents")]
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_L;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_R;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_Sub;

    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose_L;
    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose_R;
    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose_Sub;
    
    [Header("Spatial WwiseEmitters")]
    public GameObject leftEmitter;
    public GameObject rightEmitter;
    public GameObject subEmitter;

    [Header("Stereo WwiseEvents")]
    public AK.Wwise.Event[] stereoReferenceWwiseEvents;
    public AK.Wwise.Event[] stereoUserWwiseEvents;
    public int stereoEventIndex;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose;
    public AK.Wwise.Event Play_User_Jethro_Tull_Mother_Goose;

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
    private float screenLoadDelayTimer;
    public float screenLoadDelay = 0.1f;

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
        //if (wwiseManagerSingleton != null && wwiseManagerSingleton != this)
        if (wwiseManagerSingleton != null)
        {
            Debug.Log("There's a problem, more than one singleton");
            //Destroy(this);
        }
        //else
        //{
        wwiseManagerSingleton = this;

        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        screenLoadDelayTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        screenLoadDelayTimer += Time.deltaTime;
        updateWwiseRTPCs();
    }

    public void setMIDIPadsAndKnobs(int[] pads, int[] knobs)
    {

    }

    // There are three types of tests:
    // 1. Pan
    // 2. Reverb
    // 3. Gain
    public void setTestType(string inputTestType)
    {
        if (inputTestType == "Pan")
        { 
            testType = inputTestType;

            //// RTPC setting

            // Reference Pan RTPC setting (random)
            referencePan = Random.Range(0.0f, 1.0f);
            Debug.Log("WwiseManager - referencePan value: " + referencePan);
            Stereo_Reference_Pan.SetGlobalValue(referencePan);

            // User Pan RTPC setting (center)
            Stereo_User_Pan.SetGlobalValue(0.5f);

            // Reverb (wet level at min)
            Stereo_User_Wet_Level.SetGlobalValue(0);

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

            // Reverb (wet level at 0%)
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
            referenceGain = Random.Range(0.0f, 1.0f);
            Debug.Log("WwiseManager - referenceGain value: " + referenceGain);
            Stereo_Reference_Gain.SetGlobalValue(referenceGain);

            // User Gain 50%
            Stereo_User_Gain.SetGlobalValue(0.5f);

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
        if (screenLoadDelayTimer >= screenLoadDelay)
        {
            if (mk3MidiPad1Value > 0.0 && mk3MidiPad2Value > 0.0)
            {
                // Do nothing if both pads have been pressed
            }
            else if (mk3MidiPad1Value > 0.0 && mk3MidiPad2Value == 0.0) 
            {
                // Reference
                Bus_Switch.SetGlobalValue(0.0f);
                screenLoadDelayTimer = 0.0f;
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
                screenLoadDelayTimer = 0.0f;
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
            Stereo_User_Wet_Level.SetGlobalValue(0.0f);

            Stereo_Reference_Gain.SetGlobalValue(1.0f);
            Stereo_User_Gain.SetGlobalValue(1.0f);

            Output_Bus_Volume.SetGlobalValue(1.0f);

            Debug.Log("WwiseManager - PAN mk3MidiKnob1Value: " + mk3MidiKnob1Value);
        }
        else if (testType == "Reverb")
        {
            // Reverb settings
            Stereo_User_Wet_Level.SetGlobalValue(mk3MidiKnob1Value);
            Stereo_User_Reverb_Length.SetGlobalValue(mk3MidiKnob2Value);

            // Other RTPC settings
            Stereo_Reference_Pan.SetGlobalValue(0.5f);
            Stereo_User_Pan.SetGlobalValue(0.5f);

            Stereo_Reference_Gain.SetGlobalValue(1.0f);
            Stereo_User_Gain.SetGlobalValue(1.0f);

            Output_Bus_Volume.SetGlobalValue(1.0f);

            Debug.Log("WwiseManager - REVERB mk3MidiKnob1Value: " + mk3MidiKnob1Value);
            Debug.Log("WwiseManager - REVERB mk3MidiKnob2Value: " + mk3MidiKnob2Value);
        }
        else if (testType == "Gain")
        {
            // Gain settings
            Stereo_Reference_Gain.SetGlobalValue(referenceGain);
            Stereo_User_Gain.SetGlobalValue(mk3MidiKnob1Value);

            // Other RTPC settings
            Stereo_Reference_Pan.SetGlobalValue(0.5f);
            Stereo_User_Pan.SetGlobalValue(0.5f);

            Stereo_User_Wet_Level.SetGlobalValue(0.0f);
            
            Output_Bus_Volume.SetGlobalValue(1.0f);

            Debug.Log("WwiseManager - GAIN mk3MidiKnob1Value: " + mk3MidiKnob1Value);
        }
    }

    public void closeWwiseScene()
    {
        Stereo_User_Pan.SetGlobalValue(0.5f);
        Stereo_User_Wet_Level.SetGlobalValue(0.5f);
        Stereo_User_Reverb_Length.SetGlobalValue(0.5f);

        writeResults();
    }

    public void writeResults()
    {
        // This is where the RTPC value and event name will be returned
    }

    public void postStereoWwiseEvent(string eventName)
    {
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
        else if (eventName == "Pink_Noise")
        {
            Play_Pink_Noise.Post(stereoUserEmitter);
        }
    }
    
    public void postSpatialWwiseEvent(string eventName)
    {
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

            //// User
            //Play_User_Jethro_Tull_Mother_Goose_L.Post(leftEmitter);
            //Play_User_Jethro_Tull_Mother_Goose_R.Post(rightEmitter);
            //Play_User_Jethro_Tull_Mother_Goose_Sub.Post(subEmitter);
        }
    }
}
