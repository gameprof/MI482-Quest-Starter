using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandArrowScript : MonoBehaviour {
    static public Transform TRANS;

    [Header( "Controller Inscribed" )]
    public OVRRuntimeController ovrRuntimeController;
    private OVRInput.Controller controller;
    
    [Header( "Controller State" )]
    public bool                A, B;
    [Range(0,1)]
    public float trigger, grip, thumbX, thumbY;

    [Header( "Inscribed" )]
    public GameObject arrowPrefab;
    public float maxDrawDistance = 1;
    public float minShotSpeed    = 10;
    public float maxShotSpeed    = 100;
    
    public enum eState { none, drawing }
    [Header( "Dynamic" )]
    public eState state = eState.none;
    public Arrow currentArrow;
    public float drawDist;
    [Range(0,1)]
    public float drawPercent = 0;
    
    // Start is called before the first frame update
    void Start() {
        TRANS = transform;
        controller = ovrRuntimeController.m_controller;
    }

    void Update() {
        UpdateControls();

        switch ( state ) {
        case eState.none:
            if ( trigger > 0.9f ) {
                state = eState.drawing;
                GameObject go = Instantiate<GameObject>( arrowPrefab );
                currentArrow = go.GetComponent<Arrow>();
                NockArrow();
            }
            break;

        case eState.drawing:
            NockArrow();
            if ( trigger < 0.9f ) {
                FireArrow();
                state = eState.none;
            }
            break;
        }
    }

    void NockArrow() {
        currentArrow.kinematic = true;
        currentArrow.transform.position = transform.position;
        currentArrow.transform.LookAt( HandBowScript.TRANS, HandBowScript.TRANS.up );
        drawDist = Vector3.Distance( transform.position, HandBowScript.TRANS.position );
        drawDist = Mathf.Clamp( drawDist, 0, maxDrawDistance );
        drawPercent = drawDist / maxDrawDistance;
    }

    void FireArrow() {
        currentArrow.kinematic = false;
        float shotSpeed = ( 1 - drawPercent ) * minShotSpeed + drawPercent * maxShotSpeed;
        currentArrow.vel = currentArrow.forward * shotSpeed;
        Debug.Log( shotSpeed );
        currentArrow.transform.SetParent( null );
        currentArrow = null;
    }

    void UpdateControls() {
        A = OVRInput.Get( OVRInput.Button.One, controller );
        B = OVRInput.Get( OVRInput.Button.Two, controller );
        
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