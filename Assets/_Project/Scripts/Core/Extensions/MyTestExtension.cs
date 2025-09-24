using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Extensions
{
    public static class MyTestExtension
    {
        public readonly struct HorizontalBounds
        {
            public float Left { get; }
            public float Right { get; }

            public HorizontalBounds(float left, float right)
            {
                Left = left;
                Right = right;
            }
        }
        
        public static HorizontalBounds GetScreenHorizontalBounds (this Camera camera, float offset = 0.5f)
        {
            Vector3 leftPoint = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 rightPoint = camera.ViewportToWorldPoint(new Vector3(1, 0, 0));
            return new HorizontalBounds(leftPoint.x - offset, rightPoint.x + offset);
        }
        
        public static T GetRandomByWeight<T>(this IEnumerable<T> items, Func<T, int> weightSelector)
        {
            var itemList = items.ToList();
            var totalWeight = itemList.Sum(weightSelector);
            var randomValue = Random.Range(0, totalWeight);
            var currentWeight = 0;

            foreach (var item in itemList)
            {
                currentWeight += weightSelector(item);
                if (randomValue < currentWeight)
                    return item;
            }

            return itemList.Last(); // fallback
        }
    }
}