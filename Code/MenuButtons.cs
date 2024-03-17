using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Sources:*/
/*https://gamedevacademy.org/unity-start-menu-tutorial/*/
/*https://www.sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas*/
public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject ExitMenu;

    public void ButtonPlay () {
        SceneManager.LoadScene(1);
    }

    public void ButtonOptions () {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void ButtonExit () {
        MainMenu.SetActive(false);
        ExitMenu.SetActive(true);
    }

    public void ButtonBack () {
        ExitMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void ButtonQuit () {
        Application.Quit();
    }
}

