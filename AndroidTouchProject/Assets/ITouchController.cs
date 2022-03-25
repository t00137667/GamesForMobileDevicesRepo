using System.Collections.Generic;
using UnityEngine;

public interface ITouchController
{
    void tap(Vector2 position);

    void drag(List<Vector2> current_position, Touch lastTouch);

    void pinch(Vector2 position1, Vector2 position2, float relative_distance);

    void rotate(float rotation);
}
