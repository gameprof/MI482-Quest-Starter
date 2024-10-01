using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingTarget : MonoBehaviour
{
    void Awake() {
        GetComponent<Rigidbody>()?.Sleep();
    }
}
