using UnityEngine;
using UnityEngine.Rendering;

public class walking : MonoBehaviour
{
    public float speed = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        float currentX = currentRotation.x;

        transform.Rotate(new Vector3(0,1,0)*speed*Time.deltaTime );
        if (currentX<70)
        {
            transform.eulerAngles = new Vector3(90f, 0f, 0f);
            transform.Translate(new Vector3(10, 0, 0)*speed*Time.deltaTime);
        }
        
        
    }
}
