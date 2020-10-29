using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Enemies, IMove
{
    GameObject manag;
    public GameManager manager;
    public Renderer render;
    MeshRenderer parent;
    bool disabled;

    // Start is called before the first frame update
    public override void Start()
    {
        render = GetComponent<Renderer>();
        manag = GameObject.Find("Manager");
        manager = manag.GetComponent<GameManager>();
        base.Start();
        direction = new Vector3(1, 0, 0);
        randomMove = (Random.Range(0, 2));
        if (randomMove == 1)
            isMovable = true;
        parent = gameObject.GetComponentInParent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovable)
        {
            Move();
            ChangeDirection();
        }

        if (parent.enabled && disabled == true)
        {
            transform.localPosition = Vector3.zero;
            isMovable = true;
            disabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            DoDamage();

        if (other.gameObject.layer == LayerMask.NameToLayer("Camera"))
        {
            print("me choco la camara");
            disabled = true;
            isMovable = false;
            transform.localPosition = new Vector3(0, 0, 300);
        }
    }

    public void Move()
    {

        if (changeDirection)
            dirMultiplier = 1;
        else dirMultiplier = -1;

        transform.position += direction * Time.deltaTime * speed * dirMultiplier;
    }

    private void ChangeDirection()
    {
        changeTimer += Time.deltaTime;

        if (changeTimer >= changeLimit)
        {
            changeDirection = !changeDirection;
            changeTimer = 0;
        }
    }

    public void DoDamage()
    {
        manager.DamageToPlayer(damage);
    }
}
