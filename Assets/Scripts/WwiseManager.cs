using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseManager : MonoBehaviour
{
    public static WwiseManager wwiseManagerSingleton;

    [SerializeField]
    private int m_TunableParameter = 0;

    // TODO: A more flexible event management system
    [Header("Spatial WwiseEvents")]
    public AK.Wwise.Event Play_Jethro_Tull_L;
    public AK.Wwise.Event Play_Jethro_Tull_R;
    public AK.Wwise.Event Play_Jethro_Tull_Sub;
    public AK.Wwise.Event Play_Jethro_Tull_L_Spatial;
    public AK.Wwise.Event Play_Jethro_Tull_R_Spatial;
    public AK.Wwise.Event Play_Jethro_Tull_Sub_Spatial;

    [Header("Spatial WwiseEmitters")]
    public GameObject leftEmitter;
    public GameObject rightEmitter;
    public GameObject subEmitter;
    
    [Header("Stereo WwiseEvents")]

    [Header("Stereo WwiseEmitters")]
    public GameObject stereoEmitter;

    private void Awake()
    {
        if(wwiseManagerSingleton != null)
        {
            Debug.Log("There's a problem, more than one singleton");
        }
        wwiseManagerSingleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadNewWwiseScene()
    {
        m_TunableParameter++;
    }

    // TODO: An overloaded method that allows for events to be specified
    // from the scene manager
    public void postWwiseEvent(string[] emitters)
    {
        //foreach (string emitter in emitters)
        //{
        //    if (emitter == "leftEmitter")
        //    {
        //        Play_Jethro_Tull_L.Post(leftEmitter);
        //    }
        //    if (emitter == "rightEmitter")
        //    {
        //        Play_Jethro_Tull_R.Post(rightEmitter);
        //    }
        //    if (emitter == "subEmitter")
        //    {
        //        Play_Jethro_Tull_Sub.Post(subEmitter);
        //    }
        //}
        foreach (string emitter in emitters)
        {
            if (emitter == "leftEmitter")
            {
                Play_Jethro_Tull_L_Spatial.Post(leftEmitter);
            }
            if (emitter == "rightEmitter")
            {
                Play_Jethro_Tull_R_Spatial.Post(rightEmitter);
            }
            if (emitter == "subEmitter")
            {
                Play_Jethro_Tull_Sub_Spatial.Post(subEmitter);
            }
        }
    }
}
