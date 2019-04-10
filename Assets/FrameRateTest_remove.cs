using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateTest_remove : MonoBehaviour {

	// Use this for initialization
 public int target = 15;
     
    void Awake()
    {
         QualitySettings.vSyncCount = 2;
         Application.targetFrameRate = target;
    }
     
     void Update()
    {
         if(Application.targetFrameRate != target)
             Application.targetFrameRate = target;
    }
}
