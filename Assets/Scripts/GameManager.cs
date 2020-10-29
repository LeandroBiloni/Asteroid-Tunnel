using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Console
    public GameObject consoleObj;
    public Console console;
    public Text consoleText;

    //Camera
    private GameObject _cam;
    public Vector3 distance;
    private GameObject _follow;

    //UI
    public GameObject canvas;
    public Image lifeBar;
    public GameObject messageContainer;
    public Text countdown;
    public bool _activeCountdown = false;
    public float countdownTime;
    public GameObject pause;
    public Text lifeBarText;

    //Background
    private GameObject _background;
    public float rotationSpeed;

    //Player
    private GameObject _playerObj;
    private Player _player;
    private Vector3 _playerOriginalPos;
    public Vector3 playerLastPosition;
    public bool playerOnRocket = false;

    //Platforms and enemies
    private Quaternion _containerOriginalRot;
    private GameObject _platformsContainer;
    public Quaternion lastRotation;
    public Spawn spawn;

    public List<Platform> _platformsList = new List<Platform>();
    private GameObject _platParent;
    private Platform _platform;
    private bool _platformRotation = false;

    //Checkpoint
    public bool activeCheckpoint = false;

    //Buttons
    public int maxButtons = 3;
    public int activeButtons = 0;

    //Scene
    private GameObject _sceneManager;
    private SceneController _sceneController;

    //Rocket
    public GameObject _rocketObj;
    public Rocket _rocket;

    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        _sceneManager = GameObject.Find("SceneController");
        _sceneController = _sceneManager.GetComponent<SceneController>();
        _cam = GameObject.Find("Main Camera");
        _background = GameObject.Find("Background");
        _playerObj = GameObject.Find("Player");
        _player = _playerObj.GetComponent<Player>();
        _follow = _playerObj;
        _playerOriginalPos = _playerObj.transform.position;
        console.AddCommand("Countdown", ChangeCountdownState, "true to activate countdown ; false to deactivate countdown.");
        console.AddCommand("RestartLevel", RestartLevel, null);
        _platformsContainer = GameObject.Find("Platform Container");
        if (_platformsContainer)
        {
            _containerOriginalRot = _platformsContainer.transform.rotation;
            lastRotation = _platformsContainer.transform.rotation;
        }
        
        activeCheckpoint = false;
        spawn = _platformsContainer.GetComponent<Spawn>();

        if (spawn)
        {
            if (_sceneController.namee == "Level1")
            {
                spawn.GetSpawns();
                GetPlatforms();
                _rocketObj = GameObject.Find("Rocket(Clone)");
                _rocket = _rocketObj.GetComponent<Rocket>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_sceneController.actualscene.name != "Tutorial")
        {
            PlayerLifeBar();
            if (_platformRotation == false && activeButtons >= 1)
            {
                RotatePlatforms();
                StartCountdown();
            }
            if (activeButtons == maxButtons)
                _rocket.playerCanEnter = true;

            if (_activeCountdown)
                CountdownTimer();

            if (playerOnRocket)
            {
                messageContainer.SetActive(false);
                _playerObj.SetActive(false);
                ChangeCameraTarget();
                UIOff();
            }
        }
        CameraMovement();
        CheckKeys();
        BackgroundRotation();       
    }

    private void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isPaused == false)
            RotatePlatforms('q');

        if (Input.GetKeyDown(KeyCode.E) && isPaused == false)
            RotatePlatforms('e');

        if (Input.GetKeyDown(KeyCode.P))
            Pause();

        if (Input.GetKeyDown(KeyCode.Escape))
            _sceneController.GoToLevel();
    }
    private void CameraMovement()
    {
        _cam.transform.position = _follow.transform.position - distance;
    }

    private void BackgroundRotation()
    {
        _background.transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime * rotationSpeed, Space.World);
    }

    private void RotatePlatforms(char key)
    {
        if (key == 'q')
        {
            _platformsContainer.transform.Rotate(new Vector3(1, 0, 0), 90);
        }

        if (key == 'e')
        {
            _platformsContainer.transform.Rotate(new Vector3(-1, 0, 0), 90);
        }
    }
    private void Respawn()
    {
        _playerObj.transform.position = _playerOriginalPos;
        _playerObj.transform.rotation = Quaternion.Euler(Vector3.zero); // Reinicio la rotación del jugador en caso de que se haya rotado al caerse del mapa
        _platformsContainer.transform.rotation = _containerOriginalRot;
    }

    public void Checkpoint()
    {
        if (activeCheckpoint)
        {
            _playerObj.transform.position = playerLastPosition;
            _playerObj.transform.rotation = Quaternion.Euler(Vector3.zero); // Reinicio la rotación del jugador en caso de que se haya rotado al caerse del mapa
            _platformsContainer.transform.rotation = lastRotation;
        }
        else Respawn();
    }

    public Vector3 PlayerPosition()
    {
        return _playerObj.transform.position;
    }

    public void DamageToPlayer(float damage)
    {
        _player.TakeDamage(damage);
    }

    public void PlayerLifeBar()
    {
        lifeBar.fillAmount = _player.life / _player.maxLife;
        lifeBarText.text = _player.life + "/" + _player.maxLife;
    }

    private void GetPlatforms()
    {
        foreach (Transform child in _platformsContainer.transform)
        {
            _platParent = child.gameObject;

            foreach (Transform platform in _platParent.transform)
            {
                _platform = platform.gameObject.GetComponent<Platform>();
                if (_platform != null)
                    _platformsList.Add(_platform);                    
            }
        }
    }

    private void RotatePlatforms()
    {
        for (int i = 0; i < _platformsList.Count; i++)
        {
            _platformsList[i].canRotate = true;
        }
        _platformRotation = true;
    }

    public void ChangeScene(int ending)
    {
        _sceneController.EndScreen(ending);
    }

    private void StartCountdown()
    {
        messageContainer.SetActive(true);
        _activeCountdown = true;
    }

    private void CountdownTimer()
    {
        countdownTime -= Time.deltaTime;
        countdown.text = Mathf.CeilToInt(countdownTime).ToString();
        if (countdownTime <= 0)
        {
            _platformsContainer.transform.rotation = _containerOriginalRot;
            _rocket.canMove = true;
            UIOff();
            ChangeCameraTarget();
        }    
    }

    public void ChangeCameraTarget()
    {
        _follow = _rocketObj;
    }

    private void UIOff()
    {
        canvas.SetActive(false);
    }

    private void Pause()
    {
        pause.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    private void ChangeCountdownState(List<string> data)
    {
        if (data.Count > 0)
        {
            _activeCountdown = bool.Parse(data[0]);

            if (_activeCountdown)
                consoleText.text += "\n" + "Countdown activated.";
            else consoleText.text += "\n" + "Countdown deactivated.";
        }  
    }

    private void RestartLevel(List<string> data)
    {
        Time.timeScale = 1;
        Scene level = SceneManager.GetActiveScene();
        consoleText.text += "\n" + "Level restarted.";
        SceneManager.LoadScene(level.name);
        
        
    }
}
