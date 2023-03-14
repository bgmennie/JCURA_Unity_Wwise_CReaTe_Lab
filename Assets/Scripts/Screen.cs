using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    // Start is called before the first frame update

    public string screenType;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    // stereoSpatialFlag: 0 = stereo; 1 = spatial
    // testType: "Pan"; "Reverb"; "Gain"  
    public virtual void Open(int stereoSpatialFlag, string testType)
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
