using Godot;

[GlobalClass]
public partial class ItemSlot : BaseButton
{
    [Export] public TextureRect Icon { get; set; }
    [Export] public Label TextLabel { get; set; }
    [Export] public Label CountLabel { get; set; }

    public void Construct(Item item, string count)
    {
        if (item != null)
        {
            Icon.Texture = item.Icon;
            TextLabel.Text = item.Name;
        }
        CountLabel.Text = count;
    }

    public void Construct(Texture2D icon, string name, string count)
    {
        Icon.Texture = icon;
        TextLabel.Text = name;
        CountLabel.Text = count;
    }

}
