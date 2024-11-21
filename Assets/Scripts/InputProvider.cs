using UnityEngine;

public class InputProvider : IInputProvider
{
    public float GetHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }
}
