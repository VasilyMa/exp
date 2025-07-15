using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCanvas : SourceCanvas
{
    public override void InvokeCanvas()
    {
        base.InvokeCanvas();

        OpenPanel<BattlePanel>();
    }
}
