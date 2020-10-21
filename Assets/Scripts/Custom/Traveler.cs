using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Traveler
{
    [SerializeField] public string name;
    [SerializeField] public Occupation occupation;
    [SerializeField] public List<Glyph> glyphs;
    [SerializeField] public Material material;

    public void Generate()
    {
        SyllablePack syllablePack = Data.instance.SyllablePack();
        name += syllablePack.Syllable();
        int numberSyllables = Random.Range(1, 4);
        for (int i = 0; i < numberSyllables; i++) { name += syllablePack.Syllable().ToLower(); }
        occupation = Data.instance.Occupation();
        glyphs = new List<Glyph>();
        material = Data.instance.AvatarMaterial();
        Glyph[] occupationGlyphs = occupation.NecessaryGlyphs();
        for (int i = 0; i < occupationGlyphs.Length; i++)
        {
            glyphs.Add(occupationGlyphs[i]);
        }
    }

    public string Name() { return name; }
    public List<Glyph> Glyphs() { return glyphs; }
    public Occupation Occupation() { return occupation; }
    public Material Material() { return material; }
}
