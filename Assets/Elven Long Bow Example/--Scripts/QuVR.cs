using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[DefaultExecutionOrder(-99)] // This should happen before other scripts
public class QuVR : MonoBehaviour {
    static public QuVR      S;
    static public Transform L_TRANS;
    static public Transform R_TRANS;
    static public Transform H_TRANS;

    [Required("Assign the CenterEyeAnchor to this")]
    public Transform headAnchor;
    
    [BoxGroup( "Left Hand" )] public Transform           leftHandAnchor;
    [BoxGroup( "Left Hand" )] public OVRInput.Controller leftController;
    [Header("Dynamic Controller State")]
    [BoxGroup( "Left Hand" )] public bool x;
    [BoxGroup( "Left Hand" )] public bool y;
    [BoxGroup( "Left Hand" )] public bool menu;
    [Range(0,1)]
    [BoxGroup( "Left Hand" )] public float lTrigger, lGrip;
    [Range(-1,1)]
    [BoxGroup( "Left Hand" )] public float lThumbX, lThumbY;
    [BoxGroup( "Left Hand" )] public Vector2 lThumbStick; 

    
    [BoxGroup( "Right Hand" )] public Transform           rightHandAnchor;
    [BoxGroup( "Right Hand" )] public OVRInput.Controller rightController;
    [Header("Dynamic Controller State")]
    [BoxGroup( "Right Hand" )] public bool a;
    [BoxGroup( "Right Hand" )] public bool b;
    [BoxGroup( "Right Hand" )] public bool meta;
    [Range(0,1)]
    [BoxGroup( "Right Hand" )] public float rTrigger, rGrip;
    [Range(-1,1)]
    [BoxGroup( "Right Hand" )] public float rThumbX, rThumbY;
    [BoxGroup( "Right Hand" )] public Vector2 rThumbStick; 
    
    
    void Start() {
        S = this;
        L_TRANS = leftHandAnchor;
        R_TRANS = rightHandAnchor;
        H_TRANS = headAnchor;
    }

    void Update() {
        // Left Hand
        x = OVRInput.Get( OVRInput.Button.One, leftController );
        y = OVRInput.Get( OVRInput.Button.Two, leftController );
        
        lTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController);
        lGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, leftController);
        
        lThumbStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, leftController);
        lThumbX = lThumbStick.x;
        lThumbY = lThumbStick.y;
        
        
        // Right Hand
        a = OVRInput.Get( OVRInput.Button.One, rightController );
        b = OVRInput.Get( OVRInput.Button.Two, rightController );
        
        rTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);
        rGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, rightController);
        
        rThumbStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, rightController);
        rThumbX = rThumbStick.x;
        rThumbY = rThumbStick.y;
    }

    // Static read-only accessors for all contols
    public static bool X => S.x;
    public static bool Y => S.y;
    public static bool A => S.a;
    public static bool B => S.b;
    public static bool Menu => S.menu;
    public static bool Meta => S.meta;

    public static float LTrigger => S.lTrigger;
    public static float LGrip => S.lGrip;
    public static float LThumbX => S.lThumbX;
    public static float LThumbY => S.lThumbY;
    public static Vector2 LThumbStick => S.lThumbStick;
    
    public static float RTrigger =>  S.rTrigger;
    public static float RGrip =>  S.rGrip;
    public static float RThumbX =>  S.rThumbX;
    public static float RThumbY =>  S.rThumbY;
    public static Vector2 RThumbStick =>  S.rThumbStick;
    
    // Static read-only accessors for transform information
    public static Vector3 LPos => L_TRANS.position;
    public static Vector3 RPos => R_TRANS.position;
    public static Vector3 HPos => H_TRANS.position;
    public static Quaternion LRot => L_TRANS.rotation;
    public static Quaternion RRot => R_TRANS.rotation;
    public static Quaternion HRot => H_TRANS.rotation;
    public static Vector3 LEuler => L_TRANS.eulerAngles;
    public static Vector3 REuler => R_TRANS.eulerAngles;
    public static Vector3 HEuler => H_TRANS.eulerAngles;
    
}
