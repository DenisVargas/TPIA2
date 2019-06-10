using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA.SpatialGrid;
//using IA.SpatialGrid.Selections;

public class RandomEnemy : MonoBehaviour, IGridEntity
{
    public event Action<IGridEntity> OnMove = delegate { };
    public Action<IGridEntity> OnMoveEvent { get => OnMove; set { OnMove += value; } }
    public Vector3 positionInWorld { get { return transform.position; } }

    public SpatialGrid test; 

    // Start is called before the first frame update
    void Start()
    {
        
        //Using a Selector.
        //BoxSelection selector = new BoxSelection(test, 10, 10);
        //CircleSelection selector2 = new CircleSelection(test, 10);
        //selector.Select(transform.position);

        //var wariudo = test.Query(transform.position, transform.position + new Vector3(10, 0, 30), x => true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 10 * Time.deltaTime;
        //Implementación de IGridEntity
        OnMove(this);
    }
}
