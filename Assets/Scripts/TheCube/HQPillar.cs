using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProBuilder2;

public class HQPillar : MonoBehaviour
{
    [HideInInspector]
    public Transform wallsParent;

    public CubeManager cubeManager;
    public Transform playerTransform;
    public HQPillar upperPillard = null;
    public HQPillar lowerPillard = null;
    public HQPillar leftPillar = null;
    public HQPillar rightPillar = null;

    [HideInInspector]
    public GameObject wallCreatedLower = null;
    [HideInInspector]
    public GameObject wallCreatedUpper = null;
    [HideInInspector]
    public GameObject wallCreatedLeft = null;
    [HideInInspector]
    public GameObject wallCreatedRight = null;

    public GameObject[] columns;
    public Color[] save;
    private float timerCheckCircled;

    [HideInInspector]
    public bool isCreativeUpperWall;
    [HideInInspector]
    public bool isCreatingLowerWall;
    [HideInInspector]
    public bool isCreatingRightWall;
    [HideInInspector]
    public bool isCreatingLeftWall;
    [HideInInspector]
    private bool isGradientColor;

    private IEnumerator gradientConstructLower;
    private IEnumerator gradientConstructUpper;
    private IEnumerator gradientConstructLeft;
    private IEnumerator gradientConstructRight;

    private void Start()
    {
        timerCheckCircled = 0.0f;
        gradientConstructLower = GradientAndConstructWall(this, lowerPillard, IEnumeratorCreateWallLower());
        gradientConstructUpper = GradientAndConstructWall(this, upperPillard, IEnumeratorCreateWallUpper());
        gradientConstructLeft = GradientAndConstructWall(this, leftPillar, IEnumeratorCreateWallLeft());
        gradientConstructRight = GradientAndConstructWall(this, rightPillar, IEnumeratorCreateWallRight());

        isGradientColor = false;
        isCreativeUpperWall = false;
        isCreatingLowerWall = false;
        isCreatingRightWall = false;
        isCreatingLeftWall = false;

        save = new Color[columns.Length];
        save[0] = columns[0].GetComponent<Renderer>().materials[0].color;
        save[1] = columns[0].GetComponent<Renderer>().materials[1].color;
    }

    public void Deactivate()
    {
        StopAllCoroutines();
        /*StopCoroutine(gradientConstructLower);
        StopCoroutine(gradientConstructUpper);
        StopCoroutine(gradientConstructLeft);
        StopCoroutine(gradientConstructRight);*/

        if (wallCreatedLeft != null)
        {
            Destroy(wallCreatedLeft);
        }

        if(wallCreatedRight != null)
        {
            Destroy(wallCreatedRight);
        }

        if(wallCreatedUpper != null)
        {
            Destroy(wallCreatedUpper);
        }

        if(wallCreatedLower != null)
        {
            Destroy(wallCreatedLower);
        }
    }

    private bool WallRightConstructed()
    {
        return wallCreatedRight != null || rightPillar == null || rightPillar.wallCreatedLeft != null;
    }

    private bool WallLeftConstructed()
    {
        return wallCreatedLeft != null || leftPillar == null || leftPillar.wallCreatedRight != null;
    }

    private bool WallLowerConstructed()
    {
        return wallCreatedLower != null || lowerPillard == null || lowerPillard.wallCreatedUpper != null;
    }

    private bool WallUpperConstructed()
    {
        return wallCreatedUpper != null || upperPillard == null || upperPillard.wallCreatedLower != null;
    }

