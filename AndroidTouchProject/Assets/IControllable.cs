using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    bool selectToggle(bool selected);

    void drag(List<Vector2> positions);

    void tap(Vector2 position);

    void scale(float scaleFactor);

    void rotate(float rotation);

    void updateScale();

    void resetPosition();
}
