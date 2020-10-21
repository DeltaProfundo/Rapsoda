using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Reaction : ScriptableObject
{
    public List<Glyph> ingredients;
    public Glyph result;

    public List<Glyph> Ingredients() { return ingredients; }
    public Glyph Result() { return result; }
}
