using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AudioReactiveRoad : MonoBehaviour
{
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

    public Transform carTransform;
    public float spacing = 10000f;
    public float sectionLength = 5000f;

    void Start()
    {
        carTransform = GameObject.FindGameObjectWithTag("car").transform;
        
        buildings = new GameObject[nBuildings];
        float buildingLength = buildingPrefabs[0].GetComponent<Renderer>().bounds.size.z;

        float spacing = buildingLength * 3;

        buildings = new GameObject[nBuildings];

        for (int i = 0; i < nBuildings; i++)
        {
            GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

            Vector3 pos = new Vector3(25f, 0f, i * spacing);
            buildings[i] = Instantiate(prefab, pos, Quaternion.identity);
            buildings[i].transform.Rotate(0f, 270f, 0f, Space.World); //rotate to face the road
        }
    }
    void Awake()
    {
        spectrumData = new float[4096];
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
        var blockSize = spectrumData.Length / buildings.Length / (int)freqFocusWindow;
        
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
    

    public enum FrequencyFocusWindow
    {
        Entire = 1,
        FirstHalf = 2,
        FirstQuarter = 4,
        FirstEight = 8,
        FirstSixteenth = 16
    }

}
