using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsVisual : MonoBehaviour
{
    [SerializeField] private Sprite heart0Sprite;
    [SerializeField] private Sprite heart1Sprite;
    [SerializeField] private Sprite heart2Sprite;
    [SerializeField] private Sprite heart3Sprite;
    [SerializeField] private Sprite heart4Sprite;
    [SerializeField] private Image heartBackground;

    [SerializeField] private PlayerController player;

    private List<HeartImage> heartImageList;
    private HeartsSystem heartsSystem;


    private void Awake() 
    {
        heartImageList = new List<HeartImage>(); //instantiates list
    }
    private void Start() 
    {
        heartsSystem = player.playerHearts; //assigns player heart system to heartsystem
        SetHeartsSystem(heartsSystem);

    }

    //for testing
    public void Damage1()
    {
        heartsSystem.Damage(1);
    }
    public void Damage4()
    {
        heartsSystem.Damage(4);
    }

    public void Heal1()
    {
        heartsSystem.Heal(1);
    }

    //creates hearts in UI (sprite varies) based on information from heartsystem class such as number of hearts and their fragments
    public void SetHeartsSystem (HeartsSystem heartsSystem) //the heartsSystem of this class equals the heartsSystem parameter
    {
        this.heartsSystem = heartsSystem;

        List<HeartsSystem.Heart> heartList = heartsSystem.GetHeartList();
        Vector2 heartAnchorPosition = new Vector2 (0,0);
        //ector2 backgroundScale = new Vector2(0,50);
        for (int i = 0; i < heartList.Count; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            CreateHeartImage(heartAnchorPosition).SetHeartFragments(heart.GetFragmentAmount());

            heartAnchorPosition +=  new Vector2(20, 0); //spaces out each heart by 30 along x axis
            //backgroundScale += new Vector2(40,0);
        }

        heartsSystem.OnDamaged += HeartsSystem_OnDamaged; //subscribes function
        heartsSystem.OnHealed += HeartsSystem_OnHealed; //subscribes function

    }

    //reduces hearts in UI if hearts take damage in the heart system
    private void HeartsSystem_OnDamaged(object sender, System.EventArgs e) 
    {
        //if hearthealth system was damaged
        RefreshAllHearts();
    }

    private void HeartsSystem_OnHealed(object sender, System.EventArgs e)
    {
        //if heart system was healed
        RefreshAllHearts();
    }

    private void RefreshAllHearts()
    {
        //if hearthealth system was altered (damaged or healed)
        List<HeartsSystem.Heart> heartList = heartsSystem.GetHeartList();
        for (int i = 0; i < heartImageList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i]; //sets heartImage to current heartImage (from this class) at index i
            HeartsSystem.Heart heart = heartList[i]; //sets heart to current heart (from heartsystem class) at the same index
            heartImage.SetHeartFragments(heart.GetFragmentAmount()); //sets heartImage fragments to the fragments of heart
        }
    }


    //logic for creating each heart in the UI
    private HeartImage CreateHeartImage(Vector2 anchoredPosition)
    {
        //create game object
        GameObject heartGameObject = new GameObject("Heart", typeof(Image));
        //set as child of transform
        heartGameObject.transform.parent = transform;
        heartGameObject.transform.localPosition = Vector3.zero;

        //locate and size heart
        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35); //sets size of each heart
        //set heart image on UI
        Image heartImageUI = heartGameObject.GetComponent<Image>();
        heartImageUI.sprite = heart0Sprite;

        // add heart image to list of heartImages
        HeartImage heartImage = new HeartImage(this, heartImageUI); //this refers to this HeartVisual class (HeartImage constructor requires a heartvisual parameter)
        heartImageList.Add(heartImage);

        return heartImage;
    }

    //represents a single heart, keeps track of UI image based on fragments
    public class HeartImage
    {
        private HeartsVisual heartsVisual;
        private Image heartImage;

        public HeartImage(HeartsVisual heartsVisual, Image heartImage) //onstructor to receive UI image
        {
            this.heartsVisual = heartsVisual;
            this.heartImage = heartImage;
        }

        public void SetHeartFragments(int fragments) //switch statement for setting the heart sprite depending on the number of fragments
        {
            switch (fragments) 
            {
                case 0: heartImage.sprite = heartsVisual.heart0Sprite; break;
                case 1: heartImage.sprite = heartsVisual.heart1Sprite; break;
                case 2: heartImage.sprite = heartsVisual.heart2Sprite; break;
                case 3: heartImage.sprite = heartsVisual.heart3Sprite; break;
                case 4: heartImage.sprite = heartsVisual.heart4Sprite; break;
            }
        }
    }
}
