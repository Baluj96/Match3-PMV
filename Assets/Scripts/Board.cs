using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width, height;
    public GameObject tileObject;
    public float camSizeOffset, camVerticalOffset;

    void Start()
    {
        SetUpBoard();
        PositionCamera();
    }

    void SetUpBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity, gameObject.transform);
            }
        }
    }

    void PositionCamera()
    {
        float newPosX = (float)width / 2f - 0.5f;
        float newPosY = (float)height / 2f - 0.5f + camVerticalOffset;
        Camera.main.transform.position = new Vector3(newPosX, newPosY, -10f);

        float horizontal = width + 1;
        float vertical = height / 2 + 1;
        Camera.main.orthographicSize = horizontal > vertical ? horizontal + camSizeOffset : vertical;
    }

    void Update()
    {
        
    }
}
