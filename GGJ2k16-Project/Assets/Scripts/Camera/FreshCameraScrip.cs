using UnityEngine;
using System.Collections;

/// <summary>
/// There must be a GameManager in the scene
/// </summary>
public class FreshCameraScrip : MonoBehaviour
{
    public float            m_transitionDuration;
    //public float            m_tileSize;
    public Vector2          m_cameraExtents { get;  set; }
    public Camera           m_camera { get; private set; }
    private Vector3         m_target ;
    private Vector3         m_start;
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

 
        Rect cell;
        if (GameManager.s_singleton.m_worldGrid.GetCellRectFromPoint(transform.position, out cell))
        {
            m_inTransition = false;
            m_target = cell.center;
            m_target.z = m_camZ;
            transform.position = m_start = m_target;
        }

        //m_camera.orthographicSize = Screen.height / (2 * m_tileSize);
        //m_cameraExtents = new Vector2(m_camera.orthographicSize, m_cameraExtents.y * Screen.width / Screen.height);

        //m_camera.orthographicSize = GameManager.s_singleton.m_worldGrid.ClalculateOrthoCamSize();
        m_cameraExtents = new Vector2(m_camera.orthographicSize, m_cameraExtents.y * Screen.width / Screen.height);
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
                if (GameManager.s_singleton.m_worldGrid.GetCellRectFromPoint(p.transform.position, out cell)
                                                                && cell.center != (Vector2)m_target)
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
