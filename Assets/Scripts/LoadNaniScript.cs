using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using Naninovel.Commands;
using UniRx.Async;

[CommandAlias("loadNaniScene")]
public class LoadNaniScript : Command
{
    // Start is called before the first frame update
    public StringParameter Script;
    public StringParameter Label;

    public override async UniTask ExecuteAsync (CancellationToken cancellationToken = default)
    {
        
        if (Assigned(Script))
        {

            //load camera back
            /*var gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            gameCamera.enabled = false;
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = true;*/

            Debug.Log($"Hello, {Script}!");
            var player = Engine.GetService<IScriptPlayer>();
            await player.PreloadAndPlayAsync(Script, label: Label);
        }
        else
        {
            Debug.Log("No script.");
        }

        //return UniTask.CompletedTask;
    }
}