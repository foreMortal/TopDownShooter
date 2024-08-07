using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f, speedModifier = 1;
    private List<float> modifiers = new List<float>();

    private void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            moveDirection.y += 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDirection.x += -1;
        if (Input.GetKey(KeyCode.DownArrow))
            moveDirection.y += -1;
        if (Input.GetKey(KeyCode.RightArrow))
            moveDirection.x += 1;

        transform.position = Vector2.MoveTowards(transform.position, transform.position + moveDirection * (speed * speedModifier), speed * speedModifier * Time.deltaTime);
    }

    public void ChangeModifier(int type, float newMod)
    {
        //список множителей влияющих на скорость нужен для избежания проблем при накладывании друг на друга
        if (type == 0)
            modifiers.Add(newMod);
        else
            modifiers.Remove(newMod);

        newMod = 1;
        foreach (var mod in modifiers)
        {
            newMod *= mod;
        }
        speedModifier = newMod;
    }
}
