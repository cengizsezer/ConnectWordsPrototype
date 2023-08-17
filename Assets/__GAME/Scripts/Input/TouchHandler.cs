using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchHandler : Singleton<TouchHandler>
{
    TouchBase activeTouch;

    bool isDragging = false;
    bool canPlay = false;
    public void SetPlayable(bool isPlayable) => canPlay = isPlayable;

    [SerializeField] GraphicRaycaster graphicRaycaster;

    private void Start()
    {
        //SetTouch(new ExampleTouch());
        SetPlayable(true);
    }

    public T GetUIElement<T>()
    {
        return graphicRaycaster.GetUIElement<T>();
    }

    private void Update()
    {
        if(canPlay)
        {
            if (activeTouch != null)
                HandleTouch();
        }
    }
    private void HandleTouch()
    {
        if(!isDragging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                isDragging = true;
                activeTouch.OnDown();
            }
        }
        else
        {
            activeTouch.OnDrag();

            if(Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                activeTouch.OnUp();
            }
        }
        
    }

    public void SetTouch(TouchBase touchBase)
    {
        if (activeTouch != null)
            activeTouch.OnDeinitialized();

        activeTouch = touchBase;

        if (activeTouch != null)
            activeTouch.OnInitialized();
    }

}
