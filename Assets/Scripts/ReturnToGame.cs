using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using Naninovel.Commands;
using UniRx.Async;

[CommandAlias("returnToGame")]
public class ReturnToGame : Command
{
    public override async UniTask ExecuteAsync (CancellationToken cancellationToken = default)
    {
        //Debug.Log("testingA");
        var sceneFlow = Object.FindObjectOfType<SceneFlow>();
        sceneFlow.switchToGame();
        //Debug.Log("testingB");
        /*
        if (Assigned(Script))
        {

            
        }
        else
        {
            Debug.Log("No script.");
        }*/

        //return UniTask.CompletedTask;
    }
}