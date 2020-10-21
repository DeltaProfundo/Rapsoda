using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Repeller : MonoBehaviour
{
    public Vector3 direction;
    public float strength;
    public ForceMode forceMode;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody affectedBody = other.gameObject.GetComponent<Rigidbody>();
        if (affectedBody != null) { affectedBody.AddForce(direction.normalized * strength * Time.deltaTime, forceMode); }
    }

}
