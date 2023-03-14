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
        WwiseManager.wwiseManagerSingleton.postWwiseEvent("Stop_All");
    }

    public override void Open(int stereoSpatialFlag, string testType)
    {
        base.Open();
        //Debug.Log("This is an AudioScreen");

        // Stereo functionality
        if (stereoSpatialFlag == 0)
        {
            if (testType == "Pan")
            {

            }
            else if (testType == "Reverb")
            {

            }
            else if (testType == "Gain")
            {

            }
            else if (testType == "VolumeSetup")
            {
                WwiseManager.wwiseManagerSingleton.postWwiseEvent(new string[] { "stereoEmitter" });
            }
            else
            {
                Debug.LogWarning("AudioScreen Open: Invalid value for testType");
            }
        } 
        else if (stereoSpatialFlag == 1)
        {
            if (testType == "Pan")
            {

            }
            else if (testType == "Reverb")
            {

            }
            else if (testType == "Gain")
            {

            }
            else
            {
                Debug.LogWarning("AudioScreen Open: Invalid value for testType");
            }
            //WwiseManager.wwiseManagerSingleton.postWwiseEvent(new string[] {"leftEmitter", "rightEmitter", "subEmitter"});

        }
    }
}
