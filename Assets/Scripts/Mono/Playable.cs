using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Playable : MonoBehaviour
{
    public enum States { selected, unselected }
    public States state;

    private Rigidbody rb;
    private LineRenderer lineRenderer;

    public bool isSelected;

    private Icon icon;
    private SacredIcon sacredIcon;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        icon = GetComponent<Icon>();
        lineRenderer = GetComponent<LineRenderer>();
        sacredIcon = GetComponent<SacredIcon>();
    }

    // Main Loop Methods

    private void FixedUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
        if (state == States.selected)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
        else if (state == States.unselected)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    // State Methods 

    public void Selected()
    {
        isSelected = true;
        Stage.instance.Play(Data.instance.IconSelectionClip());
        if (icon != null) { icon.Selected(); }
        if (sacredIcon != null) { sacredIcon.Selected(); }
    }
    public void Unselected()
    {
        isSelected = false;
        Stage.instance.Play(Data.instance.UnselectionClip()); 
        if (icon != null) { icon.Unselected(); }
        if (sacredIcon != null) { sacredIcon.Unselected(); }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    // Action Methods

    public void AddForce(Vector3 vector)
    {
        rb.AddForce(vector);
    }

    // Input Methods

    private void OnMouseDown()
    {
        Stage.instance.Clicked(this);
    }

    // Get Set Methods

    public void LineRendererMaterial(Material newMaterial) { lineRenderer.material = newMaterial; }
    public LineRenderer LineRenderer() { return lineRenderer; }

}
