using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBowScript : MonoBehaviour {
    static public Transform TRANS;
    
    [Header("Inscribed")]
    public OVRRuntimeController ovrRuntimeController;
    
    [Header("Dynamic")]
    public OVRInput.Controller  controller;
    public bool  X, Y;
    [Range(0,1)]
    public float trigger, grip, thumbX, thumbY;
    public Vector2 thumbStick;
    
    
    // Start is called before the first frame update
    void Start() {
        TRANS = transform;
        controller = ovrRuntimeController.m_controller;
    }

    void Update() {
        X = OVRInput.Get( OVRInput.Button.One, controller );
        Y = OVRInput.Get( OVRInput.Button.Two, controller );
        
        trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        grip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        
        Vector2 thumbStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
        thumbX = thumbStick.x;
        thumbY = thumbStick.y;
    }
}

/*
            m_animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, m_controller) ? 1.0f : 0.0f);
            m_animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, m_controller) ? 1.0f : 0.0f);
            m_animator.SetFloat("Button 3", OVRInput.Get(OVRInput.Button.Start, m_controller) ? 1.0f : 0.0f);

            m_animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).x);
            m_animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).y);

            m_animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller));
            m_animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller));
*/