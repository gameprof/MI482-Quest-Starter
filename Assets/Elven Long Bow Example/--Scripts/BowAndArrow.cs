using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BowAndArrow : MonoBehaviour {
    
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
    
    void Update() {
        switch ( state ) {
        case eState.none:
            if ( QuVR.RTrigger > 0.9f ) {
                state = eState.drawing;
                GameObject go = Instantiate<GameObject>( arrowPrefab );
                currentArrow = go.GetComponent<Arrow>();
                NockArrow();
            }
            break;

        case eState.drawing:
            NockArrow();
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
        drawPercent = drawDist / maxDrawDistance;
    }

    void FireArrow() {
        currentArrow.kinematic = false;
        float shotSpeed = ( 1 - drawPercent ) * minShotSpeed + drawPercent * maxShotSpeed;
        currentArrow.vel = currentArrow.forward * shotSpeed;
        Debug.Log( shotSpeed );
        currentArrow = null;
    }

}
