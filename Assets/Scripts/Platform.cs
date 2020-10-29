using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Enemies, IMove
{
    private MeshRenderer _render;
    private Transform _trans;
    public bool canRotate;

    // Start is called before the first frame update
    public override void Start()
    {
        _render = GetComponent<MeshRenderer>();
        _trans = GetComponent<Transform>();
        speed = 25;
        canRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate)
            Move();
    }

    public void Move()
    {
        _trans.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * speed, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Camera"))
        {
            print("me choco la camara");
            _render.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Camera"))
        {
            print("salio la camara");
            _render.enabled = true;
        }
    }

    public void DoDamage()
    {
        throw new System.NotImplementedException();
    }
}
