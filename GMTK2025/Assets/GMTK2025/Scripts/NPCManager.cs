using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public bool Spawn;
    public NPC NPCPrefab;

    private float _spawnTimer = 0f;
    private float _spawnInterval = 3f; 
    private Vector3 _spawnPosition = new Vector3(0, 10, 0); 

    private void Update()
    {
        if(Spawn)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnInterval)
            {
                var npc = Instantiate(NPCPrefab, _spawnPosition, Quaternion.identity);
                _spawnTimer = 0f; // Reset the timer after spawning
            }
        }
    }
}
