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