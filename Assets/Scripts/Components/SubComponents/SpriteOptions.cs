using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOptions : MonoBehaviour {

    public List<Sprite> Sprites = new List<Sprite>();

    private SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Awake () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetOrder(int order)
    {
        _spriteRenderer.sortingOrder += order;
    }

    public void SetLevel(float level)
    {
        SetLevel((int)level);
    }

    /// <summary>
    /// If zero then it will be hidden.
    /// Otherwise take sprite from array.
    /// Index = level - 1.
    /// </summary>
    public void SetLevel(int level)
    {
        if (level <= 0)
        {
            _spriteRenderer.enabled = false;
            return;
        }
        else _spriteRenderer.enabled = true;

        var index = level - 1;
        if (index < 0) index = 0;
        else if (index > Sprites.Count - 1) index = Sprites.Count - 1;
        _spriteRenderer.sprite = Sprites[index];
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.enabled = true;
        _spriteRenderer.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
