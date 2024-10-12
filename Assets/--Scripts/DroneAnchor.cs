using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnchor : MonoBehaviour {
    public float maxRot    = 90;
    public float cyclePeriod = 10;
    
    void Update()
    {
        float yRot = Mathf.Sin( Time.time * Mathf.PI * 2f / cyclePeriod) * maxRot;
        transform.localRotation = Quaternion.Euler( 0, yRot, 0 );    
    }
}
