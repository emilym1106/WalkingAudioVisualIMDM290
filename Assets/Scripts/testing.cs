using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class testing : MonoBehaviour
{
    // Scale the plot
    [Range(1f, 100f)]
    public float scale = 10;

    //capping the plot
    [Range(0f, 20000f)]
    public float maxFrequency = 1000f;

    // frequency bins are intervals between samples in frequency domain
    GameObject[] sampleBin = new GameObject[AudioSpectrum.FFTSIZE];
    int maxBin;

    float timer = 0f;

    Vector3[] targetPos;
    Vector3[] negTargetPos;
    Vector3[] zeroPos;

 
    GameObject[] crosswalks = new GameObject[8];

    public Transform carTransform;

    //controls crosswalk spawn and despawn
    bool spawned = false;
    bool despawned = false;

    void Start()
    {
        // For every frequency bin
        int sampleRate = AudioSettings.outputSampleRate;
        maxBin = Mathf.Clamp(
            Mathf.RoundToInt((maxFrequency / sampleRate) * AudioSpectrum.FFTSIZE),
            0,
            AudioSpectrum.FFTSIZE - 1
        );

        for (int i = 0; i < maxBin; i++)
        {   // Create GO and initial position
            sampleBin[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleBin[i].transform.SetParent(this.transform); // make them children of the bars object
            sampleBin[i].transform.localPosition = new Vector3(i * 0.1f, 0.02f, 7f);
            //sampleBin[i].transform.position = new Vector3(i * .02f, 0, 0);
            sampleBin[i].transform.Rotate(90, 0, 0);
            //sampleBin[i].transform.Rotate(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            sampleBin[i].SetActive(false);
        }

        //target position for lerp
        targetPos = new Vector3[maxBin];
        negTargetPos = new Vector3[maxBin];
        zeroPos = new Vector3[maxBin];
        for (int i = 0; i < maxBin; i++)
        {
            targetPos[i] = new Vector3(i * .5f, 1f, 7f);
            negTargetPos[i] = new Vector3(i * -.5f, 1f, 7f);
            zeroPos[i] = new Vector3(0, 0, 7f);
        }



    }
    void Update()
    {
        timer += Time.deltaTime;

       
        if (timer > 16f && !spawned)
        {
            spawned = true;
            
            StartCoroutine(SpawnShapes());
        }

        if (timer > 20f && spawned && !despawned)
        {
            despawned = true;
            
            StartCoroutine(DespawnShapes());
        }

        for (int i = 0; i < maxBin; i++)
        {
            targetPos[i] = new Vector3(i * .5f, 0, 7f);
            negTargetPos[i] = new Vector3(i * -.5f, 0, 7f);
            zeroPos[i] = new Vector3(0, 0, 7f);

           

            if (sampleBin != null)
            {
                //audio bars/crosswalk reacting based on the time
                //the car stops at 42, marking the start of the rising action, then the crosswalk should be placed and done animating by the climax
                if (timer > 40 && timer < 43f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localPosition = new Vector3(i * 0.1f, 0.02f, 7f);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, 0.1f);
                }

                if (timer > 43f && timer < 45f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localPosition = new Vector3(i * 0.1f, 0.02f, 7f);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                }

                if (timer > 45f && timer < 47f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, targetPos[i], Time.deltaTime * 2f);
                }

                if (timer > 47f && timer < 49f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, negTargetPos[i], Time.deltaTime * 2f);
                }

                if (timer > 49f && timer < 51f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, targetPos[i], Time.deltaTime * 2f);
                }

                if (timer > 51f && timer < 53f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, zeroPos[i], Time.deltaTime * 2f);
                }

                if (timer > 53f)
                {
                    sampleBin[i].SetActive(false);
                }

                



                //sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, 0.1f);
                //sampleBin[i].GetComponent<Renderer>().material.color = new Color(0.3f + (float)i / 100f, 0.1f + i / 30f, 1+ i / 500f);
                Color color;

                color = Color.HSVToRGB(Mathf.Abs(0.0f + (float)i / 1000f), 0, 1 + i / 500f); // No saturation, full brightness (white)
                sampleBin[i].GetComponent<Renderer>().material.color = color;




                //timer resets at the end of the audio
                if (timer == (double)85.368)
                {
                    timer = 0f;
                }


                //if(timer < 10f)
                //{
                //    sampleBin[i].transform.position = new Vector3(scale * Mathf.Sin((float)i / 100f) + AudioSpectrum.samples[i] * scale * scale, 0, 0);
                //}
                //else
                //{
                //    sampleBin[i].transform.position = new Vector3(2 * ((float)i / 100f) + AudioSpectrum.samples[i], 0, 0);
                //}

                //sampleBin[i].transform.position = new Vector3(scale * Mathf.Sin((float)i / 100f) + AudioSpectrum.samples[i] * scale * scale, 0, scale * Mathf.Cos((float)i / 100f) + AudioSpectrum.samples[i] * scale * scale);
                //sampleBin[i].transform.position = new Vector3(2 * ((float)i / 100f) + AudioSpectrum.samples[i], 0, 0);
                // sampleBin[i].transform.Rotate(AudioSpectrum.samples[i], 0f, Mathf.Sin(AudioSpectrum.samples[i]) * AudioSpectrum.samples[i] * scale * scale);
            }
        }

    }
    Stack<GameObject> spawnedCrosswalks = new Stack<GameObject>();
    IEnumerator SpawnShapes() //crosswalk spawn
    {
        Color color;
        for (int i = 0; i < 8; i++)
        {
            crosswalks[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crosswalks[i].transform.SetParent(this.transform); //setting car as parent object
            crosswalks[i].transform.localPosition = new Vector3(i * 1.2f, 0, 7); //position in relation to current position of audio spectrum and car
            crosswalks[i].transform.localScale = new Vector3(0.5f, .2f, 4f); //size

            color = Color.HSVToRGB(Mathf.Abs(0.0f + (float)i / 1000f), 0, 1 + i / 500f); // No saturation, full brightness (white)
            crosswalks[i].GetComponent<Renderer>().material.color = color;
            spawnedCrosswalks.Push(crosswalks[i]); // push each line to a stack

            yield return new WaitForSeconds(0.05f); // each line of the crosswalk appears after 0.05 seconds, one by one, not considering lag which slows the later lines 
        }

    }

    IEnumerator DespawnShapes()
    {
        Debug.Log("DespawnShapes started. Stack count: " + spawnedCrosswalks.Count);
        while (spawnedCrosswalks.Count > 0)
        {
            GameObject obj = spawnedCrosswalks.Pop();
            Destroy(obj);
            Debug.Log("Destroyed object, remaining: " + spawnedCrosswalks.Count);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
