using UI;
using UnityEngine;

public class BigBoyButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            GetComponent<CanvasTransitionButton>().OnButtonClick();
            Destroy(gameObject);
        }
    }
}
