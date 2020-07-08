using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    Transform[] positions=null;

    public void setPosition(int i)
    {
        transform.position = positions[i].position;
    }
}
