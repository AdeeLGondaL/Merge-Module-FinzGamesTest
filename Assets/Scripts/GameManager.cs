using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    private List<int> numbersInGrid;
    private List<int> numbersDiscovered;

    [SerializeField] private GameObject newNumberPopup;
    
    void Start()
    {
        numbersEnabled = new List<Transform>();
        spawnedNumbersButtons = new List<Button>();
        numbersInGrid = new List<int>();
        numbersDiscovered = new List<int>();
        Button temp;
        foreach (var spN in spawnedNumbers)
        {
            temp = spN.GetComponent<Button>();
            temp.onClick.AddListener((() =>
            {
                AudioManager.Instance.PLaySelectSound();
                spN.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.25f, 10, 0f);
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
        newObject.gameObject.SetActive(true);
        // last number should be equal to one of the numbers already present in the grid
        if (numbersEnabled.Count == spawnedNumbers.Count - 1)
        {
            // Debug.Log("Last Number");
            // // newObject.GetComponent<SpawnedNumber>().numberText.text = numbersEnabled[Random.Range(0, numbersEnabled.Count)].GetComponent<SpawnedNumber>().numberText.text;
            // Debug.Log($"Numbers in Grid: ");
            // foreach (var n in numbersInGrid)
            // {
            //     Debug.Log(n);
            // }
            newObject.GetComponent<SpawnedNumber>().numberText.text = numbersInGrid[0].ToString();
        }
        var n = int.Parse(newObject.GetComponent<SpawnedNumber>().numberText.text);
        numbersInGrid.Add(n);
        numbersInGrid.Sort();
        if (!numbersDiscovered.Contains(n) && n > 32)
        {
            AudioManager.Instance.PlayYayyySound();
            newNumberPopup.SetActive(true);
            numbersDiscovered.Add(n);
        }
        numbersEnabled.Add(newObject);
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
            image.color = highlight ? Color.yellow : new Color(Random.Range(0.4f, 1.0f),Random.Range(0.4f, 1.0f),Random.Range(0.4f, 1.0f));
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
            numbersInGrid.Remove(num1);
            numbersInGrid.Remove(num2);
            // Debug.Log($"Numbers in Grid: ");
            // foreach (var n in numbersInGrid)
            // {
            //     Debug.Log(n);
            // }
            int mergedValue = num1 + num2;
            numbersInGrid.Add(mergedValue);
            AudioManager.Instance.PlayMergeSound();
            firstSelected.GetComponent<SpawnedNumber>().numberText.text = mergedValue.ToString();
            DOVirtual.DelayedCall(0.2f, () =>
            {
                if (!numbersDiscovered.Contains(mergedValue) && mergedValue > 32)
                {
                    AudioManager.Instance.PlayYayyySound();
                    newNumberPopup.SetActive(true);
                    numbersDiscovered.Add(mergedValue);
                }
            });
            HighlightSelected(firstSelected, false);
            HighlightSelected(secondSelected, false);
            secondSelected.gameObject.SetActive(false);
            numbersEnabled.Remove(secondSelected);
            ResetSelection();
        }
        else
        {
            AudioManager.Instance.PlayErrorSound();
            firstSelected.DOShakeRotation(0.3f, Vector3.one*6f);
            secondSelected.DOShakeRotation(0.3f, Vector3.one*6f);
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
