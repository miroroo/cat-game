using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class DialogTrigger : MonoBehaviour
{
    [Header("Настройки персонажа")]
    [SerializeField] private string npcId;

    [Header("Файл с диалогами")]
    [SerializeField] private TextAsset dialogFile;  // csv файл со всеми репликами

    private List<DialogueLine> myLines = new List<DialogueLine>();
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    // класс для строки диалога
    [System.Serializable]
    public class DialogueLine
    {
        public string npcId;
        public int lineId;
        public string text;
        public string triggerFlag;
        public string nextLineId;
        public string setFlag;
    }


    void Start()
    {
        LoadLines();
    }

    void LoadLines()
    {
        if (dialogFile == null) return;

        string[] lines = dialogFile.text.Split('\n'); // чтение строк

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');

            if (values.Length >= 3)
            {
                if (values[0].Trim() == npcId)  // загрузка строк для персонажа
                {
                    DialogueLine line = new DialogueLine()
                    {
                        npcId = values[0].Trim(),
                        lineId = int.Parse(values[1].Trim()),
                        text = values[2].Trim(),
                        triggerFlag = values.Length > 3 ? values[3].Trim() : null,
                        nextLineId = values.Length > 4 ? values[4].Trim() : null,
                        setFlag = values.Length > 5 ? values[5].Trim() : null
                    };

                    if (string.IsNullOrEmpty(line.triggerFlag))  // если triggerFlag пустая строка, присваиваем null
                        line.triggerFlag = null;

                    myLines.Add(line);
                }
            }
        }

        // сортировка по id
        myLines = myLines.OrderBy(l => l.lineId).ToList();

        // Для отладки
        Debug.Log($"NPC {npcId} загрузил {myLines.Count} строк");
    }

    public DialogueLine GetFirstAvailableLine()
    {
        foreach (DialogueLine line in myLines)
        {
            if (line.triggerFlag == null)
                return line;

            if (flags.ContainsKey(line.triggerFlag) && flags[line.triggerFlag])
                return line;
        }

        return null;
    }

    public void SetFlag(string flagName, bool value)
    {
        if (flags.ContainsKey(flagName))
            flags[flagName] = value;
        else
            flags.Add(flagName, value);

        Debug.Log($"Флаг {flagName} = {value}");
    }

    public bool GetFlag(string flagName)
    {
        return flags.ContainsKey(flagName) && flags[flagName];
    }





    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialog()
    {
        DialogueLine firstLine = GetFirstAvailableLine();

        if (firstLine != null)
        {
            Debug.Log($"Диалог начат: {firstLine.text}");
        }
        else
        {
            Debug.Log("Нет доступных строк для диалога");
        }
    }

}
