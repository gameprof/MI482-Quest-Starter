using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class DroneProjectile : MonoBehaviour {
    [Header( "Inscribed" )]
    public float avgSpeed;
    public float p1SpeedMult          = 4;
    [FormerlySerializedAs( "p1RadiusMult" )]
    public float p2RadiusMult         = 2;
    [FormerlySerializedAs( "p2DistFromTargetMult" )]
    public float p3DistFromTargetMult = 2;
    public float targetMissRadius     = 1;
    [Range(1,2)]
    public float uOvershoot           = 1.1f;

    public enum ePathMode { fourPoint, fivePoint };
    public ePathMode pathMode = ePathMode.fivePoint;
    
    [Header("Dynamic")]
    public float     duration;
    public Vector3[] points;
    
    private float     timeStart;
    
    
    public void Launch(Ray launchRay, Vector3 target) {
        timeStart = Time.time;
        // Determine points
        Vector3 p1 = launchRay.origin + launchRay.direction * avgSpeed * p1SpeedMult;
        Vector3 p2 = Random.insideUnitSphere;
        p2 = p2 - ( launchRay.direction * Vector3.Dot( launchRay.direction, p2 ) );
        p2 *= p2RadiusMult;
        p2 += p1;
        Vector3 p3 = target + (launchRay.origin - target).normalized * avgSpeed * p3DistFromTargetMult;
        Vector3 p4 = target + Random.insideUnitSphere * targetMissRadius;

        float dist;
        if ( pathMode == ePathMode.fourPoint ) {
            points = new[] { launchRay.origin, p2, p3, p4 };
            
            // Determine duration
            /* float dist = Vector3.Distance( points[0], points[1] )
                         + Vector3.Distance( points[1], points[2] )
                         + Vector3.Distance( points[2], points[3] ); */
            // Calculate a partial distance to the target
            dist = Vector3.Distance( points[0], points[1] )
                         + Vector3.Distance( points[1], points[3] );
        }  else {
            points = new[] { launchRay.origin, p1, p2, p3, p4 };
            
            // Determine duration
            /* float dist = Vector3.Distance( points[0], points[1] )
                         + Vector3.Distance( points[1], points[2] )
                         + Vector3.Distance( points[2], points[3] )
                         + Vector3.Distance( points[3], points[4] ); */
            // Calculate a partial distance to the target
            dist = Vector3.Distance( points[0], points[2] )
                         + Vector3.Distance( points[2], points[4] );
        }
        
        
        duration = dist / avgSpeed;

        FixedUpdate(); // This forces alignment and positioning of the projectile in Launch()
    }

    private void FixedUpdate() {
        float u = (Time.fixedTime - timeStart) / duration;
        Vector3 pos = QuickDimArray<Vector3>.BezierCurve( u, points );
        Vector3 posToLookAt = QuickDimArray<Vector3>.BezierCurve( u + 0.01f, points );
        transform.position = pos;
        transform.LookAt( posToLookAt );
        if (u >= uOvershoot) {
            Destroy( gameObject );
        }
    }
}
