using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCheck : MonoBehaviour
{
    public static bool rightCollision = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            rightCollision = true;
        }
        else
        {
            rightCollision = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rightCollision = false;
    }
}
