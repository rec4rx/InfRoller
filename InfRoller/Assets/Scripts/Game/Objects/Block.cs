using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The <c>Block</c> class.
/// Basic enemy of the game
/// </summary>

public class Block : MonoBehaviour
{
    //SpriteRender, get and hold to use later
    private SpriteRenderer _spriteRenderer = null;
    private SpriteRenderer SpriteRenderer()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        return _spriteRenderer;
    }

    public bool IsPassed {
        get;
        set;
    }

    //get Block's minX
    public float MinX()
    {
        SpriteRenderer spriteRenderer = SpriteRenderer();

        if (spriteRenderer == null)
        {
            //no SpriteRenderer, default is its position
            return transform.position.x;
        }
        float minX = spriteRenderer.bounds.min.x;

        return minX;
    }

    //get Block's minX
    public float MaxX()
    {
        SpriteRenderer spriteRenderer = SpriteRenderer();

        if (spriteRenderer == null)
        {
            //no SpriteRenderer, default is its position
            return transform.position.x;
        }
        float maxX = spriteRenderer.bounds.max.x;

        return maxX;
    }

    //get Block's height
    public float Height()
    {
        SpriteRenderer spriteRenderer = SpriteRenderer();

        if (spriteRenderer == null)
        {
            //no SpriteRenderer, default is 0
            return 0;
        }
        float height = spriteRenderer.bounds.size.y; //because the size is always return twice larger

        return height;
    }
}
