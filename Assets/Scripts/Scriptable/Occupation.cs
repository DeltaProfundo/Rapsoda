using UnityEngine;

[CreateAssetMenu]
public class Occupation : ScriptableObject
{
    public string invoke;

    public string nameEng;
    public string nameSpa;

    public Glyph[] necessaryGlyphs;

    public string Invoke() { return invoke; }
    public string NameEng() { return nameEng; }
    public string NameSpa() { return nameSpa; }

    public Glyph[] NecessaryGlyphs() { return necessaryGlyphs; }
}
