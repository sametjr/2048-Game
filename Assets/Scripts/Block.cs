using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public Vector2 index;
    public int value;
    private TMP_Text _valueText;
    private void OnEnable() {
        _valueText = GetComponentInChildren<TMP_Text>();
        value = 2;
        UpdateTextValue();
    }
    public void UpdateColor()
    {
        // Get the color index by looking at which power of two its value is by using mathf.log
        GetComponent<SpriteRenderer>().color = Constants.Instance.colors[Mathf.RoundToInt(Mathf.Log(value, 2))];
    }

    public void UpdateTextValue()
    {
        _valueText.text = value.ToString();
    }

    public void UpdatePosition()
    {
        Vector2 newPos = new Vector2(Constants.Instance.startPos.x + index.x * Constants.Instance.blockSize,
                                             Constants.Instance.startPos.y - index.y * Constants.Instance.blockSize);

        LeanTween.moveLocal(gameObject, newPos, 0.2f).setEaseOutBack(); 
    }

}
