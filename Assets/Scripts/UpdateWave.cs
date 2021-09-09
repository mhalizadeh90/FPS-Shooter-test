using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWave : MonoBehaviour
{
    public Text waveNumber;

    void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveNumber;
    }

    void UpdateWaveNumber(int wave)
    {
        waveNumber.text = wave.ToString();
    }

    void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveNumber;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
