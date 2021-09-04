using UnityEngine;

public static class TaskManager
{
    public static Instruction GetTask(TaskName task, NodeController node)
    {
        switch (task)
        {
            case TaskName.CreateOre:
                return new CreateOre(node, node.Options.Stats.ProduceSpeed);
            case TaskName.OreWork:
                return new OreWork(node, node.Options.Stats.ProduceSpeed);

            default: return null;
        }
    }
}