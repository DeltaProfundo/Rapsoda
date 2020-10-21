using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class SacredIcon : MonoBehaviour
{
    public Episode episode;

    public MeshRenderer meshRenderer;
    private Playable playableScript;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem spawnEffect;
    private LineRenderer lineRenderer;
    public Light selectionLight;
    private SpringJoint springJoint;

    public Rigidbody boundTo;

    public Deity deity;

    public void Start()
    {
        spawnEffect = GetComponentInChildren<ParticleSystem>();
        playableScript = GetComponent<Playable>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        selectionLight = GetComponent<Light>();
    }
    public void Setup(Deity deity)
    {
        this.deity = deity;
        meshRenderer.material = deity.Material();
        spriteRenderer.sprite = deity.Sprite();
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

    private void OnMouseEnter() { Stage.instance.MouseOver(this); }
    private void OnMouseExit() { Stage.instance.MouseExit(); }

    private void OnCollisionEnter(Collision collision)
    {
        Icon collidingIcon = collision.gameObject.GetComponent<Icon>();
        SacredIcon collidingSacredIcon = collision.gameObject.GetComponent<SacredIcon>();
        if (collidingIcon != null)
        {
            Glyph collidingGlyph = collidingIcon.Glyph();
            if (collidingGlyph.Category() == Glyph.Categories.situation && episode == null) { Stage.instance.StartEpisode(this, collidingIcon); }
            if (episode != null) 
            {
                episode.BindIcon(collidingIcon); 
            }
        }
        if (collidingSacredIcon != null)
        {
            if (episode != null && collidingSacredIcon.Episode() == null)
            {
                episode.BindIcon(collidingSacredIcon);
            }
        }

    }



    public void Selected() { selectionLight.enabled = true; }
    public void Unselected() { selectionLight.enabled = false; }

    public void Episode(Episode newEpisode) { if (episode == null) { episode = newEpisode; } }
    public void ClearEpisode() { episode = null; }

    public Deity Deity() { return deity; }
    public Episode Episode() { return episode; }
}
