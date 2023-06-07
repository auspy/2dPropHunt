using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunctions : MonoBehaviour
{
    void ToggleEnabled(Component component, bool isEnabled)
    {
        if (component != null)
        {
            if (component is Renderer)
                ((Renderer)component).enabled = isEnabled;
            else if (component is Behaviour)
                ((Behaviour)component).enabled = isEnabled;
            else if (component is Collider2D)
                ((Collider2D)component).enabled = isEnabled;
            else
                print("non toggleable component");
        }
    }
}
