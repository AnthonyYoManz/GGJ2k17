using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomScript : MonoBehaviour
{

    public GameObject m_explosionPrefab;
    public DoorScript m_exit;
    private List<GameObject> m_roomEnemies;

    public BoxCollider2D m_collider { get; private set; }

    //Don't know if we need any update stuff yet so leave it for now
	// Use this for initialization
	void Awake ()
    {
        Debug.Assert(m_exit);
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
}
