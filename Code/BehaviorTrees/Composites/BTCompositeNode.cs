using System.Collections.Generic;

public abstract class BTCompositeNode : BTNode
{
    public List<BTNode> Children { get; set; }

    public bool AddChild(BTNode child)
    {
        Children.Add(child);
        
        // Incase we need any validation
        return true;
    }

    public bool RemoveChild(BTNode child)
    {
        return Children.Remove(child);
    }

}