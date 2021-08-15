using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject directionsMenu;
    [SerializeField] private Animator fader;
    private string placeholder;
    [SerializeField] TextMeshProUGUI warningText;

    private void Start() 
    {
        placeholder = nameText.text;
    }


    public void StartGame()
    {
        
        if (nameText.text != placeholder)
        {
            StoreName();
            print(MainManager.Instance.Name);
            StartCoroutine(FadeThenLoad(2));
        }
        else
            warningText.gameObject.SetActive(true);
    }

    public void ViewScoreBoard()
    {
        StartCoroutine(FadeThenLoad(6));
    }

    public void StoreName()
    {
        MainManager.Instance.Name = nameText.text;
    }

    public void ToggleDirections()
    {
        startMenu.SetActive(false);
        directionsMenu.SetActive(true);
    }

    public void ToggleControls()
    {
        startMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    private IEnumerator FadeThenLoad(int buildIndex)
    {
        fader.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(buildIndex);
    }

    public void ReturnToMenu(GameObject currentMenu)
    {
        currentMenu.SetActive(false);
        startMenu.SetActive(true);

    }
}
