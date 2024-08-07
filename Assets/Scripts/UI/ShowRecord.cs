using UnityEngine;
using UnityEngine.UI;

public class ShowRecord : MonoBehaviour
{
    [SerializeField] private RecordScriptableObject recordObject;
    [SerializeField] private Text recordText;

    public static string RecordPath = "Record";

    private void Awake()
    {
        if (!recordObject.RecordRecieved)
        {
            recordObject.Record = PlayerPrefs.GetFloat(RecordPath);
            recordObject.RecordRecieved = true;
        }
  
        recordText.text = "Record: " + recordObject.Record.ToString();
    }
}
