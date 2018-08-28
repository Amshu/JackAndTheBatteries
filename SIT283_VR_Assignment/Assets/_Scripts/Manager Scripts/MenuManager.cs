using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    // References the Play and the exit button
    [SerializeField] GameObject play;
    [SerializeField] GameObject exit;

    bool playPress;

    //Reference the 3D Text
    [SerializeField] GameObject uiText;

    new AudioSource audio;
    [SerializeField] AudioClip menuMusic;

    bool ready = false;


    

    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.PlayOneShot(menuMusic,0.3f);
    }

    private void Update()
    {
        // Check if Exit button is pressed
        if (exit.GetComponent<MenuButton>().pressed)
        {
            // Debug.Log("Quit Application");
            Application.Quit();
        }
        // If play is not pressed before
        if (!playPress)// Check to prevent the sound from playing constantly
        {
            if (play.GetComponent<MenuButton>().pressed)
            {
                playPress = true;
                uiText.GetComponent<TextMesh>().text = "Move to the green zone and get ready";
                uiText.GetComponent<TextMesh>().color = Color.green;
                audio.Play();
            }
        }
    }

    // Check if the player is moved to the green zone
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Pokeball" && playPress)
        {
            //Debug.Log("Player1 Ready");
            ready = true; 
            PlayLevel();
            audio.Play();
        }
    }
    
    void PlayLevel()
    {
        // If player has pressed the button and is in the green zone
        if(ready &&  play.GetComponent<MenuButton>().pressed)
        {
            //Load Scene
            SceneManager.LoadScene("level_01");
        }
    }

}


