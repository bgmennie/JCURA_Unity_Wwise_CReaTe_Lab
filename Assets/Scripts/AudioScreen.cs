using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScreen : Screen
{
    // Functionality:
    // Events to post
    // Randomness handled on Unity side
    // Stereo/ spatialized cases
    // RTPCs to use (maybe have a flag for reverb vs. panning vs. gain match)

    // Input:
    // stereoSpatial flag (int)
    // testType (string): 
    // Pan
    // Reverb
    // Gain
    // VolumeSetup
    public override void Close()
    {
        base.Close();
        WwiseManager.wwiseManagerSingleton.closeWwiseScene();
        WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent("Stop_All");
        WwiseManager.wwiseManagerSingleton.postSpatialWwiseEvent("Stop_All");
        
    }

    public override void Open(int stereoSpatialFlag, string testType)
    {
        base.Open();
        //Debug.Log("This is an AudioScreen");

        // Stereo functionality
        if (stereoSpatialFlag == 0)
        {
            string selectedEvent = selectPostEvent();

            if (testType == "Pan")
            {
                WwiseManager.wwiseManagerSingleton.setTestType("Pan", 0);
                WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent(selectedEvent);
                //WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent("Jethro_Tull");
            }
            else if (testType == "Reverb")
            {
                WwiseManager.wwiseManagerSingleton.setTestType("Reverb", 0);
                WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent(selectedEvent);
                //WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent("Jethro_Tull");
            }
            else if (testType == "Gain")
            {
                WwiseManager.wwiseManagerSingleton.setTestType("Gain", 0);
                WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent(selectedEvent);
                //WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent("Jethro_Tull");
            }
            else if (testType == "VolumeSetup")
            {
                Debug.Log("VolumeSetup testType set");
                WwiseManager.wwiseManagerSingleton.setTestType("VolumeSetup", 0);
                WwiseManager.wwiseManagerSingleton.postStereoWwiseEvent("Pink_Noise");
            }
            else
            {
                Debug.LogWarning("AudioScreen Open: Invalid value for testType for stereo: " + testType);
            }
        } 
        else if (stereoSpatialFlag == 1)
        {
            WwiseManager.wwiseManagerSingleton.postSpatialWwiseEvent("Jethro_Tull");

            if (testType == "Pan")
            {
                WwiseManager.wwiseManagerSingleton.setTestType("Pan", 1);
                Debug.Log("Spatial Pan");

            }
            else if (testType == "Reverb")
            {
                WwiseManager.wwiseManagerSingleton.setTestType("Reverb", 1);
                Debug.Log("Spatial Reverb");
            }
            else if (testType == "Gain")
            {
                WwiseManager.wwiseManagerSingleton.setTestType("Gain", 1);
                Debug.Log("Spatial Gain");

            }
            else
            {
                Debug.LogWarning("AudioScreen Open: Invalid value for testType for spatial: " + testType);
            }
        }
        else
        {
            Debug.LogWarning("AudioScreen Open: Invalid value for stereoSpatialFlag: " + stereoSpatialFlag);
        }
    }

    public string selectPostEvent()
    {
        int trackSelect = Random.Range(0, 5);
        string postEvent = "";

        if (trackSelect == 0)
        {
            postEvent = "AG";
        }
        else if (trackSelect == 1)
        {
            postEvent = "Bass";
        }
        else if (trackSelect == 2)
        {
            postEvent = "Bassoon";
        }
        else if (trackSelect == 3)
        {
            postEvent = "Kick and Snare";
        }
        else if (trackSelect == 4)
        {
            postEvent = "Voice";
        }

        return postEvent;
    }
}
