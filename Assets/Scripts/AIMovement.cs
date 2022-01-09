using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIMovement : MonoBehaviour
{

    public Transform MovePoint;
    public float movementSpeed = 5f;

    public Animator animator;

    public Direction MoveDirection;
    public Direction checkDirection;

    public ParticleSystem BloodParticles;
    public ParticleSystem SlimeParticles;

    public Material SlimeMaterial;

    public int Value = 5;

    public LayerMask StopMovement;
    public LayerMask otherTilemaps;
    public LayerMask movePoints;
    public LayerMask collectibles;

    bool isQuitting = false;
    // Start is called before the first frame update
    void Start()
    {
        MovePoint.parent = null;
        isQuitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, movementSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, MovePoint.position) == 0f)
        {
            searchForPath(MoveDirection);
        }
    }
    void searchForPath(Direction direction)
    {
        Vector3[] checkPositions = new Vector3[4];
        switch (direction)
        {
            case Direction.Left:
                checkPositions[0] = MovePoint.position + new Vector3(-1f, 0f, 0f);
                checkPositions[1] = MovePoint.position + new Vector3(0f, -1f, 0f);                
                checkPositions[2] = MovePoint.position + new Vector3(0f, 1f, 0f);
                checkPositions[3] = MovePoint.position + new Vector3(1f, 0f, 0f);
                switch (checkDirection)
                {
                    case Direction.Left:
                        for (int i = 1, end = 0; i < 3 && end == 0; i++)
                        {
                            var Tile = CheckTile(checkPositions[i]);
                            if(i == 1 && CheckTile(checkPositions[0]) == null)
                            {
                                if(Random.Range(0,100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                MoveDirection = Direction.Down;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                MoveDirection = Direction.Up;
                                                break;
                                            case 3:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else { 
                                    end = 1;
                                    animator.SetFloat("Horizontal", -1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MoveDirection = Direction.Left;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            else if (Tile == null)
                            {
                                end = 1;
                                switch (i)
                                {                                                                                                           
                                    case 1:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        MoveDirection = Direction.Down;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        MoveDirection = Direction.Up;
                                        break;                                    
                                }                                
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 2)
                            {
                                Tile = CheckTile(checkPositions[3]);                               
                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Right;
                                    animator.SetFloat("Horizontal", 1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MovePoint.position = checkPositions[3];                                   
                                }
                            }
                            
                            
                        }
                        break;
                    case Direction.Right:
                        for (int i = 2, end = 0; i >= 1 && end == 0; i--)
                        {
                            var Tile = CheckTile(checkPositions[i]);
                            if (i == 2 && CheckTile(checkPositions[0]) == null)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                MoveDirection = Direction.Down;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                MoveDirection = Direction.Up;
                                                break;
                                            case 3:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else
                                {
                                    end = 1;
                                    animator.SetFloat("Horizontal", -1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MoveDirection = Direction.Left;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            else if (Tile == null)
                            {
                                end = 1;
                                switch (i)
                                {                                    
                                    case 1:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        MoveDirection = Direction.Down;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        MoveDirection = Direction.Up;
                                        break;
                                }
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 1)
                            {
                                Tile = CheckTile(checkPositions[3]);
                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Right;
                                    animator.SetFloat("Horizontal", 1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MovePoint.position = checkPositions[3];
                                }
                            }

                        }
                        break;
                    case Direction.Random:
                        int index = randomDriection();
                        if (index >= 0 && index <= 4)
                        {
                            var Tile = CheckTile(checkPositions[index]);
                            if(Tile == null)
                            {
                                switch (index)
                                {
                                    case 0:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;
                                    case 1:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        MoveDirection = Direction.Down;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        MoveDirection = Direction.Up;
                                        break;                                    
                                    case 3:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;
                                }
                                MovePoint.position = checkPositions[index];
                            }
                        }
                        break;
                }                                
                break;
            case Direction.Right:
                checkPositions[0] = MovePoint.position + new Vector3(1f, 0f, 0f);
                checkPositions[1] = MovePoint.position + new Vector3(0f, 1f, 0f);
                checkPositions[2] = MovePoint.position + new Vector3(0f, -1f, 0f);
                checkPositions[3] = MovePoint.position + new Vector3(-1f, 0f, 0f);
                switch (checkDirection)
                {
                    case Direction.Left:
                        for (int i = 1, end = 0; i < 3 && end == 0; i++)
                        {
                            var Tile = CheckTile(checkPositions[i]);
                            if (i == 1 && CheckTile(checkPositions[0]) == null)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                MoveDirection = Direction.Up;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                MoveDirection = Direction.Down;
                                                break;
                                            case 3:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;

                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else
                                {
                                    end = 1;
                                    animator.SetFloat("Horizontal", 1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MoveDirection = Direction.Right;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            else if (Tile == null)
                            {
                                end = 1;
                                switch (i)
                                {
                                    case 1:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        MoveDirection = Direction.Up;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        MoveDirection = Direction.Down;
                                        break;
                                }
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 2)
                            {
                                Tile = CheckTile(checkPositions[3]);
                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Left;
                                    animator.SetFloat("Horizontal", -1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MovePoint.position = checkPositions[3];
                                }
                            }

                        }
                        break;
                    case Direction.Right:
                        for (int i = 2, end = 0; i >= 1 && end == 0; i--)
                        {
                            var Tile = CheckTile(checkPositions[i]);
                            if (i == 2 && CheckTile(checkPositions[0]) == null)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                MoveDirection = Direction.Up;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                MoveDirection = Direction.Down;
                                                break;
                                            case 3:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;

                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else
                                {
                                    end = 1;
                                    animator.SetFloat("Horizontal", 1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MoveDirection = Direction.Right;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            else if (Tile == null)
                            {
                                end = 1;
                                switch (i)
                                {
                                    case 1:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        MoveDirection = Direction.Up;
                                        break;                                    
                                    case 2:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        MoveDirection = Direction.Down;
                                        break;
                                }
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 1)
                            {
                                Tile = CheckTile( checkPositions[3]);
                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Left;
                                    animator.SetFloat("Horizontal", -1f);
                                    animator.SetFloat("Vertical", 0f);
                                    MovePoint.position = checkPositions[3];
                                }
                            }

                        }
                        break;
                    case Direction.Random:
                        int index = randomDriection();

                        {
                            var Tile = CheckTile(checkPositions[index]);
                            if (Tile == null)
                            {
                                switch (index)
                                {
                                    case 0:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;                                    
                                    case 1:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        MoveDirection = Direction.Up;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        MoveDirection = Direction.Down;
                                        break;
                                    case 3:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;

                                }
                                MovePoint.position = checkPositions[index];
                            }
                        }
                        break;
                }
                break;
            case Direction.Up:
                checkPositions[0] = MovePoint.position + new Vector3(0f, 1f, 0f);
                checkPositions[1] = MovePoint.position + new Vector3(-1f, 0f, 0f);
                checkPositions[2] = MovePoint.position + new Vector3(1f, 0f, 0f);
                checkPositions[3] = MovePoint.position + new Vector3(0f, -1f, 0f);
                switch (checkDirection)
                {
                    case Direction.Left:
                        for (int i = 1, end = 0; i < 3 && end == 0; i++)
                        {
                            var Tile = CheckTile(checkPositions[i]);
                            if (i == 1 && CheckTile(checkPositions[0]) == null)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                MoveDirection = Direction.Up;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                            case 3:
                                                MoveDirection = Direction.Down;
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                break;

                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else
                                {

                                    end = 1;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", 1f);
                                    MoveDirection = Direction.Up;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            else if (Tile == null)
                            {
                                end = 1;
                                switch (i)
                                {
                                    case 1:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;
                                }
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 2)
                            {
                                Tile = CheckTile(checkPositions[3]);
                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Down;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", -1f);
                                    MovePoint.position = checkPositions[3];
                                }
                            }

                        }
                        break;
                    case Direction.Right:
                        for (int i = 2, end = 0; i >= 1 && end == 0; i--)
                        {
                            var Tile = CheckTile( checkPositions[i]);
                            if (i == 2 && CheckTile(checkPositions[0]) == null)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                MoveDirection = Direction.Up;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                            case 3:
                                                MoveDirection = Direction.Down;
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                break;

                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else
                                {
                                    end = 1;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", 1f);
                                    MoveDirection = Direction.Up;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            else if (Tile == null)
                            {

                                end = 1;
                                switch (i)
                                {
                                    case 1:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;                                    
                                    case 2:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;
                                }
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 1)
                            {
                                Tile = CheckTile(checkPositions[3]);

                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Down;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", -1f);
                                    MovePoint.position = checkPositions[3];
                                }
                            }

                        }
                        break;
                    case Direction.Random:
                        int index = randomDriection();
                        if (index >= 0 && index <= 4)
                        {
                            var Tile = CheckTile(checkPositions[index]);
                            if (Tile == null)
                            {
                                switch (index)
                                {
                                    case 0:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        MoveDirection = Direction.Up;
                                        break;
                                    case 1:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;
                                    case 3:
                                        MoveDirection = Direction.Down;
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        break;

                                }
                                MovePoint.position = checkPositions[index];
                            }
                        }
                        break;

                }
                break;
            case Direction.Down:
                checkPositions[0] = MovePoint.position + new Vector3(0f, -1f, 0f);
                checkPositions[1] = MovePoint.position + new Vector3(1f, 0f, 0f);
                checkPositions[2] = MovePoint.position + new Vector3(-1f, 0f, 0f);
                checkPositions[3] = MovePoint.position + new Vector3(0f, 1f, 0f);
                switch (checkDirection)
                {
                    case Direction.Left:
                        for (int i = 1, end = 0; i < 3 && end == 0; i++)
                        {
                            var Tile = CheckTile(checkPositions[i]);
                            if (i == 1 && CheckTile(checkPositions[0]) == null)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                MoveDirection = Direction.Down;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;
                                            case 3:
                                                MoveDirection = Direction.Up;
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                break;

                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else
                                {
                                    end = 1;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", -1f);
                                    MoveDirection = Direction.Down;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            else if (Tile == null)
                            {
                                end = 1;
                                switch (i)
                                {
                                    case 1:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;
                                }
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 2)
                            {
                                Tile = CheckTile(checkPositions[3]);
                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Up;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", 1f);
                                    MovePoint.position = checkPositions[3];
                                }
                            }

                        }
                        break;
                    case Direction.Right:
                        for (int i = 2, end = 0; i >= 1 && end == 0; i--)
                        {
                            var Tile = CheckTile(checkPositions[i]);
                            if (i == 2 && CheckTile(checkPositions[0]) == null)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    i = randomDriection();
                                    Tile = CheckTile(checkPositions[i]);
                                    if (Tile == null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", -1f);
                                                MoveDirection = Direction.Down;
                                                break;
                                            case 1:
                                                animator.SetFloat("Horizontal", 1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Right;
                                                break;
                                            case 2:
                                                animator.SetFloat("Horizontal", -1f);
                                                animator.SetFloat("Vertical", 0f);
                                                MoveDirection = Direction.Left;
                                                break;
                                            case 3:
                                                MoveDirection = Direction.Up;
                                                animator.SetFloat("Horizontal", 0f);
                                                animator.SetFloat("Vertical", 1f);
                                                break;

                                        }
                                        MovePoint.position = checkPositions[i];
                                    }
                                }
                                else
                                {
                                    end = 1;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", -1f);
                                    MoveDirection = Direction.Down;
                                    MovePoint.position = checkPositions[0];
                                }
                            }
                            
                            else if (Tile == null)
                            {
                                end = 1;
                                switch (i)
                                {
                                    case 1:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;
                                }
                                MovePoint.position = checkPositions[i];
                            }
                            else if (i == 1)
                            {
                                Tile = CheckTile(checkPositions[3]);
                                if (Tile == null)
                                {
                                    MoveDirection = Direction.Up;
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", 1f);
                                    MovePoint.position = checkPositions[3];
                                }
                            }


                        }
                        break;
                    case Direction.Random:
                        
                        int index = randomDriection();                       
                        if (index >= 0 && index <= 4)
                        {
                            var Tile = CheckTile(checkPositions[index]);
                            if (Tile == null)
                            {
                                switch (index)
                                {
                                    case 0:
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", -1f);
                                        MoveDirection = Direction.Down;
                                        break;
                                    case 1:
                                        animator.SetFloat("Horizontal", 1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Right;
                                        break;
                                    case 2:
                                        animator.SetFloat("Horizontal", -1f);
                                        animator.SetFloat("Vertical", 0f);
                                        MoveDirection = Direction.Left;
                                        break;
                                    case 3:
                                        MoveDirection = Direction.Up;
                                        animator.SetFloat("Horizontal", 0f);
                                        animator.SetFloat("Vertical", 1f);
                                        break;

                                }
                                MovePoint.position = checkPositions[index];
                            }
                        }
                        break;
                }
                break;
        }
    }

    Collider2D CheckTile(Vector3 pos)
    {
        //check if there is obstacle
        if (!Physics2D.OverlapCircle(pos, .2f, StopMovement))
        {
            //Check if there is any move point (prevent movement collision)
            if (!Physics2D.OverlapCircle(pos, .2f, movePoints))
            {
                //check if there is other tilemap obstacle
                if (Physics2D.OverlapCircle(pos, .2f, otherTilemaps))
                {
                    var tempObj = Physics2D.OverlapCircle(pos, .2f, otherTilemaps);
                    //check if its a stone
                    if (tempObj.tag == "Dirt")
                    {
                        return tempObj;
                    }
                    else if (tempObj.tag == "Player")
                    {
                        return null;
                    }
                    else if (tempObj.tag == "Stone")
                    {
                        return tempObj;
                    }
                    //check if its a door
                    else if (tempObj.tag == "Door")
                    {
                        var Door = tempObj.GetComponent<DoorController>();
                        if (Door.isOpen)
                        {
                            return null;
                        }
                        else
                        {
                            return tempObj;
                        }
                    }
                    //check if its Exit
                    else if (tempObj.tag == "Exit")
                    {
                        return tempObj;
                    }
                    //check if its teleport
                    else if (tempObj.tag == "Teleport")
                    {
                        return null;
                    }
                    else if (tempObj.tag == "Interactions")
                    {
                        string tileName = tempObj.GetComponent<Tilemap>().GetTile(tempObj.GetComponent<Tilemap>().WorldToCell(pos)).name;
                        if (tileName.Contains("Dirt"))
                        {
                            return tempObj;
                        }
                        else if (tileName.Contains("Lava"))
                        {
                            return tempObj;
                        }
                        else
                        {
                            return tempObj;
                        }
                    }
                    else
                    {
                        return tempObj;
                    }
                }
                else if(Physics2D.OverlapCircle(pos, .2f, collectibles))
                {
                    var tempObj = Physics2D.OverlapCircle(pos, .2f, collectibles);
                    return tempObj;
                }
                else
                {
                    return null;
                }
            }
            else if (Physics2D.OverlapCircle(pos, .2f, movePoints).tag == "Enemie" || Physics2D.OverlapCircle(pos, .2f, movePoints).tag == "Player")
            {
                return null;
            }
            else
            {
                return Physics2D.OverlapCircle(pos, .2f, movePoints);
            }
        }
        else 
        { 
            return Physics2D.OverlapCircle(pos, .2f, StopMovement);
        }
    }
    
    int randomDriection()
    {
        int range = Random.Range(0, 12);
        int index;
        if (range > 10)
            index = 3;
        else if (range > 8)
            index = 2;
        else if (range > 6)
            index = 1;
        else
            index = 0;

        return index;
    }

    public enum Direction
    {
        Left, Right, Up, Down, Random
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {            
            Destroy(collision.gameObject);
        }
    }
    private void OnDestroy()
    {
        if (GameObject.Find("Player"))
        {
            GameObject.Find("Player").GetComponent<PlayerInv>().numOfPoints += Value;
            GameObject.Find("Player").GetComponent<PlayerInv>().Player_GUI.updateGUI();
        }
        if (!isQuitting && GameObject.Find("GameManager").GetComponent<GameManager>().ReloadScene == false)
        {
            if (BloodParticles)
            {
                var tempObj = Instantiate(BloodParticles, transform.position, Quaternion.identity);
                Destroy(tempObj, 60f);
            }
            else if (SlimeParticles)
            {
                var tempObj = Instantiate(SlimeParticles, transform.position, Quaternion.identity);
                tempObj.GetComponent<ParticleSystemRenderer>().material = SlimeMaterial;
                Destroy(tempObj, 60f);
            }
        }
        if (MovePoint)
            Destroy(MovePoint.gameObject);
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }   
}
