using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIFocusController : MonoBehaviour {
    
    public enum SelectedChangeMethod {
        ByTabKey,
        ByArrowKey,
        ByUpDownKeyLikeTabKey,
        ByRightLeftKeyLikeTabKey, 
        ByReverseUpDownKeyLikeTabKey,
        ByReverseRightLeftKeyLikeTabKey
    }
    public SelectedChangeMethod selectedchangemethod = SelectedChangeMethod.ByTabKey;
    
    
    List<Selectable> SelectableList = new List<Selectable>();
    EventSystem eventsystem;


    void Awake() {
        foreach (var ChildObj in GetComponentsInChildren<Selectable>()) {
            SelectableList.Add(ChildObj);
            
        }
        eventsystem = EventSystem.current;
    }
    
    void ChangeSelectedByTabKey(bool Tab,bool S_Tab) {
        if (Tab || S_Tab){
            int SelectableActiveSum = SelectableList.Count(s => s != null && s.IsActive() );
            if (SelectableActiveSum > 1) {
                GameObject goselected = eventsystem.currentSelectedGameObject;
                if (goselected == null) return;
                Selectable selected = goselected.GetComponent<Selectable>();
                int selectedindex = SelectableList.FindIndex(s => s.Equals(selected));
                if (S_Tab) {
                    // すべてフォーカス解除
                    foreach (var selectable in SelectableList) {
                        if (selectable != null) selectable.OnDeselect(null);
                    }
                    Debug.Log("S_Tab: " + selectedindex);
                    SelectableList[0].Select();
                } else if(Tab){
                    for (int i = 0, j = selectedindex + 1; i < SelectableActiveSum; i++, j++) {
                        if (j >= SelectableList.Count - 1) j = 0;
                        if (SelectableList[j] != null && SelectableList[j].IsActive()) {
                            SelectableList[j].Select(); 
                            selected.OnDeselect(null);
                            Debug.Log("Tab: " + j);
                            return;
                        }
                    }
                }
            }
        }
    }
    void ChangeSelectedByArrowKey() {
        
        GameObject goselected = eventsystem.currentSelectedGameObject;
        if (goselected == null) return;
        Selectable selected = goselected.GetComponent<Selectable>();
        Selectable nextselectable = null;
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            nextselectable = selected.FindSelectableOnDown();
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            nextselectable = selected.FindSelectableOnUp();
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            nextselectable = selected.FindSelectableOnRight();
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            nextselectable = selected.FindSelectableOnLeft();
        } 
        if (nextselectable) nextselectable.Select();

    }

    void Update() {
        switch (selectedchangemethod) { 
            case SelectedChangeMethod.ByTabKey:
                ChangeSelectedByTabKey(
                    Input.GetKeyDown(KeyCode.Tab) && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)),
                    Input.GetKeyDown(KeyCode.Tab) &&  (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));break;
            case SelectedChangeMethod.ByUpDownKeyLikeTabKey:
                 ChangeSelectedByTabKey(Input.GetKeyDown(KeyCode.DownArrow),Input.GetKeyDown(KeyCode.UpArrow));break;
            case SelectedChangeMethod.ByRightLeftKeyLikeTabKey:
                ChangeSelectedByTabKey(Input.GetKeyDown(KeyCode.RightArrow),Input.GetKeyDown(KeyCode.LeftArrow));break;
            case SelectedChangeMethod.ByReverseUpDownKeyLikeTabKey:
                ChangeSelectedByTabKey(Input.GetKeyDown(KeyCode.UpArrow),Input.GetKeyDown(KeyCode.DownArrow)); break;
            case SelectedChangeMethod.ByReverseRightLeftKeyLikeTabKey:
                ChangeSelectedByTabKey( Input.GetKeyDown(KeyCode.LeftArrow),Input.GetKeyDown(KeyCode.RightArrow)); break;
            case SelectedChangeMethod.ByArrowKey:
                ChangeSelectedByArrowKey();break;
        }
    }
}