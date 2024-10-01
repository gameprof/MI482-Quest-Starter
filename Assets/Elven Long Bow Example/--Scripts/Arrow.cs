using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour {
    public Rigidbody rigid;
    void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    public Vector3 vel {
        get { return rigid.velocity; }
        set { rigid.velocity = value; }
    }

    public Vector3 forward => transform.forward;

    public bool kinematic {
        get { return rigid.isKinematic; }
        set { rigid.isKinematic = value; }

    }
}

