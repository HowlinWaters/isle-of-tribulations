using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scripts inheriting this can make objects "hittable" by the player
public interface IHittable
{
    void TakeDamage(int hpLost, Vector3 direction);
}
