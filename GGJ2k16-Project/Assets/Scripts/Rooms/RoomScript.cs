using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomScript : MonoBehaviour
{

    public GameObject m_explosionPrefab;
    public DoorScript m_exit;
    private List<GameObject> m_roomEnemies;
    private bool m_awake;

    public BoxCollider2D m_collider { get; private set; }

    //Don't know if we need any update stuff yet so leave it for now
	// Use this for initialization
	void Awake ()
    {
        Debug.Assert(m_exit);
        m_awake = false;
        m_collider = GetComponent<BoxCollider2D>();
        m_roomEnemies = new List<GameObject>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Vector3 epos = enemy.transform.position;
            epos.z = transform.position.z;
            if(m_collider.bounds.Contains(epos))
            {
                m_roomEnemies.Add(enemy);
                enemy.gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
    
    }

    public void PlayersAreReady()
    {
     
        foreach (GameObject enemy in m_roomEnemies)
        {
            enemy.SetActive(true);
        }
    }
	// Update is called once per frame
	void Update () {
        if (m_exit)
        {
            if (m_exit.Triggered())
            {
                //room complete
                ExploderiseEnemiesPlease();
            }
        }
	}

    ////So, an enemy calls this to kill itself. NIOCE.
    //public void KillMePls(GameObject who)
    //{
    //    bool gotem = false;
    //    foreach (GameObject enemy in m_roomEnemies)
    //    {
    //        if (m_explosionPrefab && enemy == who)
    //        {
    //            GameObject explosion = Instantiate(m_explosionPrefab);
    //            explosion.transform.position = enemy.transform.position;
    //            gotem = true;
    //            break;
    //        }
    //    }
    //    if (gotem)
    //    {
    //        m_roomEnemies.Remove(who);
    //        Destroy(who);
    //    }
    //}
    public void ExploderiseEnemiesPlease()
    {
        foreach(GameObject enemy in m_roomEnemies)
        {

            if (m_explosionPrefab)
            {
                GameObject explosion = Instantiate(m_explosionPrefab);
                explosion.transform.position = enemy.transform.position;
            }
            Destroy(enemy);
        }
        m_roomEnemies.Clear();
    }

    public void SleepBaddies()
    {
        foreach (GameObject enemy in m_roomEnemies)
        {
            enemy.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.tag == "Player")
        {
            if (!m_awake)
            {
                m_awake = true;
                PlayersAreReady();

            }
        }
    }

    void OnTriggerExit2D(Collider2D _col)
    {
        if(_col.tag == "Player")
        {
            if(m_awake)
            {
                m_awake = false;
                SleepBaddies();
            }
        }
    }
}
