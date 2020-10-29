using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject manag;
    public GameManager manager;
    MeshRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        manag = GameObject.Find("Manager");
        manager = manag.GetComponent<GameManager>();
        render = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            SavePosition();

        if (other.gameObject.layer == LayerMask.NameToLayer("Camera"))
        {
            print("me choco la camara");
            render.enabled = false;
        }
    }
    private void SavePosition()
    {
        Transform container = transform.parent; // Solía estar "GetComponentInParent<Transform>()" que lo que hace es devolverte el primer componente que encuentre de ese tipo, incluyendo a si mismo. Por ende el que encontraba era al checkpoint en sí en vez de a su padre (El Container).
        manager.lastRotation = container.rotation;
        manager.playerLastPosition = transform.position;
        manager.activeCheckpoint = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Camera"))
        {
            print("salio la camara");
            render.enabled = true;
        }
    }
}
