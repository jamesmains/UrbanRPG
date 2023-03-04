using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class QuestDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private TextMeshProUGUI questTaskDescriptionText;
    [SerializeField] private TextMeshProUGUI questTaskProgressText;

    public void Setup(string qName, string qDescription, string qTaskDescription, string qTaskProgress)
    {
        questNameText.text = qName;
        questDescriptionText.text = qDescription;
        if (string.IsNullOrEmpty(qTaskDescription))
        {
            Destroy(questTaskDescriptionText.gameObject);
        }
        else
        {
            questTaskDescriptionText.text = qTaskDescription;
        }
        if (string.IsNullOrEmpty(qTaskProgress))
        {
            Destroy(questTaskProgressText.gameObject);
        }
        else
        {
            questTaskProgressText.text = qTaskProgress;
        }
    }
}
