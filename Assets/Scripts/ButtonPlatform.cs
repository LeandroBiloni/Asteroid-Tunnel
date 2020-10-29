using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    private MeshRenderer _materialColor;
    private GameObject _managerObj;
    private GameManager _manager;
    private bool _active = false;
    private AudioSource _source;
    public AudioClip sound;
    public GameObject sceneObj;
    public SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        sceneObj = GameObject.Find("SceneController");
        scene = sceneObj.GetComponent<SceneController>();
        _materialColor = GetComponent<MeshRenderer>();
        _managerObj = GameObject.Find("Manager");
        _manager = _managerObj.GetComponent<GameManager>();
        _source = GetComponent<AudioSource>();
        _source.clip = sound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && scene.namee == "Tutorial")
        {
            scene.GoToLevel();
            print("Boton Cambio de escena");
        }
        
        if (_active == false && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _materialColor.material.color = Color.green;
            _active = true;
            _manager.activeButtons++;
            _source.Play();
        }
    }
}
