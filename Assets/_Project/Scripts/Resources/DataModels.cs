using SQLite;

[Table("Items")]
public class Item
{
    [PrimaryKey]
    public int id { get; set; }

    [Column("item_name")]
    public string itemName { get; set; }

    public string description { get; set; }

    [Column("sprite_path")]
    public string spritePath { get; set; }

    [Column("location_id")]
    public int locationId { get; set; }

    [Column("is_takeable")]
    public bool isTakeable { get; set; }
}

[Table("Flags")]
public class Flag
{
    [PrimaryKey]
    [Column("flag_name")]
    public string flagName { get; set; }

    [Column("flag_value")]
    public bool flagValue { get; set; }
}

[Table("Inventory")]
public class InventorySlot
{
    [Column("item_id")]
    public int itemId { get; set; }

    [Column("slot_index")]
    public int slotIndex { get; set; }
}

[Table("PlayerProgress")]
public class PlayerProgress
{
    [PrimaryKey]
    public int Id { get; set; } = 1; // всегда одна запись
    public string currentLocation { get; set; }
}

[Table("Actions")]
public class ActionData
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string objectId { get; set; }
    public int priority { get; set; }
    public string conditionFlag { get; set; }
    public string actionType { get; set; }
    public string actionValue { get; set; }
    public string failMessage { get; set; }
}


[Table("Dialogues")]
public class DialogueRecord
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string npc_id { get; set; }
    public string trigger_flag { get; set; }
    public string set_flag { get; set; }
    public int priority { get; set; }
    public string speaker { get; set; }
    public string text { get; set; }
    public int? next_id { get; set; } // может быть NULL
}