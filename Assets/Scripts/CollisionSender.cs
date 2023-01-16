using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSender : MonoBehaviour
{
    public static GameController gameController;
    void OnTriggerEnter(Collider collider)
    {
        if (gameController == null)
            return;
        gameController.CollideHandle(transform, collider);
    }
}
