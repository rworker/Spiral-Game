using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    private int lettersPerSecond = 20;
    public TextMeshProUGUI dialogText;
    public string textToBeTyped;
    public Image backgroundImage;

    // Start is called before the first frame update
    void Awake() //must be in awake to be called ahead of the intro sequence starting
    {
        textToBeTyped = dialogText.text;
        dialogText.text = "";
        //print(textToBeTyped);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }
}
