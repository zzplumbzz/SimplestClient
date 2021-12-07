using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
GameObject networkedClient;
    public Board boardScript;
    GameObject gameSystemManger;
    public int index;
    public Mark mark;
    public bool isMarked;

    private SpriteRenderer spriteRenderer;

    private void Awake()// all boxes are unmarked on awake/ reset on awake
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        index = transform.GetSiblingIndex();
        mark = Mark.none;
        isMarked = false;
    }

    public void SetAsMarked(Sprite sprite, Mark mark, Color color)// give the mark a sprite and color
    {
        isMarked = true;
        this.mark = mark;

        spriteRenderer.color = color;
        spriteRenderer.sprite = sprite;

        GetComponent<CircleCollider2D>().enabled = false;
    }
}
