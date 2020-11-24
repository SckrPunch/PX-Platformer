using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCheck : MonoBehaviour
{
    public static bool leftCollision = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            leftCollision = true;
        }
        else
        {
            leftCollision = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        leftCollision = false;
    }
}
