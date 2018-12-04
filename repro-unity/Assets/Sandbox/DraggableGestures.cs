using Microsoft.MixedReality.Toolkit.Core.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;
using Microsoft.MixedReality.Toolkit.Core.Services;
using Microsoft.MixedReality.Toolkit.SDK.Input.Handlers;
using UnityEngine;

public class DraggableGestures : MonoBehaviour, IMixedRealityGestureHandler<Vector3>
{
    private bool isDragging;
    private Vector3 lastPointerPos;
    private Vector3 dragTargetPos;

    void IMixedRealityGestureHandler.OnGestureCanceled(InputEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            MixedRealityToolkit.InputSystem.PopModalInputHandler();
            dragTargetPos = transform.position;
        }
    }

    void IMixedRealityGestureHandler<Vector3>.OnGestureCompleted(InputEventData<Vector3> eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            MixedRealityToolkit.InputSystem.PopModalInputHandler();
            dragTargetPos = transform.position;
        }
    }

    void IMixedRealityGestureHandler.OnGestureCompleted(InputEventData eventData)
    {
    }

    void IMixedRealityGestureHandler.OnGestureStarted(InputEventData eventData)
    {
        if (!isDragging)
        {
            dragTargetPos = transform.position;
            MixedRealityToolkit.InputSystem.PushModalInputHandler(gameObject);
        }
    }

    void IMixedRealityGestureHandler<Vector3>.OnGestureUpdated(InputEventData<Vector3> eventData)
    {
        if (!isDragging)
        {
            lastPointerPos = eventData.InputData;
            isDragging = true;
        }

        Vector3 delta = eventData.InputData - lastPointerPos;
        dragTargetPos += delta * 2;
        lastPointerPos = eventData.InputData;
    }

    void IMixedRealityGestureHandler.OnGestureUpdated(InputEventData eventData)
    {
    }

    private void Awake()
    {
        dragTargetPos = transform.position;
    }

    private void Update()
    {
        transform.position = Mathfx.Damp(transform.position, dragTargetPos, 0.5f, Time.deltaTime * 5.0f);
    }
}