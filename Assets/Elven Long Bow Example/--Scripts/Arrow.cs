using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour {
    public bool  drawEntryDepthDebug    = false;
    public float speedForFullEntryDepth = 100;
    [MinMaxSlider( 0, 1 )]
    public Vector2 entryDepthMinMax = new Vector2(0.1f, 0.9f);
    public LayerMask     targetMask;
    [Range(2,6)]
    public int           prevPosFrames = 3;
    public List<Vector3> prevPosList;
    public float         minRicochetVel        = 20;
    public int           maxRicochetCollisions = 3;
    public float         maxLifeTimeSeconds    = 60;
    
    private Rigidbody     rigid;
    private Transform     trans;
    private float         birthTime;
    private TrailRenderer tRend;
    
    void Awake() {
        rigid = GetComponent<Rigidbody>();
        tRend = GetComponent<TrailRenderer>();
        tRend.enabled = false;
        trans = transform;
        prevPosList = new List<Vector3>();
        birthTime = Time.time;
    }

    public Vector3 vel {
        get { return rigid.velocity; }
        set { rigid.velocity = value; }
    }

    public Vector3 forward => transform.forward;

    public bool kinematic {
        get { return rigid.isKinematic; }
        set {
            rigid.isKinematic = value;
            if ( !value ) {
                tRend.enabled = true;
            }
        }

    }

    private void FixedUpdate() {
        if ( kinematic ) return;

        if ( Time.fixedTime - birthTime > maxLifeTimeSeconds ) {
            if ( rigid.IsSleeping() ) {
                kinematic = true;
            } else {
                Destroy( gameObject );
            }
        }
        
        // The newest pos is at [^1]
        prevPosList.Add( transform.position );
        if ( prevPosList.Count > prevPosFrames ) {
            // The oldest pos is at [0]
            prevPosList.RemoveAt( 0 );
            if ( collisionCount < maxRicochetCollisions ) {
                // Only force alignment with velocity if it is higher than minAlignmentVel
                trans.LookAt( trans.position + ( trans.position - prevPosList[0] ) );
            }
        }
    }

    private float triggerTime = 0;
    void OnTriggerEnter(Collider coll) {
        // Avoid multiple calls to OnTriggerEnter (because Targets have multiple colliders)
        if ( Mathf.Approximately(triggerTime,  Time.fixedTime) ) return;
        triggerTime = Time.fixedTime;
        // Raycast from the point before impact
        RaycastHit hit;
        Vector3 nextPos = trans.position;
        if ( Physics.Raycast( prevPosList[0], transform.forward, out hit,
                ( nextPos - prevPosList[0] ).magnitude*1.5f,  targetMask,
                QueryTriggerInteraction.Collide ) ) {
            // We hit a target!
            Vector3 hitPoint = hit.point;
            // Calculate depth of arrow.
            float velPct = 1 - (rigid.velocity.magnitude / speedForFullEntryDepth);
            velPct = Mathf.Lerp( entryDepthMinMax.x, entryDepthMinMax.y, velPct );
            // This assumes that the arrow is 1m long
            Vector3 nockPos = hitPoint - transform.forward * velPct;
            trans.position = nockPos;
            if (drawEntryDepthDebug) Debug.DrawLine( prevPosList[0], nockPos, Color.green, 1000 );
            if (drawEntryDepthDebug) Debug.DrawLine( nockPos, nextPos, Color.yellow, 1000 );
        } else {
            if (drawEntryDepthDebug) Debug.DrawLine( prevPosList[^2], nextPos, Color.red, 1000 );
        }
        
        kinematic = true;
    }

    private int collisionCount = 0;
    void OnCollisionEnter( Collision coll ) {
        if ( coll.gameObject.layer == LayerMask.NameToLayer( "Target" ) ) {
            OnTriggerEnter( coll.collider );
            return;
        }
        
        collisionCount++;
        if ( collisionCount > maxRicochetCollisions && !kinematic ) {
            rigid.velocity *= 0.5f;
        }
        // Remove points from the prevPosList to prevent rotating arrow right around hits
        while ( prevPosList.Count > 1 ) {
            prevPosList.RemoveAt( 0 );
        }
        // Check for minimum ricochet velocity and don't ricochet any more if it's below that.
        if ( rigid.velocity.magnitude < minRicochetVel ) {
            collisionCount = maxRicochetCollisions + 1;
        }
        
    }
}

