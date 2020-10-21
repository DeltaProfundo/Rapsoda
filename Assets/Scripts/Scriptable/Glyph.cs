using UnityEngine;

[CreateAssetMenu]
public class Glyph : ScriptableObject
{
    public enum Categories { motivation, situation, place, item, logic }

    public string invoke;

    public string nameEng;
    public string nameSpa;

    public Categories category;

    public Situation situation;

    public Sprite sprite;

    public string Invoke() { return invoke; }
    public string NameEng() { return nameEng; }
    public string NameSpa() { return nameSpa; }
    public Categories Category() { return category; }
    public Situation Situation() { return situation; }
    public Sprite Sprite() { return sprite; }
}
