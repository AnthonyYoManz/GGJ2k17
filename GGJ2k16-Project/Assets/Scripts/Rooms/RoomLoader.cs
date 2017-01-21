using UnityEngine;
using System.Collections.Generic;

public class RoomLoader : MonoBehaviour {
    [System.Serializable]
    public struct ColObjPair
    {
        public Color m_col;
        public GameObject m_pref;
    }
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
                            GameObject pref = pear.m_pref;
                            if (pref)
                            {
                                GameObject newObj = Instantiate(pref);
                                newObj.transform.position = new Vector2(ix * m_tilesize.x - m_offset.x, iy * m_tilesize.y - m_offset.y);
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
