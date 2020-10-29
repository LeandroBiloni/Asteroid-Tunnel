using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : Enemies, IMove
{
    private GameObject _mng;
    private GameManager _manager;
    private Vector3 _playerPos;
    private Vector3 _distance;
    public float distance;
    private AudioSource _source;
    public AudioClip explosion;
    public AudioClip beep;
    private float _explosionTime;
    private float _beepTime;
    public float maxBeepTime;
    private bool _crashed = false;
    private bool _play;
    // Start is called before the first frame update
    public override void Start()
    {
        _mng = GameObject.Find("Manager");
        _manager = _mng.GetComponent<GameManager>();
        damage = 10;
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerPos = _manager.PlayerPosition();
        CheckDistance();
        if (_crashed)
            Explode();
        SoundTimer();
    }

    public void Move()
    {
        transform.position -= direction * Time.deltaTime * speed;
    }

    private void CheckDistance()
    {
        _distance = transform.position - _playerPos;
        direction = _distance.normalized;
        if (_distance.magnitude < distance)
        {
            Move();
            if (_source.clip != beep && _crashed == false)
                _source.clip = beep;
          
            if (_play && _crashed == false)
            {
                _source.Play();
                _play = false;
            }
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && _crashed == false)
        {
            DoDamage();
            _crashed = true;
            _source.clip = explosion;
            _source.Play();
            transform.localScale = new Vector3(0, 0, 0);
        }
            
    }

    public void DoDamage()
    {
        print("Choque al player");
        _manager.DamageToPlayer(damage);
    }

    private void Explode()
    {
        if (_explosionTime <= 2)
            _explosionTime += Time.deltaTime;

        if (_explosionTime >= 2)
            Destroy(gameObject);
    }

    private void SoundTimer()
    {
        if (!_play)
            _beepTime += Time.deltaTime;

        if (_beepTime >= maxBeepTime)
        {
            _play = true;
            _beepTime = 0;
        }
    }
}
