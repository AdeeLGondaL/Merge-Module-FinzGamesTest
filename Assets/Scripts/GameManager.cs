using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnedNumbers;
    [SerializeField] private Button spawnButton;
    private List<Transform> numbersEnabled;
    private List<Button> spawnedNumbersButtons;
    
    private Transform firstSelected;
    private Transform secondSelected;
    
    void Start()
    {
        numbersEnabled = new List<Transform>();
        spawnedNumbersButtons = new List<Button>();
        Button temp;
        foreach (var spN in spawnedNumbers)
        {
            temp = spN.GetComponent<Button>();
            temp.onClick.AddListener((() =>
            {
                AudioManager.Instance.PLaySelectSound();
                SelectNumber(spN);
            }));
            spawnedNumbersButtons.Add(temp);
        }
        spawnButton.onClick.AddListener(SpawnButton);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SpawnButton()
    {
        AudioManager.Instance.PlayClickSound();
        if(numbersEnabled.Count >= spawnedNumbers.Count) return;
        
        var newObject = spawnedNumbers[Random.Range(0, spawnedNumbers.Count)];
        while (numbersEnabled.Contains(newObject) && numbersEnabled.Count <= spawnedNumbers.Count)
        {
            newObject = spawnedNumbers[Random.Range(0, spawnedNumbers.Count)];
        }
        numbersEnabled.Add(newObject);
        newObject.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="selected"></param>
    public void SelectNumber(Transform selected)
    {
        if (firstSelected == null)
        {
            firstSelected = selected;
            HighlightSelected(firstSelected, true);
        }
        else if (secondSelected == null && selected != firstSelected)
        {
            secondSelected = selected;
            HighlightSelected(secondSelected, true);
            TryMerge();
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="selected"></param>
    /// <param name="highlight"></param>
    private void HighlightSelected(Transform selected, bool highlight)
    {
        var image = selected.GetComponent<Image>();
        if (image != null)
        {
            // image.color = highlight ? Color.yellow : new Color(0.5660378f,0.3700247f,0.3017088f);
            image.color = highlight ? Color.yellow : new Color(Random.Range(0.1f, 1.0f),Random.Range(0.1f, 1.0f),Random.Range(0.1f, 1.0f));
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void TryMerge()
    {
        if (firstSelected == null || secondSelected == null) return;

        int num1 = int.Parse(firstSelected.GetComponent<SpawnedNumber>().numberText.text);
        int num2 = int.Parse(secondSelected.GetComponent<SpawnedNumber>().numberText.text);

        if (num1 == num2)
        {
            int mergedValue = num1 + num2;
            AudioManager.Instance.PlayMergeSound();
            firstSelected.GetComponent<SpawnedNumber>().numberText.text = mergedValue.ToString();
            HighlightSelected(firstSelected, false);
            HighlightSelected(secondSelected, false);
            secondSelected.gameObject.SetActive(false);
            numbersEnabled.Remove(secondSelected);
            ResetSelection();
        }
        else
        {
            AudioManager.Instance.PlayErrorSound();
            HighlightSelected(firstSelected, false);
            HighlightSelected(secondSelected, false);
            ResetSelection();
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void ResetSelection()
    {
        firstSelected = null;
        secondSelected = null;
    }
}
