using System;
using UnityEngine;

/// <summary>
/// Represents the status of a cell, indicating whether it is available for placement or occupied.
/// </summary>
public enum CellStatus
{
    /// <summary>
    /// The cell is empty and available for placement.
    /// </summary>
    [Tooltip("The cell is empty and available for placement.")]
    EMPTY,

    /// <summary>
    /// The cell is occupied and cannot be used for placement.
    /// </summary>
    [Tooltip("The cell is occupied and cannot be used for placement.")]
    FILLED
}
