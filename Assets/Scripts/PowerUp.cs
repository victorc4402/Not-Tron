using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private static readonly string[] types = { "spd_up", "spd_down"};
    private string _type;

    private void Start()
    {
        this._type = types[Random.Range(0, types.Length)];
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public string GetPowerUpType()
    {
        return this._type;
    }
}
