using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Avatar : MonoBehaviour
{
    public Traveler traveler;

    public Transform headRig;
    public Transform bodyRig;

    private MeshRenderer[] meshRenderers;

    public void Setup(Traveler newTraveler)
    {
        traveler = newTraveler;
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material = traveler.Material();
        }
    }
    public void ClearRigs()
    {
        foreach(Transform child in headRig) { Destroy(child.gameObject); }
        foreach (Transform child in bodyRig) { Destroy(child.gameObject); }
    }

    private void OnMouseEnter() 
    {
        Debug.Log("Mouse over avatar");
        Stage.instance.MouseOver(this); 
    }
    private void OnMouseExit() { Stage.instance.MouseExit(); }

    public Traveler Traveler() { return traveler; }
}
