using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deity
{
    [SerializeField] public string name;
    [SerializeField] public List<Glyph> associations;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Material material;

    public void Generate()
    {
        name = Data.instance.GenerateName();
        associations = new List<Glyph>();
        sprite = Data.instance.GetDeitySprite();
        material = Data.instance.SacredIconMaterial();
    }

    public string Name() { return name; }
    public List<Glyph> Associations() { return associations; }
    public Glyph Association() { return associations[Random.Range(0, associations.Count)]; }
    public void AddAssociation(Glyph newGlyph)
    {
        if (!associations.Contains(newGlyph)) { associations.Add(newGlyph); }
    }
    public Sprite Sprite() { return sprite; }
    public Material Material() { return material; }
}
