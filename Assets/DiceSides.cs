using UnityEngine;

[System.Serializable]
public class DiceSide
{
    public Vector3 center;
    public Vector3 normal;
    public int value;
}

public class DiceSides : MonoBehaviour
{
    [SerializeField] public DiceSide[] sides;

    const float k_exactMatchValue = 0.995f;

    public DiceSide GetDiceSide(int index) => sides[index];

    public Quaternion GetWorldRotationFor(int index)
    {
        Vector3 worldNormalToMatch = transform.TransformDirection(GetDiceSide(index).normal);
        return Quaternion.FromToRotation(worldNormalToMatch, Vector3.up) * transform.rotation;
    }

    public int GetMatch()
    {
        int sideCount = sides.Length;

        Vector3 localVectorToMatch = transform.InverseTransformDirection(Vector3.up);

        DiceSide closestSide = null;
        float closestDot = -1f;

        for (int i = 0; i < sideCount; i++)
        {
            DiceSide side = sides[i];
            float dot = Vector3.Dot(side.normal, localVectorToMatch);

            if (closestSide == null || dot > closestDot)
            {
                closestSide = side;
                closestDot = dot;
            }

            if (dot > k_exactMatchValue)
            {
                return side.value;
            }
        }

        return closestSide?.value ?? -1;
    }
}
