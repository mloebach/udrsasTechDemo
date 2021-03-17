using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using Naninovel;

public class PlayDialogueFromScript : MonoBehaviour
{
    public string myScript = "Script";
    public string myLabel = "Label";

    //cached
    SceneFlow sceneFlow;

    public void swapToDialogue(){
        sceneFlow = FindObjectOfType<SceneFlow>();
        //Debug.Log(sceneFlow);
        sceneFlow.switchToDialogue();
    }
    public void swapToGame(){
        sceneFlow = FindObjectOfType<SceneFlow>();
        //Debug.Log(sceneFlow);
        sceneFlow.switchToGame();
    }

    public void loadDialogue(){
        //Time.timeScale = 0f;
        swapToDialogue();
        var variableManager = Engine.GetService<ICustomVariableManager>();
        var currentScript = variableManager.GetVariableValue("CurrentScript");
        var currentLabel = variableManager.GetVariableValue("CurrentLabel");
        currentScript = myScript;
        currentLabel = myLabel;
        variableManager.SetVariableValue("CurrentScript", currentScript);
        variableManager.SetVariableValue("CurrentLabel", currentLabel);

        var switchCommand = new LoadNaniScript { Script = "GameDialogue"};
	    switchCommand.ExecuteAsync().Forget();
        //swapToGame();
    }

    public void endScene(){
        var switchCommand = new LoadVNScene { nextScript = myScript, nextLabel = myLabel};
	    switchCommand.ExecuteAsync().Forget();
    }

}

