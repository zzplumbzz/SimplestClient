using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Board : MonoBehaviour
{
GameObject networkedClient;
    public NetworkedClient ncs;
    LinkedList<MovesDone> movesDone;

    const int AllMovesDone = 9;
    string movesDoneFilePath;
     GameObject gameSystemManger;
     public Box box;

    [Header("Input Settings: ")]
    [SerializeField] private LayerMask boxesLayerMask;
    [SerializeField] private float touchRadius;

    [Header("Mark Sprites: ")]
    [SerializeField] private Sprite spriteX;
    [SerializeField] private Sprite spriteO;

    [Header("Mark Sprites: ")]
    [SerializeField] private Color colorX;
    [SerializeField] private Color colorO;

    public Mark[] marks;

    private Camera cam;
    public Mark currentMark;

    private void Start()// on start try to save moves and start the game with x turn first
    {
        movesDoneFilePath = Application.dataPath + Path.DirectorySeparatorChar + "MovesDone.txt";
        cam = Camera.main;

        currentMark = Mark.X;

        marks = new Mark[9];
    }

    private void Update() // update when a box has been clicked
    {
        if(Input.GetMouseButtonUp (0))
        {
            
            Vector2 touchPosition = cam.ScreenToWorldPoint (Input.mousePosition);

            Collider2D hit = Physics2D.OverlapCircle(touchPosition, touchRadius, boxesLayerMask);

            if(hit)
            HitBox(hit.GetComponent<Box>());
            
            
        }

        
    }

    private void HitBox(Box box)// mark box and give it sprite and color and check if win
    {
        if(!box.isMarked)
        {
            marks[box.index] = currentMark;

            box.SetAsMarked(GetSprite(), currentMark, GetColor());
            bool won = CheckIfWin();
            if(won)
            {
                
            
                Debug.Log(currentMark.ToString() + "Wins");
                
                return;
                
            }

            SwitchPlayer();
        }
    }

   

     public bool CheckIfWin()// all ways to win
    {
        return
        AreBoxesMatched(0, 1, 2) || AreBoxesMatched(3, 4, 5) || AreBoxesMatched(6, 7, 8) ||
        AreBoxesMatched(0, 3, 6) || AreBoxesMatched(1, 4, 7) || AreBoxesMatched(2, 5, 8) ||
        AreBoxesMatched(0, 4, 8) || AreBoxesMatched(2, 4, 6);
    }

    private bool AreBoxesMatched(int i, int j, int k)// check boxes for match
    {
        Mark m = currentMark;
        bool matched = (marks[i] == m && marks[j] == m && marks[k] == m);

        

        

        return matched; 
    }

    public void SwitchPlayer()
    {
        currentMark = (currentMark == Mark.X) ? Mark.O : Mark.X;
       
    }

    private Color GetColor()
    {
        return (currentMark == Mark.X) ? colorX : colorO;
    }

    private Sprite GetSprite()
    {
        return (currentMark == Mark.X) ? spriteX : spriteO;
    }

    public class MovesDone
    {
        public int moves;
        public int marks;

        public MovesDone(int Moves)
        {
            marks = Moves;

        }
    }

    public void SaveMovesDone()
    {

        
        StreamWriter sw = new StreamWriter(movesDoneFilePath);

        foreach(MovesDone mo in movesDone)
            {
                sw.WriteLine(AllMovesDone + "," + mo.moves);
            }
            sw.Close();
    }

   
}
