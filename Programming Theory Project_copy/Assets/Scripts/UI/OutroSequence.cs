using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutroSequence : MonoBehaviour
{
    [SerializeField] List<DialogueBox> dialogueBoxes;
   // [SerializeField] List<Transform> cameraTransforms;
   // [SerializeField] Transform cameraActual;
    [SerializeField] GameObject proceedButton;
    [SerializeField] AudioClip laugh1;
    [SerializeField] AudioClip laugh2;
    [SerializeField] private Animator fader;

    private AudioSource outroAudio;
    private int currentDialogueBox;
    //private int currentTransformIndex = 0;
    private float fadeRate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayOutro());
        outroAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProceedToScoreboard();
        }
    }

    private IEnumerator PlayOutro()
    {
        yield return new WaitForSeconds(1f);
        outroAudio.PlayOneShot(laugh1);
        for (int i = 0; i < dialogueBoxes.Count; i++) //loops through all dialogue boxes and presents them in sequential order
        {
            yield return FadeIn(dialogueBoxes[i].backgroundImage); //fades in background
            
            yield return dialogueBoxes[i].TypeDialog(dialogueBoxes[i].textToBeTyped); //types text
            if (i == 4) //plays evil laugh audio at certain points in sequence
            {
                outroAudio.PlayOneShot(laugh2);
            }
            if (i == 6) //plays evil laugh audio at certain points in sequence
            {
                outroAudio.PlayOneShot(laugh1);
            }
            yield return new WaitForSeconds(2f);
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

    public void ProceedToScoreboard()
    {
        StartCoroutine(FadeThenLoad(6));
    }

    private IEnumerator FadeThenLoad(int buildIndex)
    {
        fader.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(buildIndex);
    }


}
