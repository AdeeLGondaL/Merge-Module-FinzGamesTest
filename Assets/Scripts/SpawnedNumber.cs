using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnedNumber : MonoBehaviour
{
    public TextMeshProUGUI numberText;

    private void OnEnable()
    {
        numberText.text = (Mathf.Pow(2, Random.Range(1, 6))).ToString(CultureInfo.CurrentCulture);
    }
}
