interface IJsonSerializer
{
    string Serialize<TData>(TData data);

    TData Deserialize<TData>(string json);

    void DeserializeOverwrite<TData>(string json, TData obj);
}