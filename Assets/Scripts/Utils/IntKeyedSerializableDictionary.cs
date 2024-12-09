using System.Collections.Generic;

[System.Serializable]
public class IntKeyedSerializableDictionary
{
    public List<int> keys = new List<int>();
    public List<string> values = new List<string>();

    public IntKeyedSerializableDictionary() { }

    public IntKeyedSerializableDictionary(Dictionary<int, string> dictionary)
    {
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public Dictionary<int, string> ToDictionary()
    {
        var dictionary = new Dictionary<int, string>();
        for (int i = 0; i < keys.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
        return dictionary;
    }
}
