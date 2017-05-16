using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entitas;
using System.Text;

public class GUIController : MonoBehaviour {

    public Text log;

    void Start () {
        IGroup<GameEntity> group = Contexts.sharedInstance.game.GetGroup(GameMatcher.Log);
        var logEntity = group.GetSingleEntity();
        logEntity.OnComponentReplaced += Log_OnComponentReplaced;
    }

    private void Log_OnComponentReplaced(IEntity entity, int index, IComponent previous, IComponent next)
    {
        var logComp = (LogComponent)next;
        var builder = new StringBuilder();

        foreach (var message in logComp.queue)
        {
            builder.Append(message + System.Environment.NewLine);
        }

        log.text = builder.ToString();
    }
}
