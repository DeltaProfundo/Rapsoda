using UnityEngine;

[CreateAssetMenu]
public class SyllablePack : ScriptableObject
{
    public string[] syllables;

    public string[] Syllables() { return syllables; }
    public string Syllable() { return syllables[Random.Range(0, syllables.Length)]; }
}
