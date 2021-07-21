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
    public Sprite Icon;
    public Color Color;
    public List<string> Requirements;
    public float ProduceSpeed;
    public string Product;

    public override string ToString()
    {
        string requirements = "";
        foreach (string req in Requirements)
        {
            requirements += req + ", ";
        }
        return $"Id {Id} \n {Description} \nSprite {Icon} \nColor {Color} \n Color \n {requirements} \n {ProduceSpeed} \n {Product}";
    }
}
[System.Serializable]
public struct Options
{
    public string Id;
    public string Description;
    public Sprite Icon;
    public Color Color;
    public List<string> Requirements;
    public float ProduceSpeed;
    public string Product;

    public Options(NodeOptions op)
    {
        Id = op.Id;
        Description = op.Description;
        Icon = op.Icon;
        Color = op.Color;
        Requirements = op.Requirements;
        ProduceSpeed = op.ProduceSpeed;
        Product = op.Product;
    }
}

