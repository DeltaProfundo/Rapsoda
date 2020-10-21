using UnityEngine;

[System.Serializable]
public class LevelPreset
{
    [SerializeField] public Glyph[] glyphs;
    [SerializeField] public bool newSacredIcon;

    public Glyph[] Glyphs() { return glyphs; }
    public bool NewSacredIcon() { return newSacredIcon; }
}
