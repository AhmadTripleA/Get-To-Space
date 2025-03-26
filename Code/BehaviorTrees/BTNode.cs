public enum NodeState { Success, Failure, Running }

public abstract class BTNode
{
    public abstract NodeState Tick();
    public BTNode Parent { get; set; }

}