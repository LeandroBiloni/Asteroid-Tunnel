using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Scene actualscene;
    public string namee;

    // Start is called before the first frame update
    void Start()
    {
        actualscene = SceneManager.GetActiveScene();
        namee = actualscene.name;
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene("Level1");
        print("Changing scene");
    }

    public void EndScreen(int ending)
    {
        switch (ending)
        {
            case 0:
                SceneManager.LoadScene("WinScreen");
                break;

            case 1:
                SceneManager.LoadScene("LoseRocketEnd");
                break;

            case 2: 
                SceneManager.LoadScene("FallEnd");
                break;

            case 3:
                SceneManager.LoadScene("KilledEnd");
                break;
        }
    }
}
