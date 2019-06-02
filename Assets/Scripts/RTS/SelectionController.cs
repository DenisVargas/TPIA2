using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RTSSelection
{
    public class SelectionController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        public static List<SelectableUnit> selectedUnits;

        [SerializeField]
        private Image selectionBoxImage;
        public static Rect selectionBoxRect;
        private Vector2 selectionStartPos;

        public delegate void SelectionAction();
        public static event SelectionAction OnBoxSelection;


        private void Awake()
        {
            selectedUnits = new List<SelectableUnit>();
        }

        private void Update()
        {
            SimpleSelection();
        }

        private void SimpleSelection()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(r, Mathf.Infinity);
                
                SelectableUnit selectedUnit = null;

                foreach (var item in hits)
                    if (item.collider.gameObject.GetComponentInParent<SelectableUnit>())
                        selectedUnit = item.collider.gameObject.GetComponent<SelectableUnit>();

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //Si mantuve Shift y la unidad es válida... 
                    //Me fijo si mi lista ya tiene al objeto, si no, lo añado y lo marco como seleccionado...
                    if (selectedUnit != null && !selectedUnits.Contains(selectedUnit))
                    {
                        selectedUnit.selectUnit();
                        selectedUnits.Add(selectedUnit);
                    }
                }
                else
                {
                    if (selectedUnit != null)
                    {
                        //Si no apreté shift y cliquee sobre una unidad válida...
                        //Deselecciono todas las unidades y limpio la lista.
                        foreach (var unit in selectedUnits)
                            unit.DeselectUnit();
                        selectedUnits.Clear();
                        //Añado la unidad seleccionada, y lo marco como seleccionado.
                        selectedUnits.Add(selectedUnit);
                        selectedUnit.selectUnit();
                    }
                    else
                    {
                        //Si no hay unidad Válida... Deselecciono todas las unidades y las marco como no seleccionadas.
                        foreach (var unit in selectedUnits)
                            unit.DeselectUnit();
                        selectedUnits.Clear();
                    }
                }

                

            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
			if(eventData.button == PointerEventData.InputButton.Left)
			{
				//Inicializar la imagen de selección y su rect.
				selectionBoxImage.gameObject.SetActive(true);
				selectionBoxRect = new Rect();
				//guardamos la posicion inicial del evento.
				selectionStartPos = eventData.position;
			}
        }

        public void OnDrag(PointerEventData eventData)
        {
			if(eventData.button == PointerEventData.InputButton.Left)
			{
				//Eventdata.position guarda la posicion de nuestro puntero(Mouse) En el momento que sucede el evento.
            if (eventData.position.x < selectionStartPos.x)
            {
                selectionBoxRect.xMin = eventData.position.x;
                selectionBoxRect.xMax = selectionStartPos.x;
            }
            else
            {
                selectionBoxRect.xMin = selectionStartPos.x;
                selectionBoxRect.xMax = eventData.position.x;
            }

            if (eventData.position.y < selectionStartPos.y)
            {
                selectionBoxRect.yMin = eventData.position.y;
                selectionBoxRect.yMax = selectionStartPos.y;
            }
            else
            {
                selectionBoxRect.yMin = selectionStartPos.y;
                selectionBoxRect.yMax = eventData.position.y;
            }

            //Ahora le igualo los valores de mi custom rect a mi imagen.
            selectionBoxImage.rectTransform.offsetMin = selectionBoxRect.min;
            selectionBoxImage.rectTransform.offsetMax = selectionBoxRect.max;
			}
        }

        public void OnEndDrag(PointerEventData eventData)
        {
			if(eventData.button == PointerEventData.InputButton.Left)
			{
				selectionBoxImage.gameObject.SetActive(false);
				OnBoxSelection();
			}
        }
    }
}
