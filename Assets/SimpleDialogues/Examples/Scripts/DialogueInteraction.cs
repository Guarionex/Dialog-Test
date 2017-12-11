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
    [SerializeField]
    GameObject skyboxes;

    bool nextEnd = false;

    private Color defaultBackPanelColor;
    private Dictionary<string, Transform> npcDictionary;

    // Simple Dialogues //
    // This is a basic example of how you can use the dialogue system. //

    
	void Start() {
        npc.SetTree("Scene1"); //This sets the current tree to be used. Resets to the first node when called.
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
        if (index == 1 && (npc.GetCurrentTree() == "Scene1" || npc.GetCurrentTree() == "Scene2") && npc.GetChoices().Length == 1) index = 0;
        if (index == 2 && (npc.GetCurrentTree() == "Scene1" || npc.GetCurrentTree() == "Scene2") && npc.GetChoices().Length == 2) index = 1;
        if (npc.GetChoices().Length != 0)
        {
            npc.NextChoice(npc.GetChoices()[index]); //We make a choice out of the available choices based on the passed index.
            Display();                               //We actually call this function on the left and right button's onclick functions
        }
        else
        {
            if (nextEnd)
            {
                endScene();
                Display();
            }
            else
            {
                Progress();
            }
        }
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
        if (npc.HasTrigger() && !nextEnd)
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
        List<string> parsedCommandsList = parseCommands(triggerName);
        List<KeyValuePair<Transform, string>> actorActions = getNPC(parsedCommandsList);
        if(actorActions.Count > 0)
        {
            triggerNPC(actorActions);
        }

        moveShipCommand(parsedCommandsList);
    }

    private void triggerNPC(List<KeyValuePair<Transform, string>> npcActions)
    {
        foreach (KeyValuePair<Transform, string> actorWithAction in npcActions)
        {
            Transform actor = actorWithAction.Key;
            string action = actorWithAction.Value;
            INPCAction npcAction = (INPCAction)actor.GetComponent(typeof(INPCAction));
            if (action.Contains("Talk"))
            {
                npcAction.talk();
            }
            else if (action.Contains("End"))
            {
                npcAction.setDeactive();
            }
            else if (action.Contains("Idle"))
            {
                npcAction.idle();
            }
        }
    }

    private List<KeyValuePair<Transform, string>> getNPC(List<string> commandList)
    {
        List<KeyValuePair<Transform, string>> npcActions = new List<KeyValuePair<Transform, string>>();
        foreach (string command in commandList)
        {
            Transform actor = null;
            if (command.Contains("Alien"))
            {
                actor = npcDictionary["exo_gray"];
            }
            else if (command.Contains("XO"))
            {
                actor = npcDictionary["Second_Officer"];
            }
            else if (command.Contains("Pilot"))
            {
                actor = npcDictionary["Pilot"];
            }
            else if (command.Contains("Nav"))
            {
                actor = npcDictionary["Navigator"];
            }
            else if (command.Contains("Comms"))
            {
                actor = npcDictionary["Comm_Officer"];
            }
            else if (command.Contains("Science"))
            {
                actor = npcDictionary["Chief_Science_Officer"];
            }
            else if (command.Contains("Engineer"))
            {
                actor = npcDictionary["Chief_Engineering_Officer"];
            }
            else if (command.Contains("Tactical"))
            {
                actor = npcDictionary["Chief_Tactical_Officer"];
            }

            if (actor != null)
            {
                KeyValuePair<Transform, string> npcToCommand = new KeyValuePair<Transform, string>(actor, command);
                npcActions.Add(npcToCommand);
            }
        }
        return npcActions;
    }

    private List<string> parseCommands(string commands)
    {
        List<string> commandsList = new List<string>();
        string[] splittedCommands = commands.Split(';');
        commandsList.AddRange(splittedCommands);
        return commandsList;
    }

    private void moveShipCommand(List<string> commands)
    {
        List<KeyValuePair<string, List<float>>> shipActions = parseShipMovement(commands);
        MoveShip moveShip = ship.GetComponent<MoveShip>();
        foreach (KeyValuePair<string, List<float>> moveCommandToValues in shipActions)
        {
            if (moveCommandToValues.Key.Contains("StartMove"))
            {
                float speed = moveCommandToValues.Value[0];
                float duration = moveCommandToValues.Value[1];
                float dirX = moveCommandToValues.Value[2];
                float dirY = moveCommandToValues.Value[3];
                float dirZ = moveCommandToValues.Value[4];

                moveShip.moveBySpeedDirection(speed, duration, new Vector3(dirX, dirY, dirZ));
            }
            if (moveCommandToValues.Key.Contains("EndMove"))
            {
                moveShip.stopMoving();
            }
            if (moveCommandToValues.Key.Contains("Rotate"))
            {
                float speed = moveCommandToValues.Value[0];
                float dirX = moveCommandToValues.Value[2];
                float dirY = moveCommandToValues.Value[3];
                float dirZ = moveCommandToValues.Value[4];

                moveShip.moveBySpeedDirection(speed, 0, new Vector3(dirX, dirY, dirZ));
            }
            if(moveCommandToValues.Key.Contains("SetRotation"))
            {
                float dirX = moveCommandToValues.Value[2];
                float dirY = moveCommandToValues.Value[3];
                float dirZ = moveCommandToValues.Value[4];

                moveShip.stopMoving();
                moveShip.setRotationTo(new Vector3(dirX, dirY, dirZ));
            }
            if(moveCommandToValues.Key.Contains("HyperDrive"))
            {
                SkyboxFader fader = skyboxes.GetComponent<SkyboxFader>();
                fader.swapToSkyBox((int) moveCommandToValues.Value[0], moveCommandToValues.Value[1]);
            }
        }
    }

    private List<KeyValuePair<string, List<float>>> parseShipMovement(List<string> commands)
    {
        List<KeyValuePair<string, List<float>>> shipActions = new List<KeyValuePair<string, List<float>>>();
        foreach (string command in commands)
        {
            if (command.Contains("Ship"))
            {
                string[] moveTrigger = command.Split(',');
                List<float> vector = new List<float>();
                string moveType = moveTrigger[0];
                for (int i = 1; i < moveTrigger.Length; i++)
                {                    
                    vector.Add(float.Parse(moveTrigger[i]));
                }
                shipActions.Add(new KeyValuePair<string, List<float>>(moveType, vector));
            }
        }
        return shipActions;
    }

    private void endScene()
    {
        if(npc.GetCurrentTree() == "Scene1")
        {
            SkyboxFader fader = skyboxes.GetComponent<SkyboxFader>();
            fader.endFade();
            MainSotry2();
        }
    }
}
