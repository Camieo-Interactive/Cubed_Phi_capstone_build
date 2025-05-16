using System;
using UnityEngine;

/// <summary>
/// Represents a set of comparison operations that can be used to compare values.
/// </summary>
[Serializable]
public enum ComparisonEnums
{
    /// <summary>
    /// Represents a comparison where the first value is less than the second value.
    /// </summary>
    [Tooltip("Less than")]
    LessThan,
    
    /// <summary>
    /// Represents a comparison where the first value is less than or equal to the second value.
    /// </summary>
    [Tooltip("Less than or equal to")]
    LessThanOrEqualTo,
    
    /// <summary>
    /// Represents a comparison where the first value is equal to the second value.
    /// </summary>
    [Tooltip("Equal to")]
    EqualTo,
    
    /// <summary>
    /// Represents a comparison where the first value is not equal to the second value.
    /// </summary>
    [Tooltip("Not equal to")]
    NotEqualTo,
    
    /// <summary>
    /// Represents a comparison where the first value is greater than or equal to the second value.
    /// </summary>
    [Tooltip("Greater than or equal to")]
    GreaterThanOrEqualTo,
    
    /// <summary>
    /// Represents a comparison where the first value is greater than the second value.
    /// </summary>
    [Tooltip("Greater than")]
    GreaterThan
}