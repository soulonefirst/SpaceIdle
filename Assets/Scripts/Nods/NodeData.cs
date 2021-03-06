using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodesData
{
    public List<NodeOptions> NodeOptionsList;
}
[System.Serializable]
public class NodeOptions
{
    public string Id;
    public string Description;
    public string Icon;
    public Color Color;
    public Stats Stats;
    public List<TaskName> Requirements;
    public TaskName BaseTask;
    public NodeType NodeType;

    public override string ToString()
    {
        string requirements = "";
        foreach (TaskName taskName in Requirements)
        {
            requirements += taskName + ", ";
        }
        return $"Id {Id} \n {Description} \nSprite {Icon} \nColor {Color} \n Color \n {requirements} \n {BaseTask}\n {NodeType} ";
    }
}
public enum NodeStats
{
    ProduceSpeed,
    ConnectionAreaSize,
    HP
}
public struct Stats
{
    public float ProduceSpeed;
    public float ConnectionAreaSize;
    public float HP;
}
public enum NodeType
{
    Station,
    Ship
}
