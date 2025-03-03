using System;
using UnityEngine;

/// <summary>
/// Represents the status of a grid cell.
/// </summary>
[Serializable]
public enum CellStatus
{
    [Tooltip("The cell is empty and available for placement.")]
    EMPTY,

    [Tooltip("The cell is occupied and cannot be used for placement.")]
    FILLED
}
