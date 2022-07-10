using System.Collections.Generic;
using Godot;

/// <summary>
/// Extensions for navigating the node tree.
/// </summary>
public static class TreeExtensions
{
    /// <summary>
    /// Finds the first child of this node with the given type.
    /// </summary>
    /// <param name="node">The node to search under.</param>
    /// <param name="missing">A value to return if no such child is found.</param>
    /// <typeparam name="T">The type to search for.</typeparam>
    /// <returns>The first child of type T, or null if none are found.</returns>
    public static T FindChild<T>(this Node node, T missing = default)
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T t1)
                return t1;
            if (child.FindChild<T>(missing) is T t2)
                return t2;
        }

        return missing;
    }

    /// <summary>
    /// Finds the first parent of this node with the given type.
    /// </summary>
    /// <param name="node">The node to search under.</param>
    /// <param name="missing">A value to return if no such parent is found.</param>
    /// <typeparam name="T">The type to search for.</typeparam>
    /// <returns>The first child of type T, or null if none are found.</returns>
    public static T FindParent<T>(this Node node, T missing = default)
    {
        Node parent = node.GetParent();
        if (parent is T t)
            return t;
        if (parent != null)
            return parent.FindParent<T>(missing);
        return missing;
    }

    /// <summary>
    /// Finds all children of this node with the given type.
    /// </summary>
    /// <param name="node">The node to search under.</param>
    /// <typeparam name="T">The type to search for.</typeparam>
    /// <returns>A list of children of the given type.</returns>
    public static IEnumerable<T> FindChildren<T>(this Node node)
    {
        List<T> children = new List<T>();
        
        foreach (Node child in node.GetChildren())
        {
            if (child is T t1)
                children.Add(t1);
            var t2 = child.FindChildren<T>();
            children.AddRange(t2);
        }

        return children;
    }
}