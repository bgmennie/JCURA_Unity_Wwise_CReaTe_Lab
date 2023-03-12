using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using MidiJack;

public class ScreenManager : MonoBehaviour
{
    private float mk2ContinuePressed;
    private float mk3ContinuePressed;

    // The amount of time needed before the next screen can be loaded
    private float screenLoadDelayTimer;

    public int currentScreen;
    public int nextScreen;

    [Header("Debug Settings")]
    public bool firstScreenVisible = true;

    [Header("MIDI Settings")]
    public int midiChannel = 1;
    
    public int mk2MidiPad1 = 48;
    
    public int mk3MidiPad1 = 40;
    public int mk3MidiPad2 = 41;
    public int mk3MidiPad3 = 42;
    public int mk3MidiPad4 = 43;
    public int mk3MidiPad5 = 36;
    public int mk3MidiPad6 = 37;
    public int mk3MidiPad7 = 38;
    public int mk3MidiPad8 = 39;

    public float screenLoadDelay = 0.5f;

    [Header("Screen Indices")]
    public int loudspeakerTestDescription;
    public int hpSystem1TestDescription;
    public int hpSystem2TestDescription;

    [SerializeField]
    private Screen[] m_Screens;
    [SerializeField]
    private Screen[] reverb_Screens;
    [SerializeField]
    private Screen[] gain_Match_Screens;
    [SerializeField]
    private Screen[] panning_Screens;

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
        if (firstScreenVisible)
        {
            m_Screens[0].Open();
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
        mk2ContinuePressed = MidiMaster.GetKey(mk2MidiPad1);
        mk3ContinuePressed = MidiMaster.GetKey(mk3MidiPad8);

        Debug.Log("mk2ContinuePressed: " + mk2ContinuePressed);
        Debug.Log("mk3ContinuePressed: " + mk3ContinuePressed);

        // Continue to the next screen if the top-left MIDI pad has been selected
        if (screenLoadDelayTimer >= screenLoadDelay)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || 
                mk2ContinuePressed > 0.0 || mk3ContinuePressed > 0.0)
            {
                loadNextScreen();
                screenLoadDelayTimer = 0.0f;
            }
        }
    }

    void loadNextScreen()
    {
        m_Screens[currentScreen].Close();
        m_Screens[nextScreen].Open();

        if (nextScreen < m_Screens.Length-1)
        {
            currentScreen++;
            nextScreen++;
        }
    }
}
