using UnityEngine;

public interface ILockOnTarget
{
    Transform GetTransform();
    event System.Action Ondeath;
    bool IsBoss { get; }
}