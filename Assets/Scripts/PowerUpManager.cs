using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject _powerUpPrefab;
    private const float _spawnInterval = 2f;
    private Vector2 _min;
    private Vector2 _max;

    private float _spawnTimer = 0f;

    private void Start()
    {
        _min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    // Update is called once per frame
    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            SpawnPowerUp();
            SpawnPowerUp();
            _spawnTimer = 0f;
        }
    }

    private void SpawnPowerUp()
    {
        float x = Random.Range(_min.x, _max.x);
        float y = Random.Range(_min.y, _max.y);
        Vector2 position = new Vector2(x, y);
        GameObject placeholder = Instantiate(_powerUpPrefab, position, Quaternion.identity);
    }

}
