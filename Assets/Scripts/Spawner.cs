using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject ToSpawn;

    public float SpawnTime;

    public Transform Goal;

    private Coroutine _spawnLoop;

    private void Start()
    {
        var player = FindObjectOfType<PlayerController>();
        if (!player)
        {
            Debug.LogWarning("No player found, aborting spawner!");
            return;
        }

        Goal = player.transform;
        _spawnLoop = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {

        while (true)
        {
            var spawned = Instantiate(ToSpawn, transform.position, Quaternion.identity);
            var move = spawned.GetComponent<MoveToGoal>();
            move.Goal = Goal;
            move.Agent.speed = Random.Range(1f, 5f);

            yield return new WaitForSeconds(SpawnTime);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(_spawnLoop);
    }
}
