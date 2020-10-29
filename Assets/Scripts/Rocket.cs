using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour, IMove
{
    private GameObject _mng;
    private GameManager _manager;
    public Transform trans;
    public Transform fire;
    public Transform secondFire;
    public float speed;
    public float fireSpeed;
    private int _direction = 1;
    public Vector3 _originalPos;
    public bool canMove = false;
    public Scene sceneName;
    public GameObject fireContainer;
    private AudioSource _source;
    public AudioClip sound;
    public float maxTimer;
    private float _timer;
    public bool _play;

    public bool playerCanEnter;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene();
        _source = GetComponent<AudioSource>();
        _source.clip = sound;
        trans = GetComponent<Transform>();
        if (sceneName.name == "Level1")
        {
            _mng = GameObject.Find("Manager");
            _manager = _mng.GetComponent<GameManager>();
            
            _originalPos = trans.position;
        }

        if (sceneName.name == "WinScreen")
        {
            _originalPos = base.transform.position;
            canMove = true;
            speed = 50;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            Move();
        SoundTimer();
    }

    public void Move()
    {
        if (sceneName.name == "Level1")
            fireContainer.SetActive(true);

        trans.position += new Vector3(0, 1, 0) * Time.deltaTime * speed;
        if (fire.position.x >= 0.1)
            _direction = -1;
        if (fire.position.x <= -0.1f)
            _direction = 1;
        fire.position += new Vector3(_direction, 0, 0) * Time.deltaTime * fireSpeed;
        secondFire.position += new Vector3(-_direction, 0, 0) * Time.deltaTime * fireSpeed;

        if (_play)
        {
            _source.Play();
            _play = false;
        }
            

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
            base.transform.position = _originalPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && playerCanEnter)
        {
            _manager.playerOnRocket = true;
            canMove = true;
            _manager.ChangeScene(0);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            _manager.ChangeScene(1);
    }

    public void DoDamage()
    {
        throw new System.NotImplementedException();
    }

    private void SoundTimer()
    {
        if (!_play)
            _timer += Time.deltaTime;

        if (_timer >= maxTimer)
        {
            _play = true;
            _timer = 0;
        }
    }
}
