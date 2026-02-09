using UnityEngine;

public class HideMainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    
    public GameObject PlayerNameMenu;

    public void CloseMainMenu()
    {
        MainMenu.SetActive(false);
        PlayerNameMenu.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
