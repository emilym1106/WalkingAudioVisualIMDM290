using UnityEngine;

public class CarPulse : MonoBehaviour
{
    AudioSource song;
    public float minScale = 1f;
    public float maxScale = 5f;
    public float sensitivity = 10f;
    public float speed = 10f;
    public GameObject car;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        song = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        float[] spectrum = new float[128];
        song.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        float loudness = 0f;
        foreach (var sample in spectrum) 
        { 
            loudness += sample;
        }
        loudness /= 128;

        float pulse = minScale + (loudness * sensitivity);
        pulse = Mathf.Clamp(pulse, minScale, maxScale);

        car.transform.localScale = Vector3.Lerp(car.transform.localScale, new Vector3(pulse, pulse, pulse), Time.deltaTime * speed);

    }
}
