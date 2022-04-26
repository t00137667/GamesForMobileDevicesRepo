using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneGenerator : MonoBehaviour
{
    Renderer planeRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = transform.gameObject;
        parent.AddComponent<TouchDeterminer>();
        parent.AddComponent<TouchManager>();

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, -5.6f, 6.4f);
        plane.transform.Rotate(-13.961f,0,0);
        plane.transform.localScale = new Vector3(10, 10, 10);
        planeRenderer = plane.GetComponent<Renderer>();
        planeRenderer.material.color = Color.green;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<CubeControl>();

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(0, 2, 0);
        sphere.AddComponent<SphereControl>();

        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsule.transform.position = new Vector3(1.5f, 0, 0);
        capsule.AddComponent<CapsuleControl>();

        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
