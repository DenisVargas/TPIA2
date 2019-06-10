using System;
using UnityEngine;

public interface IGridEntity
{
    /// <summary>
    /// Devuelve una referencia al evento OnMoveEvent de la entidad.
    /// </summary>
    Action<IGridEntity> OnMoveEvent { get; set; }
    Vector3 positionInWorld { get; }
}
