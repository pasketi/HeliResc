using UnityEngine;
using System.Collections;

public class ActionableObject : MonoBehaviour, IActionable {
	
    public virtual void UseAction() { 
    
    }
}
public interface IActionable
{
    void UseAction();
}
