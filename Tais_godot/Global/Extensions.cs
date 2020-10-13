using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TaisGodot.Scripts
{
    public static class ObjectExtensions
    {
        public static IEnumerable<T> GetChildren<T>(this Node node) where T : Node
        {
            List<T> rslt = new List<T>();
            foreach(var child in node.GetChildren())
            {
                if(child is T)
                {
                    rslt.Add(child as T);
                }
            }

            return rslt;
        }
    }
}
