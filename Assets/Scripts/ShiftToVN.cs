using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using Naninovel.Commands;
using UniRx.Async;
using UnityEngine.SceneManagement;

[CommandAlias("loadVNScene")]
public class LoadVNScene : Command
{
    public StringParameter nextScript;
    public StringParameter nextLabel;
    public StringParameter toScene = "NaniScene";

    public override UniTask ExecuteAsync (CancellationToken cancellationToken = default)
    {
        if (Assigned(nextScript))
        {
            
            //swap cameras
            var gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            gameCamera.enabled = false;
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = true;

            //load naninovelsafe scene
            Debug.Log($"Hello, {toScene}!");
            SceneManager.LoadScene(toScene);

            //load next script
            var switchCommand = new LoadNaniScript { Script = nextScript, Label = nextLabel};
	        switchCommand.ExecuteAsync().Forget();
        }
        else
        {
            Debug.Log("Hello World!");
        }

        return UniTask.CompletedTask;
    }
}
