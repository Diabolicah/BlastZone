using Fusion;
using UnityEngine;

public static class InputHandler
{
    public static void CollectInput(NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.up;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.down;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        input.Set(data);
    }
}
