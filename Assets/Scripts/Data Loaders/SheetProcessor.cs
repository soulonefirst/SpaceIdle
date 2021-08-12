using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SheetProcessor : MonoBehaviour
{
    private const char _cellSeporator = ',';
    private const char _inCellSeporator = ';';

    private Dictionary<string, Color> _colors = new Dictionary<string, Color>()
    {
        {"white", Color.white},
        {"black", Color.black},
        {"yellow", Color.yellow},
        {"red", Color.red},
        {"green", Color.green},
        {"blue", Color.blue},
        {"brown", new Color(139/255f,69/255f,19/255f) }
    };
    private Dictionary<string,TaskName> _taskName = EnumHelper.GetStringValuesPair<TaskName>();

    public NodesData ProcessNodeData(string cvsRawData)
    {
        char lineEnding = GetPlatformSpecificLineEnd();
        string[] rows = cvsRawData.Split(lineEnding);
        int dataStartRawIndex = 1;
        NodesData data = new NodesData();
        data.NodeOptionsList = new List<NodeOptions>();
        for (int i = dataStartRawIndex; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(_cellSeporator);
            for(int n = 0; n < cells.Length; n++)
            {
                cells[n] = cells[n].Trim('"');              
            }
            data.NodeOptionsList.Add(new NodeOptions()
            {
                Id = cells[0],
                Description = cells[1],
                Icon = LoadAssetBundle.GetSprite(cells[2]),
                Color = ParseColor(cells[3]),
                Requirements = ParseRequirements(cells[4]),
                ProduceSpeed = ParseFloat(cells[5]),
                BaseTask = _taskName[cells[6]],
                Draggable = ParseBool(cells[7])           
            }) ;
        }
        return data;
    }
    public void Test(string cvsRawData)
    {
        char lineEnding = GetPlatformSpecificLineEnd();
        string[] rows = cvsRawData.Split(lineEnding);
        int dataStartRawIndex = 0;
        for (int i = dataStartRawIndex; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(_cellSeporator);
            for (int n = 0; n < cells.Length; n++)
            {
                cells[n] = cells[n].Trim('"');
                Debug.Log(cells[n]);
            }
            
        }
    }
    private List<string> ParseRequirements(string r) 
    {
        List<string> requirements = new List<string>();
        string[] req = r.Split(_cellSeporator);
        foreach (string requirement in req)
        {
            requirements.Add(requirement);
        }
        return requirements;
    }
    private Color ParseColor(string color)
    {
        color = color.Trim();
        Color result = default;
        if (_colors.ContainsKey(color))
        {
            result = _colors[color];
        }
        return result;
    }

    private Vector3 ParseVector3(string s)
    {
        string[] vectorComponents = s.Split(_inCellSeporator);
        if (vectorComponents.Length < 3)
        {
            Debug.Log("Can't parse Vector3. Wrong text format");
            return default;
        }

        float x = ParseFloat(vectorComponents[0]);
        float y = ParseFloat(vectorComponents[1]);
        float z = ParseFloat(vectorComponents[2]);
        return new Vector3(x, y, z);
    }
    
    private int ParseInt(string s)
    {
        int result = -1;
        if (!int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.Log("Can't parse int, wrong text");
        }

        return result;
    }
    
    private float ParseFloat(string s)
    {
        float result = -1;
        if (!float.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.Log("Can't pars float,wrong text ");
        }

        return result;
    }

    private bool ParseBool(string s)
    {
        return s == "TRUE" ? true : false; 
    }

    private char GetPlatformSpecificLineEnd()
    {
        char lineEnding = '\n';
#if UNITY_IOS
        lineEnding = '\r';
#endif
        return lineEnding;
    }
}
