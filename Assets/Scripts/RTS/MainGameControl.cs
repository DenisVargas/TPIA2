using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameControl : MonoBehaviour {
    [SerializeField]
    public static List<GameObject> SelectedObjects = new List<GameObject>();
    public static GameObject LastObjectSelected = null;
    Vector3 pos = Vector3.zero;
	
	//Update is called once per frame
	//void Update () {
 //       print("Unidades seleccionadas: " + SelectedObjects.Count);
 //       pos = returnSelectedObjectsRelativePosition();
 //       print("Posicion Relativa" + pos.ToString());
 //   }

    public static Vector3 returnSelectedObjectsRelativePosition()
    {
        Vector3 Gpos = Vector3.zero;
        if (SelectedObjects.Count > 0)
        {
            foreach (var item in SelectedObjects)
            {
                if (Gpos == Vector3.zero)
                    Gpos = item.transform.position;
                else
                    Gpos += item.transform.position;
            }
            Gpos /= SelectedObjects.Count;

            return Gpos;
        }
        else
            return Vector3.zero;
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(pos, new Vector3(1, 1, 1));
    }*/
}
