using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] List<DialogueBox> dialogueBoxes;
    [SerializeField] List<Transform> cameraTransforms;
    [SerializeField] Transform cameraActual;
    [SerializeField] GameObject proceedButton;
    [SerializeField] AudioClip laugh1;
    [SerializeField] AudioClip laugh2;
    [SerializeField] private Animator fader;

    private AudioSource introAudio;
    private int currentDialogueBox;
    private int currentTransformIndex = 0;
    private float fadeRate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayIntro());
        introAudio = GetComponent<AudioSource>();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProceedToMenu();
        }
    }

    private IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(1f);
        introAudio.PlayOneShot(laugh1);
        for (int i = 0; i < dialogueBoxes.Count; i++) //loops through all dialogue boxes and presents them in sequential order
        {
            yield return FadeIn(dialogueBoxes[i].backgroundImage); //fades in background
            if ( i == 6) //plays evil laugh audio at certain points in sequence
            {
                introAudio.PlayOneShot(laugh2);
            }
            yield return dialogueBoxes[i].TypeDialog(dialogueBoxes[i].textToBeTyped); //types text
            if (i == 12) //plays evil laugh audio at certain points in sequence
            {
                introAudio.PlayOneShot(laugh1);
            }
            yield return new WaitForSeconds(1f);
            if (i == 2 || i == 5 || i == 8 || i == 10) //moves camera at certain points in the sequence
            {
               yield return CameraMove(2f);
            }
        }
        proceedButton.SetActive(true); //sets proceed buton to true
    }

    private IEnumerator FadeIn(Image image)
    {
        float targetAlpha = 1.0f;
        Color curColor = image.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeRate * Time.deltaTime);
            image.color = curColor;
            yield return null;
        }
    }

    private IEnumerator CameraMove(float duration)
    {
        
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            cameraActual.position = Vector3.Lerp(cameraTransforms[currentTransformIndex].position, cameraTransforms[currentTransformIndex + 1].position, t / duration);
            yield return null;
        }
        currentTransformIndex++;
    }

    public void ProceedToMenu()
    {
        StartCoroutine(FadeThenLoad(1));
    }

    private IEnumerator FadeThenLoad(int buildIndex)
    {
        fader.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(buildIndex);
    }

    
}
