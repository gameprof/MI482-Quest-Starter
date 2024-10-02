using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickVR;


public class Bow : MonoBehaviour {
    
    [Header( "Inscribed" )]
    public GameObject arrowPrefab;
    public float     minDrawDistance = 0.25f;
    public float     maxDrawDistance = 1;
    public float     minShotSpeed    = 1;
    public float     maxShotSpeed    = 10;
    public Transform bowstringTop, bowstringBottom;
    
    public enum eState { none, drawing }
    [Header( "Dynamic" )]
    public eState state = eState.none;
    public Arrow currentArrow;
    public float drawDist;
    [Range(0,1)]
    public float drawPercent = 0;

    private LineRenderer lRend;

    void Awake() {
        lRend = GetComponent<LineRenderer>();
    }
    
    void Update() {
        switch ( state ) {
        case eState.none:
            lRend.enabled = false;
            if ( QuVR.RTrigger > 0.9f ) {
                drawDist = Vector3.Distance( QuVR.RPos, QuVR.LPos );
                // Only nock an arrow if the draw hand is close to the bow
                if ( drawDist < minDrawDistance ) {
                    state = eState.drawing;
                    GameObject go = Instantiate<GameObject>( arrowPrefab );
                    currentArrow = go.GetComponent<Arrow>();
                    NockArrow();
                }
            }
            break;

        case eState.drawing:
            lRend.enabled = true;
            NockArrow();
            DrawString();
            if ( QuVR.RTrigger < 0.9f ) {
                FireArrow();
                state = eState.none;
            }
            break;
        }
    }

    void NockArrow() {
        currentArrow.kinematic = true;
        currentArrow.transform.position = QuVR.RPos;
        currentArrow.transform.LookAt( QuVR.LPos, QuVR.L_TRANS.up );
        drawDist = Vector3.Distance( QuVR.RPos, QuVR.LPos );
        drawDist = Mathf.Clamp( drawDist, 0, maxDrawDistance );
        drawPercent = Mathf.InverseLerp(minDrawDistance, maxDrawDistance, drawDist);
    }

    void DrawString() {
        lRend.positionCount = 3;
        lRend.SetPositions( new Vector3[] {
            bowstringTop.position,
            QuVR.RPos,
            bowstringBottom.position
        }  );
    }

    void FireArrow() {
        currentArrow.kinematic = false;
        float shotSpeed = 0;
        if ( drawPercent >= 0 ) { // Only fire if bow is drawn enough
            shotSpeed = Mathf.Lerp( minShotSpeed, maxShotSpeed, drawPercent );
        }
        currentArrow.vel = currentArrow.forward * shotSpeed;

        // Debug.LogWarning( $"ShotSpeed: {shotSpeed}\tDrawPercent: {drawPercent}" );
        currentArrow = null;
    }

}
