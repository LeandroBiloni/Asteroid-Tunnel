using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : MonoBehaviour
{
    public float damage;
    public float speed;
    public bool isMovable = false;
    public bool changeDirection = false;
    public int dirMultiplier = 1;
    public Vector3 direction;
    public float changeTimer;
    public float changeLimit;
    public int randomMove;
    // Start is called before the first frame update
    public virtual void Start()
    {
        damage = 5;
        speed = 3;
    }
}
