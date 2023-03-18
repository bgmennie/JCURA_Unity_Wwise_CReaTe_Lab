using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWwiseManager : MonoBehaviour
{
    [Header("Spatial WwiseEvents")]
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_L;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_R;
    public AK.Wwise.Event Play_Reference_Jethro_Tull_Mother_Goose_Sub;

    [Header("Spatial WwiseEmitters")]
    public GameObject leftEmitter;
    public GameObject rightEmitter;
    public GameObject subEmitter;

    // Start is called before the first frame update
    void Start()
    {
        Play_Reference_Jethro_Tull_Mother_Goose_L.Post(leftEmitter);
        Play_Reference_Jethro_Tull_Mother_Goose_R.Post(rightEmitter);
        Play_Reference_Jethro_Tull_Mother_Goose_Sub.Post(subEmitter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
