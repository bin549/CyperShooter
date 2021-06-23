using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cooperation : Button
{
    enum Selection
    {
        Normal,
        Highlighted,
        Pressed,
        Disabled
    }
    Selection selection;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        switch (state)
        {
        	//四种状态
            case SelectionState.Normal:
                selection = Selection.Normal;
                break;
            case SelectionState.Highlighted:
                selection = Selection.Highlighted;
                break;
            case SelectionState.Pressed:
                selection = Selection.Pressed;
                break;
            case SelectionState.Disabled:
                selection = Selection.Disabled;
                break;
            default:
                break;
        }
    }

    private void OnGUI()
    {
        GUI.skin.box.fontSize = 10;
        switch (selection)
        {
            case Selection.Highlighted:
                GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 25), "Cooperation");
                break;
            case Selection.Pressed:
                GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 25), "Pressed");
                break;
            default:
                break;
        }
    }
}
