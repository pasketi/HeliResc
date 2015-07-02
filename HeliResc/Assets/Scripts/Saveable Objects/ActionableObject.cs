using UnityEngine;
using System.Collections;

public abstract class ActionableObject : MonoBehaviour, IActionable {

    public abstract void UseAction();
}
public interface IActionable
{
    void UseAction();
}
