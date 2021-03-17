using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFlow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    [SerializeField] GameObject UICanvas;
    public void switchToDialogue(){
        //stop player movement
        //player.SetActive(false);
        PlayerMove.canMove = false;
        //hide canvas
        UICanvas.SetActive(false);
    }
     public void switchToGame(){
        //stop player movement
        //player.SetActive(true);
        PlayerMove.canMove = true;
        //hide canvas
        UICanvas.SetActive(true);
    }
}
