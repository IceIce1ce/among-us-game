﻿using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class VentsSystem : MonoBehaviour
{
    //The Vent UI Button
    [SerializeField] Button VentUIButton;

    //All the Vents in this VentsSystem
    [SerializeField] List<Vent> connectedVents;
    int CurrentVentID;

    //Other Classes
    AU_PlayerController au_player; //aaa
    //PlayerMovement playerMovement;
    ArrowsManager arrowsManager;

    void Awake()
    {
        arrowsManager = FindObjectOfType<ArrowsManager>();
        SetupVents();
    }

    //Give all vents an ID in this VentsSystem
    public void SetupVents()
    {
        int index = 0;
        foreach (Vent vent in connectedVents)
        {
            vent.ID = index;
            vent.ventsSystem = this;
            index++;
        }
    }

    //When a Player Enters a trigger of a vent in this Vents system
    public void CanEnterVentSystem(AU_PlayerController playerMovement, int VentID)
    {
        this.au_player = playerMovement;
        CurrentVentID = VentID;

        //Make the button clickable and add the function to enter the vents system when clicked
        VentUIButton.interactable = true;
        VentUIButton.onClick.RemoveAllListeners();
        VentUIButton.onClick.AddListener(() => EnterIntoTheVentSystem());
    }

    //When a Player Exits a trigger of a vent in this Vents system
    public void CantEnterVentSystem()
    {
        //Make the button UnClickable
        VentUIButton.interactable = false;
    }

    //Called when Player presses the vent button
    public void EnterIntoTheVentSystem()
    {
        //Make the Player Enter the Vent
        au_player.EnterVent(this);
        au_player.gameObject.transform.position = connectedVents[CurrentVentID].GetPos();

        //Change the Button Listener function to the ExitTheVentSystem function
        VentUIButton.onClick.RemoveAllListeners();
        VentUIButton.onClick.AddListener(() => ExitTheVentSystem());

        //Call This here if you just want to skip the Player Animation or if he had nothing
        //arrowsManager.VentEntered(this, CurrentVentID, connectedVents);
    }

    //Player Animation is Done and the arrows appear to move through the vents
    //[Can be skipped if the player don't have anitmation look to "EnterIntoTheVentSystem()" function]
    public void PlayerInVent()
    {
        //Draw the Arrows
        arrowsManager.VentEntered(this, CurrentVentID, connectedVents);
    }

    //Called when Player presses the vent button when he's inside
    public void ExitTheVentSystem()
    {
        //Remove the Arrows
        arrowsManager.ResetArrows();

        //Make the Player Exit the Vent
        au_player.VentExited();
        au_player.gameObject.transform.position = connectedVents[CurrentVentID].GetPos();

        //Change the Button Listener function to the EnterIntoTheVentSystem function
        VentUIButton.onClick.RemoveAllListeners();
        VentUIButton.onClick.AddListener(() => EnterIntoTheVentSystem());
    }

    //Called when pressing the Arrows
    public void MoveToVent(int ventID)
    {
        //Set the Current Vent ID to the new Vent ID
        CurrentVentID = ventID;

        //Move the Player to that Vent
        au_player.gameObject.transform.position = connectedVents[ventID].GetPos();

        //Reset the Arrows for the next Vent
        arrowsManager.ResetArrows();
        arrowsManager.VentEntered(this, CurrentVentID, connectedVents);
    }

    //Draw the Red Lines between the Vents to see how they are connected
    //[Not Important] it's just for testing, and can be removed
    private void OnDrawGizmos()
    {
        for (int i = 0; i < connectedVents.Count; i++)
            for (int j = 0; j < connectedVents.Count; j++)
                if (connectedVents[j].ID != connectedVents[i].ID)
                    Debug.DrawLine(connectedVents[i].GetPos(), connectedVents[j].GetPos(), Color.red);
    }
}
