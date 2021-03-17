using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using Naninovel.Commands;
using UniRx.Async;
using UnityEngine.SceneManagement;

[CommandAlias("loadGameScene")]
public class LoadGameScene : Command
{
    public StringParameter Scene;

    public override UniTask ExecuteAsync (CancellationToken cancellationToken = default)
    {
        if (Assigned(Scene))
        {
            Debug.Log($"Hello, {Scene}!");
            SceneManager.LoadScene(Scene);
            //swap cameras
            var gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            gameCamera.enabled = true;
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = false;
        }
        else
        {
            Debug.Log("Hello World!");
        }

        return UniTask.CompletedTask;
    }
}
