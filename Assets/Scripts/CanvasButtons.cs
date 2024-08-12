using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CanvasButtons : MonoBehaviour
{
    public Sprite btn, btnPressed, musicOn, musicOff;
    private Image image;
    
    private void Start()
    {
        image = GetComponent<Image>();

        if(gameObject.name == "SoundBtn"){
            if(PlayerPrefs.GetString("Music") == "Off")
                {transform.GetChild(0).GetComponent<Image>().sprite = musicOff;}
        }
    }

    public void MusicButton(){
        if(PlayerPrefs.GetString("Music") == "Off"){    
            PlayerPrefs.SetString("Music", "On");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOn;
        }
        else {
            PlayerPrefs.SetString("Music", "Off");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
        PlayBtnSound();
        
    }
    public void ShopScene(){
        StartCoroutine(LoadScene("Shop"));
        PlayBtnSound();
    }

    public void ExitBtn(){
        StartCoroutine(LoadScene("Menu"));
        PlayBtnSound();
    }
    public void PlayGame()
    {
        if(PlayerPrefs.GetString("Study Completed") == "No")
            StartCoroutine(LoadScene("Game"));
        else
        {
            PlayerPrefs.SetString("Study Completed", "No");
            StartCoroutine(LoadScene("Study"));
        }

        PlayBtnSound();
    }

    public void RestartGame()
    {
        CarController.isLose = false;
        CarController._countCars = 0;
        StartCoroutine(LoadScene("Game"));
        PlayBtnSound();
    }
       
    public void SetPressedButton()
    {
        image.sprite = btnPressed;
        transform.GetChild(0).localPosition -= new Vector3(0, 8f, 0);
    }
    public void SetDefaultButton()
    {
        image.sprite = btn;
        transform.GetChild(0).localPosition += new Vector3(0, 8f, 0);
    }

    IEnumerator LoadScene(string name)
    {
        float fadeTime = Camera.main.GetComponent<Fading>().Fade(1f);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(name);
    }

    private void PlayBtnSound(){
        if(PlayerPrefs.GetString("Music") != "Off")
            GetComponent<AudioSource>().Play();
    }
}
