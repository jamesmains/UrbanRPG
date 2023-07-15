using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tree : MonoBehaviour
{
    public int health;
    public bool felled;
    public Sprite stump;
    public SpriteRenderer gfx;
    public UnityEvent onChop;
    public UnityEvent onFell;
    public void Chop()
    {
        if (felled) return;
        health--;
        if(health<=0)
            Fell();
        else onChop.Invoke();
    }

    private void Fell()
    {
        felled = true;
        gfx.sprite = stump;
        onFell.Invoke();
        print("TIMBER!");
    }
}
