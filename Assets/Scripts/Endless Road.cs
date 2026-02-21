using UnityEngine;
using System.Collections;

public class EndlessRoad : MonoBehaviour
{
    [SerializeField]
    GameObject[] sectionsPrefabs;
    GameObject[] allSections = new GameObject[20];
    GameObject[] sections = new GameObject[10];
    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);

    Transform carTransform;
    const float sectionLength = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carTransform = GameObject.FindGameObjectWithTag("car").transform;
        int prefabIndex = 0;

        //selecting from a group of sections
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
            randSection.transform.position = new Vector3(allSections[i].transform.position.x, 0, i * sectionLength);
            randSection.SetActive(true);
            sections[i] = randSection;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionsPos();
            yield return waitFor100ms;
        }
    }

    void UpdateSectionsPos()
    {
        for (int i = 0 ; i < sections.Length; i++)
        {
            if (sections[i].transform.position.z - carTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);
                sections[i] = GetRandomSection();
                sections[i].transform.position = new Vector3(lastSectionPosition.x, 0, lastSectionPosition.z + sectionLength * sections.Length);
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
