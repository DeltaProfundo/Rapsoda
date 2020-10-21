using UnityEngine;

[CreateAssetMenu]
public class RoundPreset : ScriptableObject
{
    public LevelPreset[] levelPresets;

    public LevelPreset[] LevelPresets() { return levelPresets; }
}
