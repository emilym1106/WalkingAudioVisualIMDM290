// Unity Audio Spectrum Plot Example
// IMDM Class Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class AudioSpectrumPlot : MonoBehaviour
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

    public GameObject crosswalkSingle;

    GameObject[] crosswalks = new GameObject[8];

    public Transform carTransform;

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
        {   // Create GO and init position
            sampleBin[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleBin[i].transform.SetParent(this.transform); // make them children of the bars object
            sampleBin[i].transform.localPosition = new Vector3(i * .02f, 0, 0);
            //sampleBin[i].transform.position = new Vector3(i * .02f, 0, 0);
            sampleBin[i].transform.Rotate(90, 0, 0);
            //sampleBin[i].transform.Rotate(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            sampleBin[i].SetActive(false);
        }

        
        targetPos = new Vector3[maxBin];
        for (int i = 0; i < maxBin; i++)
        {
            targetPos[i] = new Vector3(i * 1f, carTransform.position.y, carTransform.position.z);
        }

        
        crosswalkSingle.SetActive(false);
       


    }
    void Update()
    {
        timer += Time.deltaTime;

        bool spawned = false;
        
        for (int i = 0; i < maxBin; i++)
        {
            targetPos[i] = new Vector3(i * 1f, 1f, 0f);
            if (sampleBin != null)
            {
                
                if(timer > 41 && timer < 45f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localPosition = new Vector3(i * 1.2f, 0f, 7f);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, 0.1f);
                }

                if(timer > 45f && timer < 47f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localPosition = new Vector3(i * .02f, 0f, 7f);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                }

                if(timer > 47f && timer < 49f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, targetPos[i], Time.deltaTime * 2f);
                }

                if (timer > 49f && timer < 51f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, - targetPos[i], Time.deltaTime * 2f);
                }

                if (timer > 51f && timer < 53f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, targetPos[i] / 2, Time.deltaTime * 2f);
                }

                if (timer > 53f)
                {
                    sampleBin[i].SetActive(true);
                    sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, AudioSpectrum.samples[i] * scale);
                    sampleBin[i].transform.localPosition = Vector3.Lerp(sampleBin[i].transform.localPosition, targetPos[i] - targetPos[i], Time.deltaTime * 2f);
                }

                if (timer > 53f && !spawned)
                {
                    crosswalkSingle.SetActive(true);
                    sampleBin[i].SetActive(false);
                    StartCoroutine(SpawnShapes());
                }



                //sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, 0.1f);
                //sampleBin[i].GetComponent<Renderer>().material.color = new Color(0.3f + (float)i / 100f, 0.1f + i / 30f, 1+ i / 500f);
                Color color;
                
                    color = Color.HSVToRGB(Mathf.Abs(0.0f + (float)i / 1000f), 0, 1 + i / 500f); // No saturation, full brightness (white)
                    sampleBin[i].GetComponent<Renderer>().material.color = color;

                    
                    
                
               
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

    IEnumerator SpawnShapes()
    {
        Color color;
        for (int i = 0; i < 8; i++)
            {
                crosswalks[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                crosswalks[i].transform.SetParent(this.transform);
             crosswalks[i].transform.localPosition = new Vector3(i * 1.2f, 0, 0);
                crosswalks[i].transform.localScale = new Vector3(0.5f, .2f, 4f);

            color = Color.HSVToRGB(Mathf.Abs(0.0f + (float)i / 1000f), 0, 1 + i / 500f); // No saturation, full brightness (white)
            crosswalks[i].GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(0.05f);
            }
        
        
    }
}