    private bool IsCerclingPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if(distance < 5.0f)
        {
            Vector3 direction = (transform.position - playerTransform.position);

            // En BAS DROITE
            if (direction.x < 0 && direction.z > 0)
            {
                bool cercled = true;
                // CHECK IF WALL RIGHT IS CONSTRUCT
                cercled &= WallRightConstructed();

                // CHECK IF WALL LOWER IS CONSTRUCT
                cercled &= WallLowerConstructed();

                // CHECK IF WALL LOWER OF RIGHT PILLAR IS CONSTRUCT
                //cercled &= rightPillar == null || rightPillar.WallLowerConstructed();
                cercled &= rightPillar != null && rightPillar.WallLowerConstructed();

                // CHECK IF WALL RIGHT OF LOWER PILLAR IS CONSTRUCT
                //cercled &= lowerPillard == null || lowerPillard.WallRightConstructed();
                cercled &= lowerPillard != null && lowerPillard.WallRightConstructed();

                return cercled;
            }

            // EN BAS GAUCHE
            if (direction.x > 0 && direction.z > 0)
            {
                bool cercled = true;
                // CHECK IF WALL LEFT IS CONSTRUCT 
                cercled &= WallLeftConstructed();

                // CHECK IF WALL LOWER IS CONTRUCT
                cercled &= WallLowerConstructed();

                // CHECK IF WALL LOWER OF LEFT PILLAR IS CONSTRUCT
                //cercled &= leftPillar == null || leftPillar.WallLowerConstructed();
                cercled &= leftPillar != null && leftPillar.WallLowerConstructed();

                // CHECK IF WALL LEFT OF LOWER PILLAR IS CONSTRUCT
                //cercled &= lowerPillard == null || lowerPillard.WallLeftConstructed();
                cercled &= lowerPillard != null && lowerPillard.WallLeftConstructed();

                return cercled;
            }

            // EN HAUT DROITE
            if (direction.x < 0 && direction.z < 0)
            {
                bool cercled = true;
                // CHECK IF WALL RIGHT IS CONSTRUCT
                cercled &= WallRightConstructed();

                // CHECK IF WALL UPPER IS CONSTRUCT
                cercled &= WallUpperConstructed();

                // CHECK IF WALL RIGHT OF UPPER PILLAR IS CONSTRUCT
                //cercled &= upperPillard == null || upperPillard.WallRightConstructed();
                cercled &= upperPillard != null && upperPillard.WallRightConstructed();

                // CHECK IF WALL UPPER OF RIGHT PILLAR IS CONSTRUCT
                //cercled &= rightPillar == null || rightPillar.WallUpperConstructed();
                cercled &= rightPillar != null && rightPillar.WallUpperConstructed();

                return cercled;
            }

            // EN HAUT GAUCHE
            if (direction.x > 0 && direction.z < 0)
            {
                bool cercled = true;
                // CHECK IF WALL LEFT IS CONSTRUCT
                cercled &= WallLeftConstructed();

                // CHECK IF WALL UPPER IS CONSTRUCT
                cercled &= WallUpperConstructed();

                // CHECK IF WALL LEFT OF UPPER PILLAR IS CONSTRUCT
                //cercled &= upperPillard == null || upperPillard.WallLeftConstructed();
                cercled &= upperPillard != null && upperPillard.WallLeftConstructed();

                // CHECK IF WALL UPPER OF LEFT PILLAR IS CONSTRUCT
                //cercled &= leftPillar == null || leftPillar.WallUpperConstructed();
                cercled &= leftPillar != null && leftPillar.WallUpperConstructed();

                return cercled;
            }

            return false;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance < 5.0f)
        {
            Vector3 direction = (transform.position - playerTransform.position);

            // En BAS DROITE
            if (direction.x < 0 && direction.z > 0)
            {
                if(!isCreatingLowerWall)
                {
                    CreateWallLower();
                }
                if(!isCreatingRightWall)
                {
                    CreateWallRight();
                }
            }

            // EN BAS GAUCHE
            if (direction.x > 0 && direction.z > 0 )
            {
                if(!isCreatingLowerWall)
                {
                    CreateWallLower();
                }
                if(!isCreatingLeftWall)
                {
                    CreateWallLeft();
                }
            }

            // EN HAUT DROITE
            if (direction.x < 0 && direction.z < 0)
            {
                if(!isCreativeUpperWall)
                {
                    CreateWallUpper();
                }
                if(!isCreatingRightWall)
                {
                    CreateWallRight();
                }
            }

            // EN HAUT GAUCHE
            if (direction.x > 0 && direction.z < 0)
            {
                if (!isCreativeUpperWall)
                {
                    CreateWallUpper();
                }
                if (!isCreatingLeftWall)
                {
                    CreateWallLeft();
                }
            }
        }
        else
        {
            if(wallCreatedLeft != null)
            {
                Destroy(wallCreatedLeft);
                wallCreatedLeft = null;
            }
            if(wallCreatedLower != null)
            {
                Destroy(wallCreatedLower);
                wallCreatedLower = null;
            }
            if (wallCreatedRight != null)
            {
                Destroy(wallCreatedRight);
                wallCreatedRight = null;
            }
            if (wallCreatedUpper != null)
            {
                Destroy(wallCreatedUpper);
                wallCreatedUpper = null;
            }
        }

