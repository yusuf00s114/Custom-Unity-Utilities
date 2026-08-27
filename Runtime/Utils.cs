using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{
    /// <summary>
    ///     Clamps an angle between a minimum and maximum value, handling wrap-around at 360 degrees.
    /// </summary>
    /// <param name="angle">The angle to clamp.</param>
    /// <param name="min">The minimum angle.</param>
    /// <param name="max">The maximum angle.</param>
    /// <returns>The clamped angle.</returns>
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }


    /// <summary>
    ///     Randomly shuffles the elements of a list in place using the Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    public static void ShuffleList<T>(this IList<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    /// <summary>
    ///     Calculates the Y position for <paramref name="topCollider" /> so that
    ///     it rests on top of <paramref name="bottomCollider" />. <br />
    ///     If <paramref name="bottomCollider" /> is set to null,
    ///     the method will treat <paramref name="topCollider" /> as the first
    ///     object in the stack. In this case, the returned Y position will be
    ///     equivalent to the object offset by half the height of its collider.
    /// </summary>
    /// <remarks>
    ///     This method assumes that
    ///     <paramref name="topCollider" /> and <paramref name="bottomCollider" />
    ///     are not rotated.
    /// </remarks>
    /// <param name="topCollider">The object to calculate the position of.</param>
    /// <param name="bottomCollider">
    ///     Can be null.
    ///     The object that <paramref name="topCollider" /> is
    ///     supposed to 'rest on'.
    /// </param>
    /// <param name="offsetY">
    ///     The offset in the Y direction
    ///     of <paramref name="topCollider" /> from its initial position.
    /// </param>
    /// <returns>
    ///     The Y position that <paramref name="topCollider" /> would have
    ///     if it was 'resting' on top of <paramref name="bottomCollider" />
    /// </returns>
    public static float CalculateStackPositionY(
        BoxCollider topCollider, BoxCollider bottomCollider, out float offsetY)
    {
        float newY = 0;
        float offset = 0;

        if (bottomCollider == null)
        {
            newY = topCollider.gameObject.transform.localPosition.y;
            offset = topCollider.CalculateBounds().size.y / 2;
        }
        else
        {
            newY = bottomCollider.gameObject.transform.localPosition.y;
            offset = bottomCollider.CalculateBounds().size.y / 2
                      + topCollider.CalculateBounds().size.y / 2;
            offset = offset - topCollider.center.y + bottomCollider.center.y;
            //offset = MathF.Abs(offset);
        }

        newY += offset;
        offsetY = offset;
        return newY;
    }

    /// <inheritdoc cref="CalculateStackPositionY(BoxCollider, BoxCollider, out float)" />
    public static float CalculateStackPositionY(
        BoxCollider topCollider, BoxCollider bottomCollider)
    {
        return CalculateStackPositionY(topCollider, bottomCollider, out _);
    }
    
    /// <summary>
    /// Helper function to extract the first active layer index from a mask
    /// </summary>
    public static int GetLayerIndexFromMask(LayerMask mask)
    {
        int bitmask = mask.value;
        for (int i = 0; i < 32; i++)
        {
	        // Check if the i-th bit is turned on
	        if ((bitmask & (1 << i)) != 0)
	        {
		        return i;
	        }
        }
        return 0; // Fallback to Default layer if nothing was selected
    }

    public static LayerMask GetLayerMaskFromIndex(int index)
    {
        return (LayerMask)(1 << index);
    }
    
    /// <summary>
    /// A script that creates and configures an Outline GameObject based on the given parentObject's and its children's
    /// meshes (using the MeshFilter components on the parent and children).
    /// </summary>
    /// <param name="parentObject"> The GameObject for which to create an outline.</param>
    /// <param name="outlineMaterial"> The material that will be assigned to all the Outline GameObjects.</param>
    /// <param name="renderingLayerMask"> The rendering layers that will be assigned to all the Outline GameObjects. </param>
    /// <param name="outlineLayer"> The outline layer that will be assigned to all the Outline GameObjects. </param>
    /// <param name="activeStateOnCreation"> Whether the container object will be active or not when created. </param>
    /// <returns> A container (parent) object for all Outline GameObjects; null if no MeshFilters were found on
    /// the parentObject or its active children. </returns>
    
    /*public static GameObject CreateOutlineObject(
	    GameObject parentObject,
	    Material outlineMaterial,
	    uint renderingLayerMask = 1, 
	    int outlineLayer = 1,
	    bool activeStateOnCreation = true
	    )
	{

        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>(false);

        if (meshFilters.Length == 0)
        {
            Debug.LogWarning($"Create Outline GameObject: No MeshFilters found on '{parentObject.name}' or its active children.");
            return null;
        }
        
        GameObject container = new GameObject($"{parentObject.name}_Outline");
        container.transform.SetParent(parentObject.transform, false);
        
        container.SetActive(activeStateOnCreation);
        _ = container.AddComponent<ToggleableOutlines.ToggleableOutline>();
        
        foreach (MeshFilter mf in meshFilters)
        {
            GameObject outlineChild = new GameObject($"{mf.gameObject.name}_Outline");
            outlineChild.transform.SetParent(container.transform, false);
            
            outlineChild.transform.position = mf.transform.position;
            outlineChild.transform.rotation = mf.transform.rotation;

            outlineChild.layer = outlineLayer;
            
            Vector3 worldScale = mf.transform.lossyScale;
            Vector3 parentWorldScale = container.transform.lossyScale;
            outlineChild.transform.localScale = new Vector3(
                parentWorldScale.x != 0 ? worldScale.x / parentWorldScale.x : 1,
                parentWorldScale.y != 0 ? worldScale.y / parentWorldScale.y : 1,
                parentWorldScale.z != 0 ? worldScale.z / parentWorldScale.z : 1
            );
            
            MeshFilter newFilter = outlineChild.AddComponent<MeshFilter>();
            newFilter.sharedMesh = mf.sharedMesh;
            
            MeshRenderer newRenderer = outlineChild.AddComponent<MeshRenderer>();
            
            newRenderer.sharedMaterial = outlineMaterial;
            newRenderer.renderingLayerMask = renderingLayerMask;
            newRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }
        
        return container;
	}*/
    
	/// <summary>
	/// Casts a ray from an origin transform in a specified direction to find a surface position.
	/// </summary>
	/// <param name="origin">The starting transform of the raycast.</param>
	/// <param name="layers">The LayerMask to filter which surfaces the ray can hit.</param>
	/// <param name="direction">The direction to cast. Defaults to the transform's downward facing direction.</param>
	/// <param name="maxDistance">Defaults to Mathf.Infinity</param>
	/// <returns>The Vector3 position of the surface, or the origin's position if nothing is hit.</returns>
	public static Vector3 GetSurfacePosition(
		Transform origin,
		LayerMask layers,
		Vector3 direction = default,
		float maxDistance = Mathf.Infinity
		)
	{
		// If no direction is provided (or Vector3.zero is passed), 
		// default to pointing straight "down" relative to the object's orientation.
		// For a counter setup, -origin.up or Vector3.down is usually ideal.
		if (direction == default)
		{
			direction = -1 * origin.up; 
		}

		// Perform the raycast. Adjust maxDistance (currently Mathf.Infinity) if you want to limit the range.
		if (Physics.Raycast(origin.position, direction, out RaycastHit hit, maxDistance, layers))
		{
			return hit.point;
		}

		// Fallback: If the raycast misses everything, return the origin position 
		// so your item doesn't teleport to the center of the world.
		return origin.position;
	}
}
