using System;

[Serializable]
public class SelectCharacterRequest
{
    public string type { get; set; }
    public SelectCharacterPayload payload;
}