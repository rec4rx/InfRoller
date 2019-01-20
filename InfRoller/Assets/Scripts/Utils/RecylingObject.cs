using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The <c>RecyclingObject</c> class.
/// Recycling objects
/// </summary>
public class RecyclingObject : MonoBehaviour
{

    /// <summary>
    /// init, override it for subclasses
    /// </summary>
    public virtual void Init()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// release, don't destroy
    /// </summary>
    public virtual void Release()
    {
        gameObject.SetActive(false);
    }

}