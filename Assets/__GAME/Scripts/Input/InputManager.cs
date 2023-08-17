using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyProject.Core.Manager;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] Canvas canvas;
    public bool isDragging = false;
    public bool IsTouchActiveted = false;
    public Vector3 ScreenPointerPosition => Input.mousePosition;
    public Vector3 CanvasPointerPosition
    {
        get
        {
            return GetCanvasPointerPosition();
        }
    }

    public UnityEvent e_OnPointerDown { get; set; }
    public UnityEvent e_OnDrag { get; set; }
    public UnityEvent e_OnPointerUp { get; set; }
   
   
    public Vector3 _initialMousePos;
   
    public RenderMode CanvasRenderMode
    {
        get
        {
            return canvas.renderMode;
        }
    }

    void Start()
    {
        e_OnPointerDown = new UnityEvent();
        e_OnDrag = new UnityEvent();
        e_OnPointerUp = new UnityEvent();
      
    }

    public void OnUpdate()
    {
        if (!GameManager.I.isRunning) return;
        if (GameManager.I.isFiveLevelCondition) return;
        //if (!IsTouchActiveted) return;


        if (!isDragging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnPointerDown();
            }
        }
        else
        {
            OnDrag();

            if (Input.GetMouseButtonUp(0))
            {
                OnPointerUp();
            }
        }

        
    }

    public void OnPointerDown()
    {
        isDragging = true;
        _initialMousePos = ScreenPointerPosition;
        e_OnPointerDown?.Invoke();
    }

    public void OnDrag()
    {
        if (_initialMousePos != ScreenPointerPosition)
        {
            e_OnDrag?.Invoke();
        }
    }

    public void OnPointerUp()
    {
        isDragging = false;
        e_OnPointerUp?.Invoke();
    }

    Vector3 GetCanvasPointerPosition()
    {
        Camera mainCamera = Camera.main;
        if (CanvasRenderMode == RenderMode.ScreenSpaceOverlay)
        {
            return ScreenPointerPosition;
        }
        else if (CanvasRenderMode == RenderMode.ScreenSpaceCamera)
        {
            var screenPoint = ScreenPointerPosition;
            screenPoint.z = transform.position.z - mainCamera.transform.position.z; //distance of the plane from the camera
            return mainCamera.ScreenToWorldPoint(screenPoint);
        }
        else if (CanvasRenderMode == RenderMode.WorldSpace)
        {
            var screenPoint = ScreenPointerPosition;
            screenPoint.z = transform.position.z - mainCamera.transform.position.z; //distance of the plane from the camera
            return mainCamera.ScreenToWorldPoint(screenPoint);
        }

        return Vector3.zero;
    }
}

