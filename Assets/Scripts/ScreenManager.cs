using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using MidiJack;

public class ScreenManager : MonoBehaviour
{

    // The amount of time needed before the next screen can be loaded
    private float screenLoadDelayTimer;

    public int currentScreen;
    public int nextScreen;

    [Header("Debug Settings")]
    public bool firstScreenVisible = true;

    [Header("MIDI Settings")]
    public MidiChannel midiChannel = MidiChannel.Ch1;

    public int mk3MidiPad1 = 40;
    private float mk3MidiPad1Value;
    
    public int mk3MidiPad2 = 41;
    
    public int mk3MidiPad3 = 42;
    
    public int mk3MidiPad4 = 43;
    
    public int mk3MidiPad5 = 36;
    
    public int mk3MidiPad6 = 37;
    
    public int mk3MidiPad7 = 38;
    
    public int mk3MidiPad8 = 39;
    private float mk3ContinuePressed;

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

    public float screenLoadDelay = 0.5f;

    [Header("Screen Indices")]
    public int volumeSetupIndex = 6;
    public int loudspeakerTestDescriptionScreenIndex = 7;
    public int hpSystem1TestDescriptionScreenIndex = 8;
    public int hpSystem2TestDescriptionScreenIndex = 9;
    public int exitScreenIndex = 11;

    private int testScreenTypeOrderIndex;

    [SerializeField]
    private Screen[] openExitDescScreens;
    [SerializeField]
    private Screen[] panningScreens;
    [SerializeField]
    private Screen[] reverbScreens;
    [SerializeField]
    private Screen[] gainMatchScreens;

    private Screen[] allScreens;
    private int[] testScreenTypeOrder;

    public static ScreenManager s_ScreenManagerSingleton;

    private void Awake()
    {
        s_ScreenManagerSingleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        screenLoadDelayTimer = 0.0f;
        SceneManager.LoadScene("Basic_Room_Spatial_Demo_w_Headtracking", LoadSceneMode.Additive);
        initializeAllScreens();
        if (firstScreenVisible)
        {
            //Debug.Log("openExitDescScreens Length: " + openExitDescScreens.Length);
            allScreens[0].Open();
            currentScreen = 0;
            nextScreen = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        screenLoadDelayTimer += Time.deltaTime;
        midiMapManager();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void midiMapManager()
    {
        mk3ContinuePressed = MidiMaster.GetKey(mk3MidiPad8);

        // Continue to the next screen if the bottom-right MIDI pad has been selected
        if (screenLoadDelayTimer >= screenLoadDelay)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
                mk3ContinuePressed > 0.0)
            {
                loadNextScreen();
                screenLoadDelayTimer = 0.0f;
            }
        }
    }

