using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickVR;

public class DrawHand : MonoBehaviour {
    static public DrawHand _S;
    [Header( "Inscribed" )]
    public GameObject[] gameObjectsToDisable; 
    
    [Header("Dynamic")]
    [SerializeField]
    private bool _shieldActive = false;

    void Awake() {
        _S = this;
    }

    void Update() {
        shieldActive = QuVR.RGrip > 0.9f;
    }

    public bool shieldActive {
        get { return _shieldActive; }
        private set {
            if ( _shieldActive != value ) {
                _shieldActive = value;
                foreach ( GameObject go in gameObjectsToDisable ) {
                    go.SetActive(_shieldActive);
                }
            }
        }
    }

    static public bool SHIELD_ACTIVE {
        get {
            if ( _S == null ) return false;
            return _S._shieldActive;
        }
    }
}