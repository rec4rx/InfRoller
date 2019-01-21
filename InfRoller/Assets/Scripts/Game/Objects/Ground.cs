using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The <c>Ground</c> class.
/// Ground platforms for rolling
/// </summary>
public class Ground : MonoBehaviour
{
    //list contains parts
    [SerializeField] private List<GameObject> _parts;
    //how many times expanded
    private int _expandTime = 0;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Loop 2 grounds, make the ground looks like it's never end.
    /// </summary>
    /// <param name="groundWidth">Ground width.</param>
    public void VirtualExpand (float groundWidth)
    {
        _expandTime++;
        if (_expandTime % 2 == 0)
        {
            _parts[1].transform.position = _parts[0].transform.position + new Vector3(groundWidth, 0, 0);
        }
        else
        {
            _parts[0].transform.position = _parts[1].transform.position + new Vector3(groundWidth, 0, 0);
        }
    }
}
