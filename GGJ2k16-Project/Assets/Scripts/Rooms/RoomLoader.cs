using UnityEngine;
using System.Collections.Generic;

public class RoomLoader : MonoBehaviour {
    public enum MapObject
    {
        EMPTY,
        WALL
    }

    [System.Serializable]
    public struct ColObjPair
    {
        public Color m_col;
        public MapObject m_obj;
    }

    public GameObject m_crossWall;
    public GameObject m_verticalWall;
    public GameObject m_horizontalWall;
    public GameObject m_soloWall;
    public GameObject m_closedLeftWall;
    public GameObject m_closedRightWall;
    public GameObject m_closedUpWall;
    public GameObject m_closedDownWall;
    public GameObject m_topLeftWall;
    public GameObject m_topRightWall;
    public GameObject m_botLeftWall;
    public GameObject m_botRightWall;
    public GameObject m_horizontalUpCrossWall;
    public GameObject m_horizontalDownCrossWall;
    public GameObject m_verticalRightCrossWall;
    public GameObject m_verticalLeftCrossWall;

    public ColObjPair[] m_keys = new ColObjPair[1];
    public Texture2D m_image;

    public Vector2 m_tilesize = new Vector2(1.2f, 1.2f);
    public Vector2 m_offset;
    // Use this for initialization
    void Start () {
	    if(m_image)
        {
            for (int ix = 0; ix < m_image.width; ix++)
            {
                for (int iy = 0; iy < m_image.height; iy++)
                {
                    Color pix = m_image.GetPixel(ix, iy);
                    foreach (ColObjPair pear in m_keys)
                    {
                        Color col = pear.m_col;
                        if (col == pix)
                        {
                            Dictionary<int, Vector2> ezDirs = new Dictionary<int, Vector2>();
                            ezDirs.Add(1, new Vector2(-1, -1));
                            ezDirs.Add(2, new Vector2(0, -1));
                            ezDirs.Add(3, new Vector2(1, -1));
                            ezDirs.Add(4, new Vector2(-1, 0));
                            ezDirs.Add(5, new Vector2(0, 0));
                            ezDirs.Add(6, new Vector2(1, 0));
                            ezDirs.Add(7, new Vector2(-1, 1));
                            ezDirs.Add(8, new Vector2(0, 1));
                            ezDirs.Add(9, new Vector2(1, 1));
                            switch (pear.m_obj)
                            {
                                case MapObject.WALL:
                                    {
                                        GameObject fate;
                                        bool[] positions = { false, false, false, false, false, true, false, false, false, false };
                                        List<int> exclusions = new List<int>();
                                        exclusions.Add(1);
                                        exclusions.Add(3);
                                        exclusions.Add(5);
                                        exclusions.Add(7);
                                        exclusions.Add(9);
                                        if (ix == 0)
                                        {
                                            //nothing on the left
                                            exclusions.Add(4);
                                        }
                                        if(ix == m_image.width - 1)
                                        {
                                            //nothing on the right
                                            exclusions.Add(6);
                                            
                                        }
                                        if (iy == 0)
                                        {
                                            //nothing below
                                            exclusions.Add(2);
                                        }
                                        if (iy == m_image.height - 1)
                                        {
                                            //nothing above
                                            exclusions.Add(8);
                                        }
                                        
                                        int connCount = 0;
                                        for(int i=1; i<=9; i++)
                                        {
                                            while(exclusions.Contains(i))
                                            {
                                                i++;
                                            }
                                            if(i>9)
                                            {
                                                break;
                                            }
                                            Color adj = m_image.GetPixel(ix + (int)ezDirs[i].x, iy + (int)ezDirs[i].y);
                                            adj.a = col.a;
                                            if(adj == col)
                                            {
                                                positions[i] = true;
                                                connCount++;
                                            }
                                        }
                                        fate = m_soloWall;
                                        if (connCount == 4)
                                        {
                                            //4 prong
                                            fate = m_crossWall;
                                        }
                                        if(connCount == 3)
                                        {
                                            //3 prong
                                            if(positions[8] && positions[2])
                                            {
                                                //vertical cross
                                                if(positions[6])
                                                {
                                                    fate = m_verticalRightCrossWall;
                                                }
                                                else
                                                {
                                                    fate = m_verticalLeftCrossWall;
                                                }
                                            }
                                            if (positions[4] && positions[6])
                                            {
                                                //horizontal cross
                                                if (positions[2])
                                                {
                                                    fate = m_horizontalDownCrossWall;
                                                }
                                                else
                                                {
                                                    fate = m_horizontalUpCrossWall;
                                                }
                                            }
                                        }
                                        if(connCount == 2)
                                        {
                                            //line or corner
                                            if (positions[8] && positions[2])
                                            {
                                                //vertical line
                                                fate = m_verticalWall;
                                            }
                                            if (positions[4] && positions[6])
                                            {
                                                //horizontal line
                                                fate = m_horizontalWall;
                                            }
                                            if(positions[8] && positions[6])
                                            {
                                                //botleft
                                                fate = m_botLeftWall;
                                            }
                                            if (positions[8] && positions[4])
                                            {
                                                //botright
                                                fate = m_botRightWall;
                                            }
                                            if (positions[2] && positions[4])
                                            {
                                                //topright
                                                fate = m_topRightWall;
                                            }
                                            if (positions[2] && positions[6])
                                            {
                                                //topleft
                                                fate = m_topLeftWall;
                                            }
                                        }
                                        if(connCount == 1)
                                        {
                                            //line end
                                            if(positions[2])
                                            {
                                                //closed on top
                                                fate = m_closedUpWall;
                                            }
                                            if(positions[4])
                                            {
                                                //closed on right
                                                fate = m_closedRightWall;
                                            }
                                            if (positions[6])
                                            {
                                                //closed on left
                                                fate = m_closedLeftWall;
                                            }
                                            if (positions[8])
                                            {
                                                //closed at bottom
                                                fate = m_closedDownWall;
                                            }
                                        }
                                        GameObject newObj = Instantiate(fate);
                                        newObj.transform.position = new Vector2(ix * m_tilesize.x - m_offset.x, iy * m_tilesize.y - m_offset.y);
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
