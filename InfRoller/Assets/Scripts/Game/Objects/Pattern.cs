using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The <c>Pattern</c> class.
/// Contains blocks, main game object
/// </summary>

public class Pattern : RecyclingObject
{
    //width of the whole pattern
    [SerializeField] private float _width = 0.0f;
    public float Width {
        get {
            return _width;
        }
    }

    //list of blocks in this pattern
    [SerializeField] List<Block> _blocks = new List<Block>();

    public override void Init()
    {
        base.Init();
        ReInitBlocks();
    }

    //init blocks for using poll correcty
    private void ReInitBlocks()
    {
        foreach (Block block in _blocks)
        {
            block.IsPassed = false;
        }
    }

    //for editor: auto fill blocks into pattern, also calculate its width
    public void EditorAutoFillBlocks ()
    {
        //check and clear the list first to prevent bugs.
        if (_blocks.Count > 0)
        {
            _blocks.Clear();
            _width = 0.0f;
        }

        float minX = int.MaxValue;
        float maxX = int.MinValue;

        //then add all its block-type children to the list
        foreach (Block block in GetComponentsInChildren<Block>())
        {
            //add block to list
            _blocks.Add(block);

            //calc minX of pattern
            float bMinX = block.MinX();
            if (minX > bMinX)
            {
                minX = bMinX;
            }

            //calc maxX of pattern
            float bMaxX = block.MaxX();
            if (maxX < bMaxX)
            {
                maxX = bMaxX;
            }

            //calc pattern width
            _width = maxX - minX;

            block.transform.position = new Vector3(block.transform.position.x, block.Height() / 2.0f, 0);
        }
    }
}
