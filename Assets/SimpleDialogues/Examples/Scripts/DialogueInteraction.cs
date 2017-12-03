using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueInteraction : MonoBehaviour {

    [SerializeField]
    Dialogues npc;

    [SerializeField]
    Text dialogueText;
    [SerializeField]
    Text leftText;
    [SerializeField]
    Text rightText;
    [SerializeField]
    Text middleText;
    [SerializeField]
    GameObject backPanel;
    [SerializeField]
    GameObject ship;

    bool nextEnd = false;

    private Color defaultBackPanelColor;
    private Dictionary<string, Transform> npcDictionary;

    // Simple Dialogues //
    // This is a basic example of how you can use the dialogue system. //

    
	void Start() {
        //npc.SetTree("FirstMeeting"); //This sets the current tree to be used. Resets to the first node when called.
        defaultBackPanelColor = backPanel.GetComponent<Image>().color;
        Display();
        npcDictionary = new Dictionary<string, Transform>();
        for (int i = 0; i < npc.transform.childCount; i++)
        {
            Transform child = npc.transform.GetChild(i);
            if (child != null)
            {
                npcDictionary.Add(child.name, child);
            }
        }
	}

    public void Choice(int index)
    {
        if (index == 2 && npc.GetCurrentTree() == "TalkAgain") index = 1;

        if (index == 1 && (npc.GetCurrentTree() == "Scene1" || npc.GetCurrentTree() == "Scene2") && npc.GetChoices().Length == 1) index = 0;
        if (index == 2 && (npc.GetCurrentTree() == "Scene1" || npc.GetCurrentTree() == "Scene2") && npc.GetChoices().Length == 2) index = 1;
        if (npc.GetChoices().Length != 0)
        {
            npc.NextChoice(npc.GetChoices()[index]); //We make a choice out of the available choices based on the passed index.
            Display();                               //We actually call this function on the left and right button's onclick functions
        }
        else
        {
            Progress();
        }
    }

    public void TalkAgain()
    {
        npc.SetTree("TalkAgain");
        nextEnd = false;
        Display();
    }

    // Custom code
    public void MainSotry()
    {
        npc.SetTree("Scene1");
        nextEnd = false;
        Display();
    }

    public void MainSotry2()
    {
        npc.SetTree("Scene2");
        nextEnd = false;
        Display();
    }

    public void Progress()
    {
        npc.Next(); //This function returns the number of choices it has, in my case I'm checking that in the Display() function though.
        Display();
    }

    public void Display()
    {
        if (nextEnd == true)
        {
            backPanel.SetActive(false);
        }
        else
        {
            backPanel.SetActive(true);
        }

        if (npc.GetCurrentDialogue().Equals(""))
        {
            backPanel.GetComponent<Image>().color = Color.clear;
        }
        else
        {
            backPanel.GetComponent<Image>().color = defaultBackPanelColor;
        }
        //Sets our text to the current text
        dialogueText.text = npc.GetCurrentDialogue();
        //Just debug log our triggers for example purposes
        if (npc.HasTrigger())
        {
            Debug.Log("Triggered: " + npc.GetTrigger());
            handleTrigger(npc.GetTrigger());
        }
        //This checks if there are any choices to be made
        
        if (npc.GetChoices().Length != 0)
        {
            
            if (npc.GetChoices().Length == 1)
            {
                leftText.transform.parent.gameObject.SetActive(false);
                middleText.text = npc.GetChoices()[0];
                middleText.transform.parent.gameObject.SetActive(true);
                rightText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                //Setting the text's of the buttons to the choices text, in my case I know I'll always have a max of three choices for this example.
                leftText.text = npc.GetChoices()[0];
                middleText.text = npc.GetChoices()[1];
                //If we only have two choices, adjust accordingly
                if (npc.GetChoices().Length > 2)
                    rightText.text = npc.GetChoices()[2];
                else
                    rightText.text = npc.GetChoices()[1];
                //Setting the appropriate buttons visability
                leftText.transform.parent.gameObject.SetActive(true);
                rightText.transform.parent.gameObject.SetActive(true);
                if (npc.GetChoices().Length > 2)
                    middleText.transform.parent.gameObject.SetActive(true);
                else
                    middleText.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            middleText.text = "Continue";
            //Setting the appropriate buttons visability
            leftText.transform.parent.gameObject.SetActive(false);
            rightText.transform.parent.gameObject.SetActive(false);
            middleText.transform.parent.gameObject.SetActive(true);
        }
        
        if (npc.End()) //If this is the last dialogue, set it so the next time we hit "Continue" it will hide the panel
            nextEnd = true;
    }

    private void handleTrigger(string triggerName)
    {
        Transform actor = getNPC(triggerName);
        if(actor != null)
        {
            triggerNPC(actor, triggerName);
        }
        
        if(triggerName.Contains("StartMove"))
        {
            string[] moveTrigger = triggerName.Split(',');
            float speed = float.Parse(moveTrigger[1]);
            float duration = float.Parse(moveTrigger[2]);
            float dirX = float.Parse(moveTrigger[3]);
            float dirY = float.Parse(moveTrigger[4]);
            float dirZ = float.Parse(moveTrigger[5]);

            MoveShip moveShip = ship.GetComponent<MoveShip>();
            moveShip.moveBySpeedDirection(speed, duration, new Vector3(dirX, dirY, dirZ));
        }
        if (triggerName.Contains("EndMove"))
        {
            MoveShip moveShip = ship.GetComponent<MoveShip>();
            moveShip.stopMoving();
        }
    }

    private void triggerNPC(Transform actingNPC, string npcCommand)
    {
        if (npcCommand.Contains("Talk"))
        {
            INPCAction npcAction = (INPCAction)actingNPC.GetComponent(typeof(INPCAction));
            npcAction.setActive();
            npcAction.talk();
        }
        if (npcCommand.Contains("End"))
        {
            INPCAction npcAction = (INPCAction)actingNPC.GetComponent(typeof(INPCAction));
            npcAction.setDeactive();
        }
    }

    private Transform getNPC(string npcName)
    {
        Transform actingNPC = null;
        if(npcName.Contains("Alien"))
        {
            actingNPC = npcDictionary["exo_gray"];
        }
        else if (npcName.Contains("XO"))
        {
            actingNPC = npcDictionary["Second_Officer"];
        }
        else if (npcName.Contains("Pilot"))
        {
            actingNPC = npcDictionary["Pilot"];
        }
        else if (npcName.Contains("Nav"))
        {
            actingNPC = npcDictionary["Navigator"];
        }
        else if (npcName.Contains("Comms"))
        {
            actingNPC = npcDictionary["Comm_Officer"];
        }
        else if (npcName.Contains("Science"))
        {
            actingNPC = npcDictionary["Chief_Science_Officer"];
        }
        else if (npcName.Contains("Engineer"))
        {
            actingNPC = npcDictionary["Chief_Engineering_Officer"];
        }
        else if (npcName.Contains("Tacticts"))
        {
            actingNPC = npcDictionary["Chief_Tactical_Officer"];
        }
        return actingNPC;
    }
}
