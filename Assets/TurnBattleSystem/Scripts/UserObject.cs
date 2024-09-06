using System;

[System.Serializable]
public class UserObjects
{
    public string name;
    public string description;
    public string iconFile;
    public string path;

    // Constructor
    public UserObjects(string name, string description, string iconFile,string path)
    {
        this.name = name;
        this.description = description;
        this.iconFile = iconFile;
        this.path = path;
    }

    // Default constructor
    public UserObjects()
    {
    }
}
