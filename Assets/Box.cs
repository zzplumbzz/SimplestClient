using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    public Board boardScript;
    public GameSystemManger GSMScript;
    public int index;
    public Mark mark;
    public bool isMarked;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        index = transform.GetSiblingIndex();
        mark = Mark.none;
        isMarked = false;
    }

    public void SetAsMarked(Sprite sprite, Mark mark, Color color)
    {
        isMarked = true;
        this.mark = mark;

        spriteRenderer.color = color;
        spriteRenderer.sprite = sprite;

        GetComponent<CircleCollider2D>().enabled = false;
    }

    // public void SetAsUnMarked(Sprite sprite, Mark mark, Color color)
    // {
    //     isMarked = false;
    //     this.mark = Mark.none;

    //     // spriteRenderer.color = null;
    //     spriteRenderer.sprite = null;

    //     GetComponent<CircleCollider2D>().enabled = true;
    // }

    

    
}
