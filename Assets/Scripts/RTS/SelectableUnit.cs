using UnityEngine;


namespace RTSSelection
{
    public class SelectableUnit : MonoBehaviour
    {
        [SerializeField]
        private bool Selected = false;

        public void Awake()
        {
            SelectionController.OnBoxSelection += BoxSelection;
        }

        public bool IsSelected()
        {
            return Selected;
        }

        public void selectUnit()
        {
            Selected = true;
            MainGameControl.SelectedObjects.Add(gameObject);
            MainGameControl.LastObjectSelected = gameObject;
        }

        public void DeselectUnit()
        {
            Selected = false;
            MainGameControl.SelectedObjects.Remove(gameObject);
        }

        private void BoxSelection()
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            if (SelectionController.selectionBoxRect.Contains(screenPosition))
            {
                if (!SelectionController.selectedUnits.Contains(this))
                    SelectionController.selectedUnits.Add(this);
                //print("FuiSeleccionado");
                selectUnit();
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl) && IsSelected())
                {
                    if (!SelectionController.selectedUnits.Contains(this))
                        SelectionController.selectedUnits.Add(this);
                }
            }
        }
    }
}
