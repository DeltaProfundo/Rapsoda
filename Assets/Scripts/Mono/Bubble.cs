using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public SpriteRenderer glyphRenderer;

    public void Setup(Glyph glyph)
    {
        glyphRenderer.sprite = glyph.Sprite();
    }
}
