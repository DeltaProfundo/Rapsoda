using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start() { initialRotation = transform.rotation; }

    void Update() { transform.rotation = initialRotation; }
}
