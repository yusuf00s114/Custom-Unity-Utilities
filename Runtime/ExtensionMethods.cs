using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Custom extension methods created by Yusuf Shabanov.
public static class ExtensionMethods
{
    /// <summary>
    ///     Returns a formatted string containing all elements in the list.
    ///     Usage: myList.ToStringContents();
    /// </summary>
    public static string ToStringContents<T>(this List<T> list)
    {
        if (list == null) return "List is null";
        if (list.Count == 0) return "List is empty";

        var sb = new StringBuilder();

        for (var i = 0; i < list.Count; i++)
        {
            sb.AppendLine($"Element {i}:");

            // Get the string representation and indent it for the "nested" look
            var elementString = list[i]?.ToString() ?? "null";

            // Split by lines to ensure every line of a multi-line ToString() is indented
            var lines = elementString.Split('\n');
            foreach (var line in lines) sb.AppendLine($"    {line.TrimEnd()}");
        }
        // Debug.LogWarning("ignore me :D");

        return sb.ToString().TrimEnd() + "\n";
    }
    
    /// <summary>
    ///     Returns a formatted string containing all key-value pairs in the dictionary.
    ///     Usage: myDictionary.ToStringContents();
    /// </summary>
    public static string ToStringContents<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null) return "Dictionary is null";
        if (dictionary.Count == 0) return "Dictionary is empty";

        var sb = new StringBuilder();
        int i = 0;

        foreach (var kvp in dictionary)
        {
            // Format the header with the index and the Key's string representation
            var keyString = kvp.Key?.ToString() ?? "null";
            sb.AppendLine($"Element {i} [Key: {keyString}]:");

            // Get the string representation of the Value and indent it for the "nested" look
            var valueString = kvp.Value?.ToString() ?? "null";

            // Split by lines to ensure every line of a multi-line ToString() is indented
            var lines = valueString.Split('\n');
            foreach (var line in lines) sb.AppendLine($"    {line.TrimEnd()}");

            i++;
        }
        // Debug.LogWarning("ignored you :D");

        return sb.ToString().TrimEnd() + "\n";
    }

    /// <summary>
    ///     Does the same calculation as Unity does when calculating Collider.bounds.
    ///     (Takes rotation into account).
    ///     The difference between this and Collider.bounds is that
    ///     Collider.bounds.size returns 0 when the Collider component is disabled,
    ///     while this accurately calculates the bounds even when the Collider component
    ///     is disabled.
    /// </summary>
    public static Bounds CalculateBounds(this BoxCollider box)
    {
        var t = box.transform;
        var center = box.center;
        var extents = box.size * 0.5f;

        // Define the 8 corners in local space
        var corners = new Vector3[8]
        {
            center + new Vector3(-extents.x, -extents.y, -extents.z),
            center + new Vector3(-extents.x, -extents.y, extents.z),
            center + new Vector3(-extents.x, extents.y, -extents.z),
            center + new Vector3(-extents.x, extents.y, extents.z),
            center + new Vector3(extents.x, -extents.y, -extents.z),
            center + new Vector3(extents.x, -extents.y, extents.z),
            center + new Vector3(extents.x, extents.y, -extents.z),
            center + new Vector3(extents.x, extents.y, extents.z)
        };

        // Create bounds and encapsulate the world-space corners
        var b = new Bounds(t.TransformPoint(corners[0]), Vector3.zero);
        for (var i = 1; i < 8; i++) b.Encapsulate(t.TransformPoint(corners[i]));
        return b;
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 WithXY(this Vector3 v, float x, float y)
    {
        return new Vector3(x, y, v.z);
    }

    public static Vector3 WithXZ(this Vector3 v, float x, float z)
    {
        return new Vector3(x, v.y, z);
    }

    public static Vector3 WithYZ(this Vector3 v, float y, float z)
    {
        return new Vector3(v.x, y, z);
    }

    public static Vector3 WithXYZ(this Vector3 v, float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    /// <summary>
    ///     Modifies <c>transform.position</c> by changing only the specified components. <br />
    ///     For example, <c>transform.SetPositionWithX(5)</c> will set the X component of the position to 5,
    ///     while keeping the Y and Z components unchanged. <br />
    ///     In other words, <c>transform.SetPositionWithX(5)</c> is equivalent to
    ///     <code>
    /// transform.position = new Vector3(5, transform.position.y, transform.position.z);
    /// </code>
    /// </summary>
    public static void SetPositionWithX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    /// <inheritdoc cref="SetPositionWithX" />
    public static void SetPositionWithY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    /// <inheritdoc cref="SetPositionWithX" />
    public static void SetPositionWithZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    /// <inheritdoc cref="SetPositionWithX" />
    public static void SetPositionWithXY(this Transform transform, float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    /// <inheritdoc cref="SetPositionWithX" />
    public static void SetPositionWithXZ(this Transform transform, float x, float z)
    {
        transform.position = new Vector3(x, transform.position.y, z);
    }

    /// <inheritdoc cref="SetPositionWithX" />
    public static void SetPositionWithXYZ(this Transform transform, float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    /// <summary>
    ///     Modifies <c>transform.position</c> by changing only the specified components. <br />
    ///     For example, <c>transform.SetLocalPositionWithX(5)</c> will set the X component of the position to 5,
    ///     while keeping the Y and Z components unchanged. <br />
    ///     In other words, <c>transform.SetLocalPositionWithX(5)</c> is equivalent to
    ///     <code>
    /// transform.localPosition = new Vector3(5, transform.localPosition.y, transform.localPosition.z);
    /// </code>
    /// </summary>
    public static void SetLocalPositionWithX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    /// <inheritdoc cref="SetLocalPositionWithX" />
    public static void SetLocalPositionWithY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }

    /// <inheritdoc cref="SetLocalPositionWithX" />
    public static void SetLocalPositionWithZ(this Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }

    /// <inheritdoc cref="SetLocalPositionWithX" />
    public static void SetLocalPositionWithXY(this Transform transform, float x, float y)
    {
        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
    }

    /// <inheritdoc cref="SetLocalPositionWithX" />
    public static void SetLocalPositionWithXZ(this Transform transform, float x, float z)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, z);
    }

    /// <inheritdoc cref="SetLocalPositionWithX" />
    public static void SetLocalPositionWithXYZ(this Transform transform, float x, float y, float z)
    {
        transform.localPosition = new Vector3(x, y, z);
    }

    /// <summary>
    ///     Returns a random element from this collection.
    /// </summary>
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
            throw new ArgumentNullException(nameof(enumerable));

        // If the collection is empty, ElementAtOrDefault or throwing an exception is best
        if (!enumerable.Any())
            throw new InvalidOperationException("Sequence contains no elements");

        return enumerable.ElementAt(UnityEngine.Random.Range(0, enumerable.Count() - 1));
    }

    /// <summary>
    ///     Returns a random element that is not the same as the 'last' provided element.
    /// </summary>
    public static T RandomNoRepeat<T>(this IEnumerable<T> enumerable, ref T last)
    {
        var localLast = last;

        var excluded = enumerable.Where(x => !EqualityComparer<T>.Default.Equals(x, localLast)).ToList();

        if (!excluded.Any()) return last; // Only one item exists, so we return it

        var result = excluded.Random();
        last = result;
        return result;
    }

    /// <summary>
    ///     Returns a random element from the list, ensuring every element is picked
    ///     once before repeating.
    /// </summary>
    public static T RandomUnique<T>(this IEnumerable<T> list, ref IEnumerable<T> remainingPool)
    {
        // If remainingPool is null or empty, refill it from the source list
        if (remainingPool == null || !remainingPool.Any()) remainingPool = list.ToList();

        // Pick a random item from the remaining pool
        var result = remainingPool.Random();

        // Remove the picked item from the remainingPool for the next call
        var selectedCopy = result;
        remainingPool = remainingPool.Where(x => !EqualityComparer<T>.Default.Equals(x, selectedCopy)).ToList();

        return result;
    }
    
    /// <summary>
    /// Selects a random key from the dictionary based on its float weight using UnityEngine.Random.
    /// </summary>
    public static T RandomWeighted<T>(this IDictionary<T, float> dictionary)
    {
        if (dictionary == null || dictionary.Count == 0)
        {
            throw new ArgumentException("Dictionary cannot be null or empty.");
        }

        // 1. Calculate the total weight sum
        float totalWeight = 0f;
        foreach (var weight in dictionary.Values)
        {
            if (weight < 0)
            {
                throw new ArgumentException("Weights cannot be negative.");
            }
            totalWeight += weight;
        }

        if (totalWeight <= 0f)
        {
            throw new InvalidOperationException("Total weight must be greater than zero.");
        }

        // 2. Roll the dice using Unity's Random.Range
        // For float arguments, Random.Range is inclusive of both 0f and totalWeight
        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        // 3. Find the item that corresponds to the rolled value
        foreach (var kvp in dictionary)
        {
            cumulativeWeight += kvp.Value;
            if (roll <= cumulativeWeight)
            {
                return kvp.Key;
            }
        }

        // Fallback for extreme floating-point rounding edge cases
        return dictionary.Keys.Last();
    }
    
    /// <summary>
    /// Returns a random key from the dictionary based on its weight, 
    /// ensuring every key is picked once before repeating.
    /// </summary>
    public static T RandomWeightedUnique<T>(this IDictionary<T, float> dictionary, ref IEnumerable<T> remainingPool)
    {
        if (dictionary == null || dictionary.Count == 0)
        {
            throw new ArgumentException("Dictionary cannot be null or empty.");
        }

        // 1. If the pool is null or empty, reset it with all dictionary keys
        if (remainingPool == null || !remainingPool.Any())
        {
            remainingPool = dictionary.Keys.ToList();
        }

        // 2. Calculate the total weight of ONLY the items left in the pool
        float totalWeight = 0f;
        foreach (T key in remainingPool)
        {
            if (dictionary.TryGetValue(key, out float weight))
            {
                if (weight < 0) throw new ArgumentException("Weights cannot be negative.");
                totalWeight += weight;
            }
        }

        if (totalWeight <= 0f)
        {
            throw new InvalidOperationException("Total weight of remaining items must be greater than zero.");
        }

        // 3. Roll the dice against the remaining pool's total weight
        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        T selectedKey = default;
        bool found = false;

        foreach (T key in remainingPool)
        {
            cumulativeWeight += dictionary[key];
            if (roll <= cumulativeWeight)
            {
                selectedKey = key;
                found = true;
                break;
            }
        }

        // Fallback for rare floating-point rounding edge cases
        if (!found)
        {
            selectedKey = remainingPool.Last();
        }

        // 4. Update the pool by removing the selected item
        // We convert to a list to modify it, then pass it back to the ref IEnumerable
        var updatedPool = remainingPool.ToList();
        updatedPool.Remove(selectedKey);
        remainingPool = updatedPool;

        return selectedKey;
    }
    
    /// <summary>
    /// Returns a random key from the dictionary based on its weight, 
    /// ensuring every key is picked once before repeating.
    /// </summary>
    public static T RandomWeightedUnique<T>(this IDictionary<T, float> dictionary, ref List<T> remainingPool)
    {
        if (dictionary == null || dictionary.Count == 0)
        {
            throw new ArgumentException("Dictionary cannot be null or empty.");
        }

        // 1. If the list is null or empty, reset it with all dictionary keys
        if (remainingPool == null || remainingPool.Count == 0)
        {
            remainingPool = dictionary.Keys.ToList();
        }

        // 2. Calculate total weight of items remaining in the pool
        float totalWeight = 0f;
        for (int i = 0; i < remainingPool.Count; i++)
        {
            T key = remainingPool[i];
            if (dictionary.TryGetValue(key, out float weight))
            {
                if (weight < 0) throw new ArgumentException("Weights cannot be negative.");
                totalWeight += weight;
            }
        }

        if (totalWeight <= 0f)
        {
            throw new InvalidOperationException("Total weight of remaining items must be greater than zero.");
        }

        // 3. Roll the dice
        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        T selectedKey = default;
        bool found = false;

        for (int i = 0; i < remainingPool.Count; i++)
        {
            T key = remainingPool[i];
            cumulativeWeight += dictionary[key];
            if (roll <= cumulativeWeight)
            {
                selectedKey = key;
                found = true;
                break;
            }
        }

        if (!found)
        {
            selectedKey = remainingPool[^1]; // Index from end operator (equivalent to remainingPool.Count - 1)
        }

        // 4. Remove the selected item directly from the list
        remainingPool.Remove(selectedKey);

        return selectedKey;
    }

    /// <summary>
    /// Sets all the children of this GameObject to the given layer.
    /// </summary>
    public static void SetLayerRecursively(this GameObject gameObject, int layer, bool includeInactive = true)
    {
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(includeInactive))
        {
            t.gameObject.layer = layer;
        }
    }
    
    /// <summary>
    /// Clamps this Vector3 component-wise between a minimum and maximum Vector3.
    /// </summary>
    public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Clamp(value.x, min.x, max.x),
            Mathf.Clamp(value.y, min.y, max.y),
            Mathf.Clamp(value.z, min.z, max.z)
        );
    }
}