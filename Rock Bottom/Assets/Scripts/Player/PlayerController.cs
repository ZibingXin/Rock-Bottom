using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        Move();
    }
    public void Move()
    {
        // Handle player movement input
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //This method is called every frame the player is colliding with another object
        //Start a timer, depending on MoveSpeed, after which the object gets destroyed and oil is consumed
    }
}
