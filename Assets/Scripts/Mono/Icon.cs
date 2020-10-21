using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Icon : MonoBehaviour
{
    public Episode episode;

    private Rigidbody rb;
    private Playable playableScript;
    public MeshRenderer meshRenderer;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem spawnEffect;
    private LineRenderer lineRenderer;
    public Light selectionLight;
    private SpringJoint springJoint;

    public Rigidbody boundTo;

    public Glyph glyph;
    public float initialPushForce;

    // Setup Methods

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playableScript = GetComponent<Playable>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        selectionLight = GetComponent<Light>();
    }
    private void Start()
    {
        rb.AddForce(RandomDirection() * initialPushForce);
        Stage.instance.Play(Data.instance.IconSpawnClip());
    }
    public void Setup(Glyph glyph)
    {
        this.glyph = glyph;
        if (glyph.Category() == global::Glyph.Categories.motivation) { meshRenderer.material = Data.instance.MotivationMaterial(); }
        else if (glyph.Category() == global::Glyph.Categories.situation) { meshRenderer.material = Data.instance.SituationMaterial(); }
        else if (glyph.Category() == global::Glyph.Categories.place) { meshRenderer.material = Data.instance.PlaceMaterial(); }
        else if (glyph.Category() == global::Glyph.Categories.item) { meshRenderer.material = Data.instance.ItemMaterial(); }
        else if (glyph.Category() == global::Glyph.Categories.logic) { meshRenderer.material = Data.instance.LogicMaterial(); }
        spriteRenderer.sprite = glyph.Sprite();
        spawnEffect.GetComponent<ParticleSystemRenderer>().material = meshRenderer.material;
        if (playableScript != null) { playableScript.LineRendererMaterial(meshRenderer.material); }
    }

    // Main Loop Methods

    public void FixedUpdate()
    {
        if (boundTo != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, boundTo.transform.position);
        }
    }


    // State Methods

    public void Selected() { selectionLight.enabled = true; }
    public void Unselected() { selectionLight.enabled = false; }

    public void Episode(Episode newEpisode) { if (episode == null) { episode = newEpisode; } }
    public void ClearEpisode() { episode = null; }

    public void Disappear() { Disappear(false); }
    public void Disappear(bool spawnEffect)
    {
        if (spawnEffect) { Stage.instance.SpawnDisappearEffect(transform.position); }
        Stage.instance.DestroyIcon(this);
    }

    public void BindTo(Rigidbody other)
    {
        boundTo = other;
        springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = other;
        Stage.instance.Unplayable(gameObject);
    }
    public void Unbind()
    {
        boundTo = null;
        ClearEpisode();
        Destroy(springJoint);
        Stage.instance.Playable(gameObject);
    }

    public Vector3 RandomDirection()
    {
        Vector3 output = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0).normalized;
        return output;
    }

    // Input Methods

    private void OnMouseEnter() { Stage.instance.MouseOver(this); }
    private void OnMouseExit() { Stage.instance.MouseExit(); }

    public void OnCollisionEnter(Collision collision)
    {
        Icon collidingIcon = collision.gameObject.GetComponent<Icon>();
        if (collidingIcon != null)
        {
            Stage.instance.OnIconCollision(this, collidingIcon);
            if (episode != null) 
            {
                episode.BindIcon(collidingIcon);
            }
        }
        
        
    }

    // Get Set Methods

    public Glyph Glyph() { return glyph; }

}
