using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private static readonly string[] types = { "spd_up", "spd_down"};
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;
    private int _typeIndex;
    private string _type;
    private const int _lifetime = 3;
    private float _timer = 0f;

    private void Start()
    {
        this._typeIndex = Random.Range(0, types.Length);
        this._type = types[this._typeIndex];
        this._spriteRenderer = GetComponent<SpriteRenderer>();
        this._spriteRenderer.sprite = this._sprites[this._typeIndex];
    }

    // Update is called once per frame
    private void Update()
    {
        this._timer += Time.deltaTime;
        if (this._timer >= _lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    public string GetPowerUpType()
    {
        return this._type;
    }
}
