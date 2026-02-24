using UnityEngine;
using System.Collections;

public class EndlessRoad : MonoBehaviour
{
    //CAR
     public Transform carTransform;
    public float moveSpeed = 3f;


    //ROAD SECTIONS
    [SerializeField]
    GameObject[] sectionsPrefabs;
    GameObject[] allSections = new GameObject[20];
    GameObject[] sections = new GameObject[10];
    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);

    //AUDIO REACTIVE BUILDINGS
    public AudioSource audioSource;
    public GameObject[] buildingPrefabs; 
    int nBuildings = 32;
    GameObject[] buildings;
    public FrequencyFocusWindow freqFocusWindow;
    public float amplification = 1.0f;
    public float baseHeight = 0.0f;
    public FFTWindow fftWindow;
    public bool useDecibels;
    public float[] spectrumData;

    public float spacing;
    public float sectionLengthBuildings;
    const float sectionLength = 39f;
    public float pauseAtTime = 40f;
    private bool isPaused = false;

    //AUDIO BARS
    public Transform audioBarTransform;


    void Start()
    {
        carTransform = GameObject.FindGameObjectWithTag("car").transform;
        int prefabIndex = 0;

        audioBarTransform = GameObject.FindGameObjectWithTag("bars").transform;

        //selecting from a group of road sections
        for (int i = 0; i < allSections.Length; i++)
        {
            allSections[i] = Instantiate(sectionsPrefabs[prefabIndex]);
            allSections[i].SetActive(false);

            prefabIndex++;
            if (prefabIndex > sectionsPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }

        //randomly add sections to road
        for (int i = 0; i < sections.Length; i++)
        {
            GameObject randSection = GetRandomSection();
            randSection.transform.position = new Vector3(0, 0, i * sectionLength);   //allSections[i].transform.position.x
            randSection.SetActive(true);
            sections[i] = randSection;
        }

        StartCoroutine(UpdateLessOftenCO());


        ///////////////////////////////////////////////////////////////////////////////////
        
        //create random buildings
        buildings = new GameObject[nBuildings];
        sectionLengthBuildings = buildingPrefabs[0].GetComponent<Renderer>().bounds.size.z;

        float spacing = sectionLengthBuildings * 2;

        buildings = new GameObject[nBuildings];

        for (int i = 0; i < nBuildings; i++)
        {
            GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

            float side = (i % 2 == 0) ? 20f : -20f;

            Vector3 pos = new Vector3(side, 0f, i * spacing);
            
            buildings[i] = Instantiate(prefab, pos, Quaternion.identity);

            float rotation = (i % 2 == 0) ? 270f : 90f;
            buildings[i].transform.Rotate(0f, rotation, 0f, Space.World); //rotate to face the road
        }
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (!isPaused)
        {
            UpdateSectionsPos();
            yield return waitFor100ms;
        }
    }

    void Awake()
    {
        spectrumData = new float[4096];
    }

    //updating buildings
    void Update()
    {
        //walkers
        GameObject walker = GameObject.Find("walker");
         GameObject walker1 = GameObject.Find("walker1");
         GameObject walker2 = GameObject.Find("walker2");
         GameObject walker3 = GameObject.Find("walker3");
         GameObject walker4 = GameObject.Find("walker4");
        Debug.Log(audioSource.time);
        if (audioSource.time >= pauseAtTime && audioSource.time <= 71)
        {
            isPaused = true;
        } else
        {
            isPaused = false;
        }
        
        if (!isPaused) {
            //make walkers invisible
            walker.GetComponent<Renderer>().enabled = false;
            walker1.GetComponent<Renderer>().enabled = false;
            walker2.GetComponent<Renderer>().enabled = false;
            walker3.GetComponent<Renderer>().enabled = false;
            walker4.GetComponent<Renderer>().enabled = false;
            carTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); //move car forward
            audioBarTransform.position = new Vector3(carTransform.position.x - 3.5f, carTransform.position.y + 0.1f, carTransform.position.z + 7); //move audio with cars

            audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
            int blockSize = Mathf.Max(1, spectrumData.Length / buildings.Length / (int)freqFocusWindow);        
            for (int i = 0; i < buildings.Length; ++i)
            {
                float sum = 0;
                for (int j = 0; j < blockSize; j++)
                {
                    sum += spectrumData[i * blockSize + j];
                }
                sum /= blockSize;
                float amplitude = Mathf.Clamp(sum, 1e-7f, 1f);
                var scale = buildings[i].transform.localScale;
                if (useDecibels)
                {
                    scale.y = -Mathf.Log10(amplitude) * amplification / 200;
                } 
                else
                {
                    scale.y = sum * amplification + baseHeight;
                } 
                buildings[i].transform.localScale = scale;
            }

            RecycleBuildings();
        }else
        {
            //position walkers in front of the caar
            Vector3 currentPositionCar = GameObject.FindGameObjectWithTag("car").transform.position;
            Vector3 currentPosition = walker.transform.position;
            walker.transform.position = new Vector3(currentPosition.x, currentPosition.y, currentPositionCar.z+10);
            Vector3 currentPosition1 = walker1.transform.position;
            walker1.transform.position = new Vector3(currentPosition1.x, currentPosition1.y, currentPositionCar.z+10);
            Vector3 currentPosition2 = walker2.transform.position;
            walker2.transform.position = new Vector3(currentPosition2.x, currentPosition2.y, currentPositionCar.z+10);
            Vector3 currentPosition3 = walker3.transform.position;
            walker3.transform.position = new Vector3(currentPosition3.x, currentPosition3.y, currentPositionCar.z+10);
            Vector3 currentPosition4 = walker4.transform.position;
            walker4.transform.position = new Vector3(currentPosition4.x, currentPosition4.y, currentPositionCar.z+10);
            //make walkers visible
            walker.GetComponent<Renderer>().enabled = true;
            walker1.GetComponent<Renderer>().enabled = true;
            walker2.GetComponent<Renderer>().enabled = true;
            walker3.GetComponent<Renderer>().enabled = true;
            walker4.GetComponent<Renderer>().enabled = true;
        }
    }

    void RecycleBuildings()
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].transform.position.z < carTransform.position.z - sectionLength)
            {
                float maxZ = GetFurthestZ();

                buildings[i].transform.position = new Vector3(buildings[i].transform.position.x, 0, maxZ + spacing);
            }
        }
    }

    float GetFurthestZ()
    {
        float maxZ = buildings[0].transform.position.z;

        for (int i = 1; i < buildings.Length; i++)
        {
            if (buildings[i].transform.position.z > maxZ)
                maxZ = buildings[i].transform.position.z;
        }

        return maxZ;
    }
    
    //updating road section positions
    void UpdateSectionsPos()
    {
        for (int i = 0 ; i < sections.Length; i++)
        {
            if (sections[i].transform.position.z - carTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);
                sections[i] = GetRandomSection();
                sections[i].transform.position = new Vector3(0, 0, lastSectionPosition.z + sectionLength * sections.Length);    //lastSectionPosition.x
                sections[i].SetActive(true);
            }
        }
    }

    GameObject GetRandomSection()
    {
        int randIndex = Random.Range(0, allSections.Length);
        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            if (!allSections[randIndex].activeInHierarchy)
            {
                isNewSectionFound = true;
            } else
            {
                randIndex++;
                if (randIndex > allSections.Length - 1)
                {
                    randIndex = 0;
                }
            }
        }

        return allSections[randIndex];
    }

    public enum FrequencyFocusWindow
    {
        Entire = 1,
        FirstHalf = 2,
        FirstQuarter = 4,
        FirstEight = 8,
        FirstSixteenth = 16
    }
}
