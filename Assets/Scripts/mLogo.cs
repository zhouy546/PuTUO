using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlapState {
Front,Back
}

public class mLogo : MonoBehaviour
{
    public Transform m_transform;
    public FlapState flapState = FlapState.Back;

    void Awake() {
        m_transform = this.GetComponent<Transform>();
    }
    
}
