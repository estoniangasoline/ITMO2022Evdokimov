using Cinemachine;
using Ubavar.core.Utils;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private void ActivateCamera(string name)
    {
        GameObjectUtils.FindDescendentTransformByPathOnRootTransform("GameVirtualCamera")
            .GetComponent<CinemachineVirtualCamera>().Priority = int.MaxValue;
        GameObjectUtils.FindDescendentTransformByPathOnRootTransform("ObserverVirtualCamera")
            .GetComponent<CinemachineVirtualCamera>().Priority = int.MaxValue;

        GameObjectUtils.FindDescendentTransformByPathOnRootTransform(name)
            .GetComponent<CinemachineVirtualCamera>().Priority = int.MinValue;
    }

    public void ActivateGameCamera()
    {
        ActivateCamera("GameVirtualCamera");
    }

    public void ActivateObserverCamera()
    {
        ActivateCamera("ObserverVirtualCamera");
    }
}