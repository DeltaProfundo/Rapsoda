using System.Collections.Generic;
using UnityEngine;

public class Episode : MonoBehaviour
{
    public List<SacredIcon> sacredIcons;
    public List<Icon> icons;

    public void Setup(SacredIcon firstSacredIcon, Icon firstIcon)
    {
        sacredIcons = new List<SacredIcon>();
        icons = new List<Icon>();
        BindIcon(firstIcon);
        BindIcon(firstSacredIcon);
    }

    public void Update()
    {
        if (!IsCoherent())
        {
            Cancel();
        }
        if (IsComplete())
        {
            Stage.instance.CloseEpisode(this);
        }
    }
    public bool IsCoherent()
    {
        bool result = false;
        bool hasOneSacredIcon = false;
        bool hasOneIcon = false;
        bool hasSituationGlyph = false;
        if (sacredIcons.Count > 0) { hasOneSacredIcon = true; }
        if (icons.Count > 0) { hasOneIcon = true; }
        if (Icon(Glyph.Categories.situation) != null) { hasSituationGlyph = true; }
        if (hasOneSacredIcon && hasOneIcon && hasSituationGlyph) { result = true; }
        return result;
    }
    public bool IsComplete()
    {
        bool result = false;
        int conditionsMet = 0;
        if (Icon(Glyph.Categories.situation) != null) 
        {
            if (Icon(Glyph.Categories.situation).Glyph().Situation().DeitiesRequired() == sacredIcons.Count)
            {
                conditionsMet++;
            }
        }
        if (Icon(Glyph.Categories.motivation) != null) { conditionsMet++; }
        if (Icon(Glyph.Categories.place) != null) { conditionsMet++; }
        if (Icon(Glyph.Categories.item) != null) { conditionsMet++; }
        if (conditionsMet >= 4) { result = true; }
        return result;
    }


    public bool BindIcon(Icon icon)
    {
        Debug.Log(icon.Glyph().name);
        bool done = false;
        if (icon.Glyph().Category() == Glyph.Categories.situation && Icon(Glyph.Categories.situation) == null) 
        {
            icons.Add(icon);
            icon.Episode(this);
            Stage.instance.Unplayable(icon.gameObject);
            return true; 
        }
        if (Icon(Glyph.Categories.situation) == null) { Cancel(); }
        Icon situationIcon = Icon(Glyph.Categories.situation);
        Rigidbody situationIconBody = situationIcon.gameObject.GetComponent<Rigidbody>();
        if (icon.Glyph().Category() == Glyph.Categories.motivation && Icon(Glyph.Categories.motivation) == null) { done = true; }
        else if (icon.Glyph().Category() == Glyph.Categories.place && Icon(Glyph.Categories.place) == null) { done = true; }
        else if (icon.Glyph().Category() == Glyph.Categories.item && Icon(Glyph.Categories.item) == null) { done = true; }
        if (done)
        {
            Debug.Log("Bound " + icon.Glyph().NameEng() + " to " + gameObject.name);
            icons.Add(icon);
            icon.Episode(this);
            icon.BindTo(situationIconBody);
            Stage.instance.Unplayable(icon.gameObject);

        }

        return done;
    }
    public bool BindIcon(SacredIcon icon)
    {
        bool done = false;
        Icon situationIcon = Icon(Glyph.Categories.situation);
        Debug.Log("BindIcon called for sacred icon");
        if (situationIcon != null)
        {
            Debug.Log("... and situation icon ain't null");
            Rigidbody situationIconBody = situationIcon.gameObject.GetComponent<Rigidbody>();
            if (situationIcon.Glyph().Situation().DeitiesRequired() > sacredIcons.Count)
            {
                done = true;
                icon.Episode(this);
                icon.BindTo(situationIconBody);
                sacredIcons.Add(icon);
            }
        }
        else
        {
            Debug.Log("Sacred icons can't be bound without a set situation glyph");
            Cancel();
        }
        return done;
    }
    public void UnbindIcon(Icon icon)
    {
        icon.Unbind();
        icon.ClearEpisode();
        icons.Remove(icon);
    }
    public void UnbindIcon(SacredIcon icon)
    {
        icon.Unbind();
        icon.ClearEpisode();
        sacredIcons.Remove(icon);
    }

    public Icon Icon(Glyph.Categories category)
    {
        Icon output = null;
        for(int i = 0; i < icons.Count; i++) { if (icons[i].Glyph().Category() == category) { output = icons[i]; } }
        return output;
    }

    public void Cancel()
    {
        foreach(SacredIcon si in sacredIcons) { UnbindIcon(si); }
        foreach(Icon i in icons) { UnbindIcon(i); }
        Destroy(gameObject);
    }

    public List<SacredIcon> SacredIcons() { return sacredIcons; }
    public List<Icon> Icons() { return icons; }

}
