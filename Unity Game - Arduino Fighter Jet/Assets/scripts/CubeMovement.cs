using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public SampleUserPolling_ReadWrite reader;
    public OverWorldManager overWorldManager;

    public float speed = 3f;
    public float rotationSpeed = 10f;
    public float tiltSpeed = 10f;
    public float restitutionSpeed = 10f;

    float elapsedTime = 0f;

    public Transform player;
    private Camera cam;

    private Vector3 dir, arduinoDir;

    private void Awake()
    {

        cam = GetComponentInChildren<Camera>();
        cam.transform.LookAt(player.transform);
    }

    private void FixedUpdate()
    {
        UpdateRotation();
        UpdatePosition();
    }

    void UpdatePosition()
    {
        player.transform.position += player.transform.forward * speed;

        cam.transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + 3f,
            player.transform.position.z -15f);
    }
    public Vector3 GetPos()
    {
        return player.position;
    }

    void UpdateRotation()
    {
        UpdateDirectionalAngle(reader.GetMessage());

        if (dir != null)
        {
            UpdateRotation2(arduinoDir.x, arduinoDir.y, arduinoDir.z);
        }
    }
    public void UpdateRotation(float x, float y, float z)
    {
        float givenX, givenY, givenZ;

        if (x > 0)
        {
            givenX = RefactorValue(x);
        }
        else
        {
            givenX = -RefactorValue(x);
        }
        if (y > 0)
        {
            givenY = RefactorValue(y);
            givenZ = -RefactorValue(y);
        }
        else
        {
            givenY = -RefactorValue(y);
            givenZ = RefactorValue(y);
        }
        givenX = -givenX * 80f;
        givenY = givenY * 80f;
        givenZ = givenZ * 45f;

        elapsedTime += Time.fixedDeltaTime;

        Vector3 targetRot = new Vector3(givenX, givenY, givenZ);
        Debug.Log(targetRot);
        targetRot = Vector3.Lerp(player.transform.eulerAngles, targetRot, elapsedTime);

        player.rotation = Quaternion.Euler(targetRot);
    }
    public void UpdateRotation2(float x, float y, float z)
    {
        float xIncrement, yIncrement, zIncrement;
        float xRestitution, yRestitution, zRestitution;

        //rotation controll
        if (x > 0)
        {
            xIncrement = RefactorValue(x) * Time.fixedDeltaTime * tiltSpeed;
        }
        else
        {
            xIncrement = -RefactorValue(x) * Time.fixedDeltaTime * tiltSpeed;
        }
        if (y > 0)
        {
            yIncrement = RefactorValue(y) * Time.fixedDeltaTime * rotationSpeed;
            zIncrement = -RefactorValue(y) * Time.fixedDeltaTime * rotationSpeed;
        }
        else
        {
            yIncrement = -RefactorValue(y) * Time.fixedDeltaTime * rotationSpeed;
            zIncrement = RefactorValue(y) * Time.fixedDeltaTime * rotationSpeed;
        }

        //restitution
        if (dir.x < 0)
        {
            xRestitution = RefactorValue(z) * Time.fixedDeltaTime * restitutionSpeed;
            Debug.Log(dir.x +  "x > 0: " + xRestitution);
        }
        else
        {
            xRestitution = -RefactorValue(z) * Time.fixedDeltaTime * restitutionSpeed;
            Debug.Log(dir.x + "x < 0: " + xRestitution);
        }
        if (dir.y > 0)
        {
            yRestitution = -RefactorValue(z) * Time.fixedDeltaTime * restitutionSpeed;
        }
        else
        {
            yRestitution = RefactorValue(z) * Time.fixedDeltaTime * restitutionSpeed;
        }
        if (dir.z > 0)
        {
            zRestitution = -RefactorValue(z) * Time.fixedDeltaTime * restitutionSpeed;
        }
        else
        {
            zRestitution = RefactorValue(z) * Time.fixedDeltaTime * restitutionSpeed;
        }

        dir.x += xIncrement + xRestitution; 
        dir.y += yIncrement + yRestitution;
        dir.z += zIncrement + zRestitution;

        dir.x = Mathf.Clamp(dir.x, -80f, 80f);
        dir.y = Mathf.Clamp(dir.y, -80f, 80f);
        dir.z = Mathf.Clamp(dir.z, -45f, 45f);

        player.rotation = Quaternion.Euler(dir);
    }
    public void UpdateRotation3(Vector3 dir)
    {
        Vector3 remappedDir = new Vector3(
            dir.y, -dir.x, dir.z);
        player.rotation = Quaternion.LookRotation(remappedDir);

        Vector3 pos = gameObject.transform.position;
        pos.x += remappedDir.x * Time.deltaTime * speed;
        pos.y += remappedDir.y * Time.deltaTime * speed;
        pos.z = gameObject.transform.position.z;
        gameObject.transform.position = pos;
    }
    float RefactorValue(float val, float clampMin = -1f, float clampMax = 1f)
    {
        float refactored = 1f / (-Mathf.Pow(val, 2) + 1.5f) - (2f / 3f);
        refactored = Mathf.Clamp(refactored, clampMin, clampMax);
        return refactored;
    }
    public void UpdateDirectionalAngle(string mssg)
    {
        if (mssg != null)
        {
            string[] coordinates = mssg.Split(';');

            //check if the snippet consists of more than one element
            if (coordinates.Length == 3)
            {
                for (int i = 0; i < coordinates.Length; i++)
                {
                    //I have no idea why I needed to do this, but it works
                    coordinates[i] = coordinates[i].Replace('.', '.');
                }
                //create a new vector 3 with the accelerometer data
                arduinoDir =
                    new Vector3(
                        float.Parse(coordinates[0]),
                        float.Parse(coordinates[1]),
                        float.Parse(coordinates[2]));
                //coordinates have a value between -1.00, 1.00
            }
        }
    }
}
    