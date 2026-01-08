using UnityEngine;

public class BoardCamera : MonoBehaviour
{
    public float degree = 0f;

    private float desiredDegree;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desiredDegree = degree;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(degree);

        if (Input.GetKeyDown(KeyCode.A))
        {
            desiredDegree -= 45;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            desiredDegree += 45; 
        }
        
        degree = Mathf.Lerp(degree, desiredDegree, Time.deltaTime * 10f);
        MoveCamera(degree);
    }

    private void MoveCamera(float angleDegrees)
    { 
        Vector3 center = Vector3.zero;
 
        float radius = 6f;

        float angleRad = angleDegrees * Mathf.Deg2Rad;

        float x = Mathf.Sin(angleRad) * radius;
        float z = Mathf.Cos(angleRad) * radius;

        transform.position = new Vector3(x, transform.position.y, z);
        transform.LookAt(center);
    }
}
