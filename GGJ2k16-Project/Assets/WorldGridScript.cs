using UnityEngine;
using System.Collections;

public class WorldGridScript : MonoBehaviour
{
    /// <summary>
    /// Width & Height of cell
    /// </summary>
    public Vector2 m_sizeOfCell;


    public Vector2 m_numberOfCells;

    //used for debugging, collision /w points.
    //private Rect[,] m_cells;
    void Awake()
    {
        //m_cells = new Rect[(int)m_numberOfCells.x, (int)m_numberOfCells.y];
        //Vector2 pos = transform.position;
        //for (int i = 0; i < m_numberOfCells.x; ++i)
        //{
        //    for (int j = 0; j < m_numberOfCells.y; ++j)
        //    {
        //        m_cells[i, j].size = m_sizeOfCell;
        //        m_cells[i, j].position = pos +  Maths.ComponentMultiply(new Vector2(i, j), m_sizeOfCell);
        //    }
        //}
    }
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public bool GetCellRectFromPoint(Vector2 point, out Rect rect)
    {
        rect = new Rect();
        Vector2 gridPos = transform.position;
        for (int i = 0; i < m_numberOfCells.x; ++i)
        {
            for (int j = 0; j < m_numberOfCells.y; ++j)
            {
                rect.position = gridPos + Maths.ComponentMultiply(new Vector2(i, j), m_sizeOfCell);
                rect.size = m_sizeOfCell;
                if (rect.Contains(point))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Vector2 gridPos = transform.position;
        for (int i = 0; i < m_numberOfCells.x; ++i)
        {
            for (int j = 0; j < m_numberOfCells.y; ++j)
            {
                Vector2 cellPos = gridPos + Maths.ComponentMultiply(new Vector2(i, j), m_sizeOfCell) + (m_sizeOfCell/2);
                Gizmos.DrawWireCube(cellPos, m_sizeOfCell);
            }
        }
       
    }
}
