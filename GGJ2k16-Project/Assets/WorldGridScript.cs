using UnityEngine;
using System.Collections;

public class WorldGridScript : MonoBehaviour
{
    /// <summary>
    /// Width & Height of cell
    /// </summary>
    public Vector2 m_sizeOfCell;

    public Vector2 m_sizeOfPadding;
    public Vector2 m_numberOfCells;
    /// <summary>
    /// When determining the position of players, count paddings as part of cells?
    /// </summary>
    public bool m_paddingIsGridSpace;
    public bool m_usePaddingForCameraSize;

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

    public float ClalculateOrthoCamSize()
    {
        if (m_usePaddingForCameraSize)
        {
            return m_sizeOfCell.x / 2;

        }
        else
        {
            return (m_sizeOfCell.x + m_sizeOfPadding.x) / 2;
        }
    }

	// Update is called once per frame
	void Update ()
    {
	
	}

    public bool GetCellRectFromPoint(Vector2 point, out Rect rect)
    {
        rect = new Rect();
        Vector2 gridPos = transform.position;
        Vector2 idx = new Vector2();
        for (int i = 0; i < m_numberOfCells.x; ++i)
        {
            for (int j = 0; j < m_numberOfCells.y; ++j)
            {
                idx.x = i;
                idx.y = j;
                if (m_paddingIsGridSpace)
                {
                    rect.position = gridPos + Maths.ComponentMultiply(idx, m_sizeOfCell) - m_sizeOfPadding / 2;
                    rect.size = m_sizeOfCell + Maths.ComponentMultiply(idx, m_sizeOfPadding);
                }
                else
                {
                    rect.position = gridPos + Maths.ComponentMultiply(idx, m_sizeOfCell);// Maths.ComponentMultiply(idx, m_sizeOfPadding);
                    rect.size = m_sizeOfCell;
                }

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
        Vector2 idx = new Vector2();
        for (int i = 0; i < m_numberOfCells.x; ++i)
        {
            for (int j = 0; j < m_numberOfCells.y; ++j)
            {
                idx.x = i;
                idx.y = j;
                Vector2 cellPos = gridPos + Maths.ComponentMultiply(idx, m_sizeOfCell) + (m_sizeOfCell/2)  + Maths.ComponentMultiply(idx, m_sizeOfPadding);
                Gizmos.DrawWireCube(cellPos, m_sizeOfCell);
            }
        }
       
    }
}
