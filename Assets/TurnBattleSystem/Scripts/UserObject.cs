
public enum GearType {
    Weapon,
    Armor,
    Accessory,
    Other
}
public class UserObjects
{
    public string name;
        public string description;
    public string iconFile;
    public string path;

    // New attributes for equipment stats
    public int agility;
    public int intelligence;
    public int lifePoints; 

        public GearType Type { get; set; } // New GearType property

    // Constructor to initialize the attributes
    public UserObjects(string name, string description, string iconFile, string path,int agility, int intelligence, int lifePoints,GearType type)
    {
        this.name = name;
        this.description = description;
        this.iconFile = iconFile;
        this.path = path;
        
        this.agility = agility;
        this.intelligence = intelligence;
        this.lifePoints = lifePoints;
    }

    public UserObjects()
    {
      
    }
}
