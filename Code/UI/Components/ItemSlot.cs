using Godot;

[GlobalClass]
public partial class ItemSlot : BaseButton
{
    public void Construct(Item item, int amount)
    {
        if (item != null)
        {
            TextureRect iconNode = GetNode<TextureRect>("Icon");
            if (iconNode != null) iconNode.Texture = item.Icon;

            Label labelNode = GetNode<Label>("Label");
            if (labelNode != null) labelNode.Text = item.Name;

            Label countNode = GetNode<Label>("Count");
            if (countNode != null) countNode.Text = amount.ToString();
        }
    }

}
