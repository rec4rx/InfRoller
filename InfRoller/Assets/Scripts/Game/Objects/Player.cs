using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The <c>Player</c> class.
/// Main player
/// </summary>
public class Player : MonoBehaviour
{
    //dead event delegate
    public delegate void EndgameDelegate();
    public static EndgameDelegate endgameDelegate;

    //rigid body
    private Rigidbody2D _rigidbody = null;

    //check if player is jumping or not
    private bool _isJumping = false;
    public bool IsJumping => _isJumping;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Force to move forward, with given speed
    /// </summary>
    /// <param name="speed">Speed</param>
    public void Move(float speed)
    {
        _rigidbody.velocity = new Vector2(speed, 0);
    }

    /// <summary>
    /// Jump with specified power.
    /// </summary>
    /// <param name="power">Power.</param>
    public void Jump(float power)
    {
        _isJumping = true;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, power);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObj = collision.gameObject;

        if (collisionObj.CompareTag("Ground") == true)
        {
            //land
            _isJumping = false;
        } else if (collisionObj.CompareTag("Block") == true)
        {
            //game end, call delegate
            endgameDelegate();
        }
    }
}