    void initializeAllScreens()
    {
        int numOfTestScreens = panningScreens.Length + reverbScreens.Length + gainMatchScreens.Length;
        int numOfScreens = openExitDescScreens.Length + (numOfTestScreens * 3);

        allScreens = new Screen[numOfScreens];
        testScreenTypeOrder = new int[numOfTestScreens*3];

        int panningScreenIndex = 0;
        int reverbScreenIndex = 0;
        int gainMatchScreenIndex = 0;
        int openExitDescScreenIndex = 0;

        testScreenTypeOrderIndex = 0;

        hpSystem1TestDescriptionScreenIndex += numOfTestScreens;
        hpSystem2TestDescriptionScreenIndex += numOfTestScreens * 2;
        exitScreenIndex += numOfTestScreens * 3;

        bool screenAdded;

        for (int screenIndex = 0; screenIndex < numOfScreens; screenIndex++)
        {
            screenAdded = false;

            if (screenIndex <= loudspeakerTestDescriptionScreenIndex)
            {
                allScreens[screenIndex] = openExitDescScreens[openExitDescScreenIndex];
                openExitDescScreenIndex++;
            }
            else if (screenIndex > loudspeakerTestDescriptionScreenIndex && screenIndex < hpSystem1TestDescriptionScreenIndex)
            {
                int testType = Random.Range(0, 3); // Selects a random test: 0 = pan; 1 = reverb; 2 = gain match
                while (!screenAdded)
                {
                    if (testType == 0 && panningScreenIndex < panningScreens.Length)
                    {
                        allScreens[screenIndex] = panningScreens[panningScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 0;
                        testScreenTypeOrderIndex++;
                        panningScreenIndex++;
                        screenAdded = true;
                    }
                    else if (testType == 1 && reverbScreenIndex < reverbScreens.Length)
                    {
                        allScreens[screenIndex] = reverbScreens[reverbScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 1;
                        testScreenTypeOrderIndex++;
                        reverbScreenIndex++;
                        screenAdded = true;
                    }
                    else if (testType == 2 && gainMatchScreenIndex < gainMatchScreens.Length)
                    {
                        allScreens[screenIndex] = gainMatchScreens[gainMatchScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 2;
                        testScreenTypeOrderIndex++;
                        gainMatchScreenIndex++;
                        screenAdded = true;
                    }
                    else
                    {
                        testType = (testType + 1) % 3;
                    }
                }
            }
            else if (screenIndex == hpSystem1TestDescriptionScreenIndex)
            {
                panningScreenIndex = 0;
                reverbScreenIndex = 0;
                gainMatchScreenIndex = 0;
                allScreens[screenIndex] = openExitDescScreens[openExitDescScreenIndex];
                openExitDescScreenIndex++;
            }
            else if (screenIndex > hpSystem1TestDescriptionScreenIndex && screenIndex < hpSystem2TestDescriptionScreenIndex)
            {
                int testType = Random.Range(0, 3); // Selects a random test: 0 = pan; 1 = reverb; 2 = gain match
                while (!screenAdded)
                {
                    if (testType == 0 && panningScreenIndex < panningScreens.Length)
                    {
                        allScreens[screenIndex] = panningScreens[panningScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 0;
                        testScreenTypeOrderIndex++;
                        panningScreenIndex++;
                        screenAdded = true;
                    }
                    else if (testType == 1 && reverbScreenIndex < reverbScreens.Length)
                    {
                        allScreens[screenIndex] = reverbScreens[reverbScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 1;
                        testScreenTypeOrderIndex++;
                        reverbScreenIndex++;
                        screenAdded = true;
                    }
                    else if (testType == 2 && gainMatchScreenIndex < gainMatchScreens.Length)
                    {
                        allScreens[screenIndex] = gainMatchScreens[gainMatchScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 2;
                        testScreenTypeOrderIndex++;
                        gainMatchScreenIndex++;
                        screenAdded = true;
                    }
                    else
                    {
                        testType = (testType + 1) % 3;
                    }
                }
            }
            else if (screenIndex == hpSystem2TestDescriptionScreenIndex || screenIndex == hpSystem2TestDescriptionScreenIndex + 1)
            {
                panningScreenIndex = 0;
                reverbScreenIndex = 0;
                gainMatchScreenIndex = 0;
                allScreens[screenIndex] = openExitDescScreens[openExitDescScreenIndex];
                openExitDescScreenIndex++;
            }
            else if (screenIndex > hpSystem2TestDescriptionScreenIndex + 1 && screenIndex < exitScreenIndex)
            {
                int testType = Random.Range(0, 3); // Selects a random test: 0 = pan; 1 = reverb; 2 = gain match
                while (!screenAdded)
                {
                    if (testType == 0 && panningScreenIndex < panningScreens.Length)
                    {
                        allScreens[screenIndex] = panningScreens[panningScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 0;
                        testScreenTypeOrderIndex++;
                        panningScreenIndex++;
                        screenAdded = true;
                    }
                    else if (testType == 1 && reverbScreenIndex < reverbScreens.Length)
                    {
                        allScreens[screenIndex] = reverbScreens[reverbScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 1;
                        testScreenTypeOrderIndex++;
                        reverbScreenIndex++;
                        screenAdded = true;
                    }
                    else if (testType == 2 && gainMatchScreenIndex < gainMatchScreens.Length)
                    {
                        allScreens[screenIndex] = gainMatchScreens[gainMatchScreenIndex];
                        testScreenTypeOrder[testScreenTypeOrderIndex] = 2;
                        testScreenTypeOrderIndex++;
                        gainMatchScreenIndex++;
                        screenAdded = true;
                    }
                    else
                    {
                        testType = (testType + 1) % 3;
                    }
                }
            }
            else if (screenIndex == exitScreenIndex)
            {
                allScreens[screenIndex] = openExitDescScreens[openExitDescScreenIndex];
            }
        }
        testScreenTypeOrderIndex = 0;
    }

    void loadNextScreen()
    {

        if (nextScreen <= loudspeakerTestDescriptionScreenIndex && nextScreen != volumeSetupIndex)
        {
            allScreens[currentScreen].Close();
            allScreens[nextScreen].Open();
            currentScreen++;
            nextScreen++;
        }
        else if (nextScreen == volumeSetupIndex)
        {
            allScreens[currentScreen].Close();
            allScreens[nextScreen].Open(0, "VolumeSetup");
            currentScreen++;
            nextScreen++;
        }
        else if (nextScreen > loudspeakerTestDescriptionScreenIndex && nextScreen < hpSystem1TestDescriptionScreenIndex)
        {
            if (testScreenTypeOrder[testScreenTypeOrderIndex] == 0)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(0, "Pan");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
            else if (testScreenTypeOrder[testScreenTypeOrderIndex] == 1)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(0, "Reverb");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
            else if (testScreenTypeOrder[testScreenTypeOrderIndex] == 2)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(0, "Gain");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
        }
        else if (nextScreen == hpSystem1TestDescriptionScreenIndex)
        {
            allScreens[currentScreen].Close();
            allScreens[nextScreen].Open();
            currentScreen++;
            nextScreen++;
        }
        else if (nextScreen > hpSystem1TestDescriptionScreenIndex && nextScreen < hpSystem2TestDescriptionScreenIndex)
        {
            if (testScreenTypeOrder[testScreenTypeOrderIndex] == 0)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(0, "Pan");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
            else if (testScreenTypeOrder[testScreenTypeOrderIndex] == 1)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(0, "Reverb");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
            else if (testScreenTypeOrder[testScreenTypeOrderIndex] == 2)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(0, "Gain");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
        }
        else if (nextScreen == hpSystem2TestDescriptionScreenIndex || nextScreen == hpSystem2TestDescriptionScreenIndex + 1)
        {
            allScreens[currentScreen].Close();
            allScreens[nextScreen].Open();
            currentScreen++;
            nextScreen++;
        }
        else if (nextScreen > hpSystem2TestDescriptionScreenIndex + 1 && nextScreen < exitScreenIndex)
        {
            if (testScreenTypeOrder[testScreenTypeOrderIndex] == 0)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(1, "Pan");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
            else if (testScreenTypeOrder[testScreenTypeOrderIndex] == 1)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(1, "Reverb");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
            else if (testScreenTypeOrder[testScreenTypeOrderIndex] == 2)
            {
                allScreens[currentScreen].Close();
                allScreens[nextScreen].Open(1, "Gain");
                testScreenTypeOrderIndex++;
                currentScreen++;
                nextScreen++;
            }
        }
        else if (nextScreen == exitScreenIndex)
        {
            allScreens[currentScreen].Close();
            allScreens[nextScreen].Open();
        }
    }
}
