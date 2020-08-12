using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singularity : MonoBehaviour
{
    public static bool isSingularityAwake = false;
    private GameObject player;
    [SerializeField] private GameObject body = default;
    [SerializeField] private AudioClip spawnClip = default;

    private void Start()
    {
        isSingularityAwake = true;
        player = GameObject.Find("Player");
        player.GetComponent<AudioSource>().PlayOneShot(spawnClip);
    }
    private void Update()
    {
        foreach(GameObject go in Unit.GetAllEnemies(player))
        {
            if (go.GetComponent<Unit>())
            {
                float distanceFactor = (9001 / Vector3.Distance(go.transform.position, body.transform.position));
                go.transform.position = Vector3.MoveTowards(go.transform.position, body.transform.position, distanceFactor* Time.deltaTime);

            }
        }
    }

    public void DestroySelf()
    {
        isSingularityAwake = false;
        Destroy(gameObject);
    }
}
