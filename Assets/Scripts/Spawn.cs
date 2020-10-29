using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject checkpoint;
    public GameObject verticalPlatformPrefab;
    public GameObject horizontalPlatformPrefab;
    public GameObject spikePrefab;
    public GameObject minePrefab;
    public GameObject buttonPrefab;
    public GameObject rocketPrefab;

    public List<GameObject> spawnContainer = new List<GameObject>();
    public GameObject container;
    
    public int index = 0;
    public int changePlatform;
    public int min = 0;
    public int max = 4;
    public int random;
    public int randomCopy;
    public int randomEnemy;

    public List<GameObject> buttonContainer = new List<GameObject>();
    public GameObject butContainer;
    public GameObject buttonA;
    public GameObject buttonB;
    public GameObject buttonC;

    private int _checkPointLocation;

    // Start is called before the first frame update
    void Start()
    {
        butContainer = GameObject.Find("ButtonContainer");
        transform.position = Vector3.zero;
        container = gameObject;
    }

    public void GetSpawns()
    {
        foreach(Transform child in container.transform)
        {
            if (child != null)
            {
                spawnContainer.Add(child.gameObject);
            }
        }
        foreach(Transform child in butContainer.transform)
        {
            if (child != null)
            {
                buttonContainer.Add(child.gameObject);
            }
        }
        SpawnPlatformsAndEnemies();
        SpawnButtons();
        SpawnCheckpoint();
        SpawnRockets();
    }

    public void SpawnPlatformsAndEnemies()
    {
        for (int i = 0; i < spawnContainer.Count; i++)
        {
            i = Random.Range(i, i+4);
            randomCopy = i % 2;

            if (i < spawnContainer.Count && i != spawnContainer.Count/2 && i != (spawnContainer.Count / 2)+1  && i != spawnContainer.Count/4 && i != (spawnContainer.Count/4)*3 && i != spawnContainer.Count-1)
            {
                if (randomCopy == 0) // Crea plataforma horizontal
                {
                    GameObject plat = Instantiate(horizontalPlatformPrefab, spawnContainer[i].transform, false);
                    plat.transform.localPosition = Vector3.zero;

                    randomEnemy = Random.Range(0, 3);

                    if(randomEnemy == 0) // Crea Spikes
                    {
                        GameObject spike = Instantiate(spikePrefab, plat.transform, false);
                        if (plat.transform.parent.position.y == 2)
                            spike.transform.localPosition = Vector3.zero + new Vector3(14, 15, 10);
                        if (plat.transform.parent.position.y == -2)
                            spike.transform.localPosition = Vector3.zero + new Vector3(11, 20, -14);
                        spike.transform.localRotation = new Quaternion(-90, 0, 0, 0);
                        spike.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
                    }

                    if(randomEnemy == 1) // Crea Minas
                    {
                        GameObject mine = Instantiate(minePrefab, plat.transform, false);
                        if (plat.transform.parent.position.y == 2)
                            mine.transform.localPosition = Vector3.zero + new Vector3(0, 0, 16);
                        if (plat.transform.parent.position.y == -2)
                            mine.transform.localPosition = Vector3.zero + new Vector3(0, 0, -16);
                        mine.transform.localScale = new Vector3(2, 4, 3);
                    }
                }
                else // Crea plataforma vertical
                {
                    GameObject plat = Instantiate(verticalPlatformPrefab, spawnContainer[i].transform, false);
                    plat.transform.localPosition = Vector3.zero;

                    randomEnemy = Random.Range(0, 3);

                    if (randomEnemy == 0) // Crea Spikes
                    {
                        GameObject spike = Instantiate(spikePrefab, plat.transform, false);
                        if (plat.transform.parent.position.z == -3)
                            spike.transform.localPosition = Vector3.zero + new Vector3(10, -18, 12);
                        if (plat.transform.parent.position.z == 3)
                            spike.transform.localPosition = Vector3.zero + new Vector3(10, 0, -15);
                        spike.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        spike.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
                    }

                    if (randomEnemy == 1) // Crea Minas
                    {
                        GameObject mine = Instantiate(minePrefab, plat.transform, false);
                        if (plat.transform.parent.position.z == -3)
                            mine.transform.localPosition = Vector3.zero + new Vector3(0, 0, 13);
                        if (plat.transform.parent.position.z == 3)
                            mine.transform.localPosition = Vector3.zero + new Vector3(0, 0, -13);
                        mine.transform.localScale = new Vector3(2, 4, 3);
                    }



                } 
            }
        }    
    }

    private void SpawnButtons()
    {
        //Crea botones
        buttonA = Instantiate(buttonPrefab, buttonContainer[0].transform, false);
        buttonA.transform.localPosition = Vector3.zero;
        buttonA.transform.Rotate(new Vector3(0, 0, 0), Space.Self);

        buttonB = Instantiate(buttonPrefab, buttonContainer[1].transform, false);
        buttonB.transform.localPosition = Vector3.zero;
        buttonB.transform.Rotate(new Vector3(-45, 0, 0), Space.Self);

        buttonC = Instantiate(buttonPrefab, buttonContainer[2].transform, false);
        buttonC.transform.localPosition = Vector3.zero;
        buttonC.transform.Rotate(new Vector3(45, 0, 0), Space.Self);
    }

    private void SpawnCheckpoint()
    {
        for (int i = 0; i < 3; i++)
        {

            switch (i)
            {
                case 0:
                    _checkPointLocation = (spawnContainer.Count / 2);
                    break;

                case 1:
                    _checkPointLocation = (spawnContainer.Count / 2) + (spawnContainer.Count / 4);
                    break;

                case 2:
                    _checkPointLocation = (spawnContainer.Count / 2) - (spawnContainer.Count / 4);
                    break;
            }
            //Crea Checkpoint
            GameObject checkPlat = Instantiate(horizontalPlatformPrefab, spawnContainer[_checkPointLocation].transform, false);
            checkPlat.transform.localPosition = Vector3.zero;
            GameObject checkpA = Instantiate(checkpoint, spawnContainer[_checkPointLocation].transform, false);
            checkpA.transform.localPosition = Vector3.zero + new Vector3(0, 0.5f, 0);
            GameObject checkpB = Instantiate(checkpoint, spawnContainer[_checkPointLocation].transform, false);
            checkpB.transform.localPosition = Vector3.zero + new Vector3(0, -0.5f, 0);
            checkpB.transform.localRotation = new Quaternion(180, 0, 0, 0);
        }
        
    }

    private void SpawnRockets()
    {
        //Crea cohete
        GameObject rocketPlat = Instantiate(horizontalPlatformPrefab, spawnContainer[spawnContainer.Count - 1].transform, false);
        rocketPlat.transform.localPosition = Vector3.zero;

        GameObject rocket = Instantiate(rocketPrefab, spawnContainer[spawnContainer.Count - 1].transform, false);
        rocket.transform.localPosition = new Vector3(0, 1.5f, 0);
    }
}
