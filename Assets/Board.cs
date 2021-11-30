using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Board : MonoBehaviour
{

    public NetworkedClient ncs;
    LinkedList<MovesDone> movesDone;

    const int AllMovesDone = 9;
    string movesDoneFilePath;
     public GameSystemManger GSMScript;
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

    private void Start()
    {
        movesDoneFilePath = Application.dataPath + Path.DirectorySeparatorChar + "MovesDone.txt";
        cam = Camera.main;

        currentMark = Mark.X;

        marks = new Mark[9];
    }

    private void Update() 
    {
        if(Input.GetMouseButtonUp (0))
        {
            
            Vector2 touchPosition = cam.ScreenToWorldPoint (Input.mousePosition);

            Collider2D hit = Physics2D.OverlapCircle(touchPosition, touchRadius, boxesLayerMask);

            if(hit)
            HitBox(hit.GetComponent<Box>());
            
            
        }

        
    }

    private void HitBox(Box box)
    {
        if(!box.isMarked)
        {
            marks[box.index] = currentMark;

            box.SetAsMarked(GetSprite(), currentMark, GetColor());
            //ncs.GetComponent<NetworkedClient>().SendMessageToHost(MovesDone);
            bool won = CheckIfWin();
            if(won)
            {
                
            
                Debug.Log(currentMark.ToString() + "Wins");
                
                return;
                
            }

            SwitchPlayer();
        }
    }

   

     public bool CheckIfWin()
    {
        return
        AreBoxesMatched(0, 1, 2) || AreBoxesMatched(3, 4, 5) || AreBoxesMatched(6, 7, 8) ||
        AreBoxesMatched(0, 3, 6) || AreBoxesMatched(1, 4, 7) || AreBoxesMatched(2, 5, 8) ||
        AreBoxesMatched(0, 4, 8) || AreBoxesMatched(2, 4, 6);
    }

    private bool AreBoxesMatched(int i, int j, int k)
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

    // public void LoadMovesDone()
    //     {

    //         if(File.Exists(movesDoneFilePath))
    //         {

            

    //         StreamReader sr = new StreamReader(movesDoneFilePath);

    //         string line;

    //             while(true)
    //             {
    //                 line = sr.ReadLine();
    //                 if(line == null)
    //                 break;
    //                 string[] csv = line.Split(',');

    //                 int signifier = int.Parse(csv[0]);

    //                 if(signifier == AllMovesDone)
    //                 {
    //                     MovesDone mo = new MovesDone();
    //                     movesDone.AddLast(mo);
    //                 }
    //                 /*else if(signifier == )
    //                 {
                        
    //                 }*/
    //             }
    //             sr.Close();
    //         }

    //     }


}
