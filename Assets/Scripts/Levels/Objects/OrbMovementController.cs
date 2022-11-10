using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public float initialImpulse;
    public float groundDistanceToStartOscilation;
    public float oscilationAmplitude;
    public float oscilationFrequency;

    private float yOriginPoint;
    private float time = 0;

    public bool hasStartedOscilating = false;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.AddForce(new Vector2(0, initialImpulse), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasStartedOscilating)
        {
            hasStartedOscilating = GroundDetection(groundDistanceToStartOscilation);
        }
        else
        {
            OrbOscilation();
            time += Time.fixedDeltaTime;
        }
    }

    bool GroundDetection(float distance) //Para detectar hasta una distancia igual a distance
    {
        bool detectGround = false; //Inicialmente la variable a devolver es false

        Vector2 endPos = transform.position - transform.up * distance;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, endPos, 1 << LayerMask.NameToLayer("Ground")); //Para que el Raycast detecte las capas Blocks y Solids

        if (hit.collider != null) //Si detecta algo, la variable a devolver se vuelve true
        {
            detectGround = true;
            yOriginPoint = transform.position.y;
            _rigidbody2D.gravityScale = 0f;
        }

        Debug.DrawRay(transform.position, distance * -transform.up, Color.blue);

        return detectGround;
    }

    void OrbOscilation()
    {
        transform.position = new Vector2(transform.position.x, yOriginPoint - oscilationAmplitude * Mathf.Sin((2 * Mathf.PI / oscilationFrequency) * time));
    }
}
