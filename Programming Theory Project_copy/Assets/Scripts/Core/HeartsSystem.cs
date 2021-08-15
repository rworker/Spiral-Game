using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeartsSystem //public so can be referenced in the heartvisual class
{
    public const int MAX_FRAGMENT_AMOUNT = 4; //const since this should not be able to change from 4

    public event EventHandler OnDamaged; //could probably replace this with an Action
    public event EventHandler OnHealed; //could probably replace this with an Action
    public event EventHandler OnDead;


    private List<Heart> heartList;

    public HeartsSystem(int heartAmount)
    {
        heartList = new List<Heart>();
        for (int i = 0; i < heartAmount; i++)
        {
            Heart heart = new Heart(4); //each heart has 4 fragments
            heartList.Add(heart);
        }

        //heartList[heartList.Count - 1].SetFragments(0); sets last heart to 0 fragments for test purposes
    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    //logic for damage to hearts
    public void Damage(int damageAmount)
    {
        for (int i = heartList.Count - 1; i >= 0; i--) //cycles through hearts starting at the last one
        {
            Heart heart = heartList[i];
            //test if this heart can absorb the damage
            if (damageAmount > heart.GetFragmentAmount()) //if the damage is greater than current heart fragments in the given heart
            {   //heart can't absorb the full damage
                damageAmount -= heart.GetFragmentAmount(); //reduces damage amount by fragment amount (this will then carry over to the next heart in the sequence)
                heart.Damage(heart.GetFragmentAmount()); //damages heart by current fragment amount (basically reduces it to 0)

                //move on to next heart
            }
            else
            {   //heart can absorb all the damage
                heart.Damage(damageAmount);
                break;
            }
        }

        //triggers if OnDamaged has subscribers
        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);

        //checks if whoever this health system belongs to is dead and triggers ondead event if it has subscribers
        if (IsDead())
        {
            if (OnDead != null) OnDead(this, EventArgs.Empty);
        }
    }

    public int GetTotalFragments() //used to get total current heart fragments
    {
        int totalHeartFragments = 0;
        foreach (var heart in heartList)
        {
            totalHeartFragments += heart.GetFragmentAmount();
        }
        return totalHeartFragments;
    }

    public bool IsDamaged() //used to check if player currently is missing any health
    {
        if (GetTotalFragments() < heartList.Count * 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int healAmount)
    {
        for (int i = 0; i < heartList.Count; i++) //cycles through hearts starting at first heart and heals all of them that require healing until the healing amount runs out or all hearts are healed, will stop once all hearts are cycled through
        {
            Heart heart = heartList[i];
            int missingFragments = MAX_FRAGMENT_AMOUNT - heart.GetFragmentAmount(); //gets current missing fragement amount
            if (healAmount > missingFragments) 
            {   //if the current heart will be filled, fills remaining fragements for that heart up to the max and reduces the healamount by the fragments healed
                healAmount -= missingFragments; 
                heart.Heal(missingFragments); //heals missing fragments if healamount exceeds missing fragements
            }
            else
            {
                heart.Heal(healAmount);
                break; //breaks cycle if heal amount runs out
            }
        }
        if (OnHealed != null) OnHealed(this, EventArgs.Empty); //triggers OnHealed if has subscribers

    }

    public bool IsDead()
    {
        return heartList[0].GetFragmentAmount() == 0; //checks if first heart (aka the last one that can lose health) is empty. If so, the character is dead and the function returns true
    }

    //represents a single heart
    public class Heart //public so can be referenced in the heartvisual class
    {
        private int fragments;

        public Heart (int fragments) //constructor for hearts in which you set the number of fragments
        {
            this.fragments = fragments;
        }

        public int GetFragmentAmount()
        {
            return fragments;
        }

        public void SetFragments(int fragments)
        {
            this.fragments = fragments;
        }

        public void Damage (int damageAmount) //allows heart to take damage
        {
            if (damageAmount >= fragments)  //makes it so hearts can't be damaged to lower than 0 fragements
            {
                fragments = 0;
            }
            else
            {
                fragments -= damageAmount;
            }
        }

        public void Heal (int healAmount)
        {
            if (fragments + healAmount > MAX_FRAGMENT_AMOUNT) //makes it so hearts can never have their fragements healed above max fragement amount of 4 (restricts fragements to a max of 4))
            {
                //if heal amount + the current fragments exceeds the max fragments
                fragments = MAX_FRAGMENT_AMOUNT;
            }
            else
            {
                fragments += healAmount;
            }
        }

        
    }

    
}
