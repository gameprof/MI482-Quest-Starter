using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Drone : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform  launchPoint;
    public Transform  target;
    [MinMaxSlider( 1, 100 )]
    public Vector2Int volleyCount = new Vector2Int(1, 5);
    [MinMaxSlider( .1f, 30f )]
    public Vector2  volleyDelay = new Vector2(10, 15);
    [MinMaxSlider( 0f, 1f )]
    public Vector2 rocketDelay = new Vector2( 0f, 0.2f );

    public bool keepLaunching = true; 

    void Start() {
        StartCoroutine( LaunchVolley() );
    }

    IEnumerator LaunchVolley() {
        while ( keepLaunching ) {
            yield return new WaitForSeconds( Random.Range( volleyDelay.x, volleyDelay.y ) );
            
            int count = Random.Range( volleyCount.x, volleyCount.y );
            for ( int i = 0; i < count; i++ ) {
                GameObject projectile = Instantiate( projectilePrefab ); // Pos and Rot are set in the Launch() method
                DroneProjectile droneProjectile = projectile.GetComponent<DroneProjectile>();
                droneProjectile.Launch( new Ray( launchPoint.position,  launchPoint.forward ), target.position );
                yield return new WaitForSeconds( Random.Range( rocketDelay.x, rocketDelay.y ) );
            }
        }
    }
}
