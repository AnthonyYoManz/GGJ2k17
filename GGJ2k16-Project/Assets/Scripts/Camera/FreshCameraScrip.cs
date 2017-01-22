using UnityEngine;
using System.Collections;

public class FreshCameraScrip : MonoBehaviour
{
    public float            m_transitionDuration;
    public float            m_tileSize;
    public Vector2          m_cameraExtents;
    public WorldGridScript  m_worldGrid;
   
    public Camera           m_camera;
    public Vector3          m_target;
    public Vector3          m_start;
    private int             m_camZ = -10;
    private bool            m_inTransition;
    private Timer           m_transitionTimer;
    void Awake()
    {
        m_transitionTimer = new Timer();
        m_camera = GetComponent<Camera>();
        m_inTransition = false;
    }
    // Use this for initialization
    void Start ()
    {
        m_camera.orthographicSize = Screen.height / (2 * m_tileSize);
        m_cameraExtents.y = m_camera.orthographicSize;
        m_cameraExtents.x = m_cameraExtents.y * Screen.width / Screen.height;

        Rect cell;
        if (m_worldGrid.GetCellRectFromPoint(transform.position, out cell))
        {
            m_inTransition = false;
            m_target = cell.center;
            m_target.z = m_camZ;
            transform.position = m_start = m_target;
        }
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (m_inTransition)
        {
            float progress = m_transitionTimer.Elapsed() / m_transitionDuration;
            transform.position = Vector3.Lerp(m_start, m_target, progress);

            if (transform.position == m_target)
            {
                m_inTransition = false;
                m_start = transform.position;
                m_target = transform.position;
               
            }
        }
        else
        {
            foreach (Player p in GameManager.s_singleton.GetPlayers())
            {
                Rect cell;
                if (m_worldGrid.GetCellRectFromPoint(p.transform.position, out cell) && cell.center != (Vector2)m_target)
                {
                    if (cell.Contains(p.transform.position) )
                    {
                        Debug.Log("starting transition");
                        m_target = cell.center;
                        m_target.z = m_camZ;
                        m_inTransition = true;
                        m_start = transform.position;
                        m_transitionTimer.Restart();
                    }
                }
            }
        }
	}


}
