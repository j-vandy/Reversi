using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Screen : MonoBehaviour
{
    private bool _bIsEnabled = true;
    public bool bIsEnabled
    {
        get
        {
            return _bIsEnabled;
        }
        set
        {
            if (value)
                Enable();
            else
                Disable();
            _bIsEnabled = value;
        }
    }

    public virtual void Enable()
    {
        foreach(Transform child in transform)
            child.gameObject.SetActive(true);
    }

    public virtual void Disable()
    {
        foreach(Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public virtual void ScreenTransition(Screen screenToEnable)
    {
        screenToEnable.Enable();
        Disable();
    }
}
