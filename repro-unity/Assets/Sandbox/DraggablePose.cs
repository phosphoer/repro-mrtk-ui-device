using Microsoft.MixedReality.Toolkit.Core.Definitions.Devices;
using Microsoft.MixedReality.Toolkit.Core.Definitions.InputSystem;
using Microsoft.MixedReality.Toolkit.Core.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;
using Microsoft.MixedReality.Toolkit.Core.Services;
using Microsoft.MixedReality.Toolkit.SDK.Input.Handlers;
using UnityEngine;

public class DraggablePose : MonoBehaviour, IMixedRealitySourcePoseHandler, IMixedRealityInputHandler
{
    [SerializeField]
    private MixedRealityInputAction inputAction;

    private bool isDragging;
    private bool firstUpdate;
    private Vector3 lastInputPos;
    private Vector3 dragTargetPos;
    private IMixedRealityInputSource dragInputSource;
    private uint dragInputId;


    void IMixedRealitySourceStateHandler.OnSourceDetected(SourceStateEventData eventData)
    {
    }

    void IMixedRealitySourceStateHandler.OnSourceLost(SourceStateEventData eventData)
    {
    }

    void IMixedRealitySourcePoseHandler.OnSourcePoseChanged(SourcePoseEventData<TrackingState> eventData)
    {
    }

    void IMixedRealitySourcePoseHandler.OnSourcePoseChanged(SourcePoseEventData<Vector2> eventData)
    {
    }

    void IMixedRealitySourcePoseHandler.OnSourcePoseChanged(SourcePoseEventData<Vector3> eventData)
    {
        if (eventData.InputSource == dragInputSource && eventData.SourceId == dragInputId)
        {
            if (isDragging)
            {
                if (firstUpdate)
                {
                    firstUpdate = false;
                    lastInputPos = eventData.SourceData;
                }

                dragTargetPos += eventData.SourceData - lastInputPos;
                lastInputPos = eventData.SourceData;
            }
        }
    }

    void IMixedRealitySourcePoseHandler.OnSourcePoseChanged(SourcePoseEventData<Quaternion> eventData)
    {
    }

    void IMixedRealitySourcePoseHandler.OnSourcePoseChanged(SourcePoseEventData<MixedRealityPose> eventData)
    {
    }

    void IMixedRealityInputHandler.OnInputUp(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == inputAction)
        {
            if (isDragging)
            {
                isDragging = false;
                MixedRealityToolkit.InputSystem.PopModalInputHandler();
            }
        }
    }

    void IMixedRealityInputHandler.OnInputDown(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == inputAction)
        {
            if (!isDragging)
            {
                isDragging = true;
                firstUpdate = true;
                dragTargetPos = transform.position;
                MixedRealityToolkit.InputSystem.PushModalInputHandler(gameObject);

                dragInputSource = eventData.InputSource;
                dragInputId = eventData.SourceId;
            }
        }
    }

    void IMixedRealityInputHandler.OnInputPressed(InputEventData<float> eventData)
    {
    }

    void IMixedRealityInputHandler.OnPositionInputChanged(InputEventData<Vector2> eventData)
    {
    }

    private void Awake()
    {
        dragTargetPos = transform.position;
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = Mathfx.Damp(transform.position, dragTargetPos, 0.5f, Time.deltaTime * 5.0f);
        }
    }
}