        if (timerCheckCircled > 3.0f)
        {
            timerCheckCircled = 0;
            if (IsCerclingPlayer())
            {
                cubeManager.Lose(CubeManager.LoseReason.SYSTEM);
                Debug.Log("ENCERCLE");
            }
        }
        timerCheckCircled += Time.deltaTime;
    }

    private IEnumerator ColorGradient(float duration, Material material, Color start, Color end)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            
            material.color = Color.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        material.color = start;
    }
    private void ChangeColorWarning(float duration)
    {
        foreach (GameObject column in columns)
        {
            Material[] materials = column.GetComponent<Renderer>().materials;
            int i = 0;
            foreach (Material material in materials)
            {
                Color from;
                Color to;
                if (i == 0)
                {
                    from = save[0];
                    to = new Color(0.850f, 0.150f, 0.150f, 1);
                }
                else
                {
                    from = save[1];
                    to = new Color(0.776f, 0.604f, 0.604f, 1);
                }
                StartCoroutine(ColorGradient(duration, material, from, to));
                i++;
            }

        }
    }
    private IEnumerator GradientAndConstructWall(HQPillar firstPillar, HQPillar secondPillar, IEnumerator toStart)
    {
        float timerGradient = 0.0f;
        float gradientDuration = 2.0f;

        if(!firstPillar.isGradientColor)
        {
            firstPillar.isGradientColor = true;
            firstPillar.ChangeColorWarning(gradientDuration);
        }

        if(!secondPillar.isGradientColor)
        {
            secondPillar.isGradientColor = true;
            secondPillar.ChangeColorWarning(gradientDuration);
        }

        while(timerGradient < gradientDuration)
        {
            timerGradient += Time.deltaTime;
            yield return null;
        }

        firstPillar.isGradientColor = false;
        secondPillar.isGradientColor = false;
        if(Vector3.Distance(transform.position, playerTransform.position) < 5.0f)
        {
            yield return StartCoroutine(toStart);
        }
        else
        {
            isCreatingLeftWall = false;
            isCreatingRightWall = false;
            isCreativeUpperWall = false;
            isCreatingLowerWall = false;
            yield return null;
        }
    }

    public void CreateWallLower()
    {
        if (lowerPillard != null && wallCreatedLower == null && lowerPillard.wallCreatedUpper == null)
        {
            isCreatingLowerWall = true;
            StopCoroutine(gradientConstructLower);
            gradientConstructLower = GradientAndConstructWall(this, lowerPillard, IEnumeratorCreateWallLower());
            StartCoroutine(gradientConstructLower);
        }
    }

    public void CreateWallUpper()
    {
        if (upperPillard != null && wallCreatedUpper == null && upperPillard.wallCreatedLower == null)
        {
            isCreativeUpperWall = true;
            StopCoroutine(gradientConstructUpper);
            gradientConstructUpper = GradientAndConstructWall(this, upperPillard, IEnumeratorCreateWallUpper());
            StartCoroutine(gradientConstructUpper);
        }
    }

    public void CreateWallLeft()
    {
        if (leftPillar != null && wallCreatedLeft == null && leftPillar.wallCreatedRight == null)
        {
            isCreatingLeftWall = true;
            StopCoroutine(gradientConstructLeft);
            gradientConstructLeft = GradientAndConstructWall(this, leftPillar, IEnumeratorCreateWallLeft());
            StartCoroutine(gradientConstructLeft);
        }
    }

    public void CreateWallRight()
    {
        if (rightPillar != null && wallCreatedRight == null && rightPillar.wallCreatedLeft == null)
        {
            isCreatingRightWall = true;
            StopCoroutine(gradientConstructRight);
            gradientConstructRight = GradientAndConstructWall(this, rightPillar, IEnumeratorCreateWallRight());
            StartCoroutine(gradientConstructRight);
        }
    }

    public IEnumerator IEnumeratorCreateWallLower()
    {
        if(lowerPillard != null && wallCreatedLower == null && lowerPillard.wallCreatedUpper == null)
        {
            wallCreatedLower = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Vector3 difference = transform.position - lowerPillard.transform.position;
            Vector3 localScale = GetComponent<Collider>().bounds.size;
            localScale.z = difference.z - localScale.z;
            localScale.x = localScale.x / 2;
            wallCreatedLower.transform.localScale = localScale;

            Vector3 position = transform.position;
            position.z -= localScale.z / 2 + (GetComponent<Collider>().bounds.size.z / 2);
            position.y -= 0.05f;
            wallCreatedLower.transform.position = position;

            wallCreatedLower.transform.SetParent(wallsParent, false);
            wallCreatedLower.AddComponent<BoxCollider>();
            wallCreatedLower.layer = LayerMask.NameToLayer("Unwalkable");
            lowerPillard.wallCreatedUpper = wallCreatedLower;
        }

        isCreatingLowerWall = false;
        yield return null;
    }

    public IEnumerator IEnumeratorCreateWallUpper()
    {
        if(upperPillard != null && wallCreatedUpper == null && upperPillard.wallCreatedLower == null)
        {
            wallCreatedUpper = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Vector3 difference = transform.position - upperPillard.transform.position;
            Vector3 localScale = GetComponent<Collider>().bounds.size;
            localScale.z = localScale.z - difference.z - (localScale.z * 2);
            localScale.x = localScale.x / 2;
            wallCreatedUpper.transform.localScale = localScale;

            Vector3 position = transform.position;
            position.z += localScale.z / 2 + (GetComponent<Collider>().bounds.size.z / 2);
            position.y -= 0.05f;
            wallCreatedUpper.transform.position = position;

            wallCreatedUpper.transform.SetParent(wallsParent, false);
            wallCreatedUpper.AddComponent<BoxCollider>();
            wallCreatedUpper.layer = LayerMask.NameToLayer("Unwalkable");
            upperPillard.wallCreatedLower = wallCreatedUpper;
        }

        isCreativeUpperWall = false;
        yield return null;
    }

    public IEnumerator IEnumeratorCreateWallLeft()
    {
        if(leftPillar != null && wallCreatedLeft == null && leftPillar.wallCreatedRight == null)
        {
            wallCreatedLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Vector3 difference = transform.position - leftPillar.transform.position;
            Vector3 localScale = GetComponent<Collider>().bounds.size;
            localScale.x = difference.x - localScale.x;
            localScale.z = localScale.z / 2;
            wallCreatedLeft.transform.localScale = localScale;

            Vector3 position = transform.position;
            position.x -= localScale.x / 2 + (GetComponent<Collider>().bounds.size.x / 2);
            position.y -= 0.05f;
            wallCreatedLeft.transform.position = position;

            wallCreatedLeft.transform.SetParent(wallsParent, false);
            wallCreatedLeft.AddComponent<BoxCollider>();
            wallCreatedLeft.layer = LayerMask.NameToLayer("Unwalkable");
            leftPillar.wallCreatedRight = wallCreatedLeft;
        }

        isCreatingLeftWall = false;
        yield return null;
    }

    public IEnumerator IEnumeratorCreateWallRight()
    {
        if(rightPillar != null && wallCreatedRight == null && rightPillar.wallCreatedLeft == null)
        {
            wallCreatedRight = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Vector3 difference = transform.position - rightPillar.transform.position;
            Vector3 localScale = GetComponent<Collider>().bounds.size;
            localScale.x = localScale.x - difference.x - (localScale.x * 2);
            localScale.z = localScale.z / 2;
            wallCreatedRight.transform.localScale = localScale;

            Vector3 position = transform.position;
            position.x += localScale.x / 2 + (GetComponent<Collider>().bounds.size.x / 2);
            position.y -= 0.05f;
            wallCreatedRight.transform.position = position;

            wallCreatedRight.transform.SetParent(wallsParent, false);
            wallCreatedRight.AddComponent<BoxCollider>();
            wallCreatedRight.layer = LayerMask.NameToLayer("Unwalkable");
            rightPillar.wallCreatedLeft = wallCreatedRight;
        }

        isCreatingRightWall = false;
        yield return null;
    }
}
