public interface ISaveable
{
    string Save();   // Returns serialized data (e.g., JSON string)
    void Load(string data); // Takes serialized data as input
}
