using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    bool selectToggle();

    void drag(List<Vector2> positions);
}
