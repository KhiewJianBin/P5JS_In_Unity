using UnityEngine;
using static P5JSExtension;

public class P5JSBehaviour : MonoBehaviour
{
    void Start()
    {
        setup();
    }

    void OnGUI()
    {
        resetMatrix();

        draw();
    }

    protected virtual void setup() { }
    protected virtual void draw() { }
}
