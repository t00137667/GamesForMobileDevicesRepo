using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    void Hold(Vector2 vector2);
    bool IsHoldable();
    void Scale(float scaleValue);
    bool IsScaleable();
    void Drag(Vector2 position);
    bool IsDraggable();
    void Tap();
    bool IsTappable();
    void Rotate(float rotation);
    bool IsRotatable();
    void Deselect();
    void ResetPosition();
}
