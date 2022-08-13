using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Min(0)]
    public float density = 1;
    public List<GameObject> prefabs;
    public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
    public bool randomRotation = true;
    public Bounds scales = new Bounds(Vector3.one * 2, Vector3.one * 0.5f);
    public Vector3 direction = Vector3.back;
    public float minSpeed = 1;
    public float maxSpeed = 1;

    private Coroutine coroutine;

    void OnDrawGizmos()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = new Color(0.5f, 1.0f, 0.5f, 0.5f);
        Gizmos.DrawCube(bounds.center, bounds.size);
    }

    void OnEnable()
    {
        coroutine = StartCoroutine(Generate());
    }

    void OnDisable()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator Generate()
    {
        while (true)
        {
            int id = Random.Range(0, prefabs.Count);
            GameObject obj = Instantiate(prefabs[id]);
            obj.transform.parent = this.transform;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            obj.transform.localPosition = new Vector3(x, y, z);
            obj.transform.rotation = Random.rotation;

            float scaleX = Random.Range(scales.min.x, scales.max.x);
            float scaleY = Random.Range(scales.min.y, scales.max.y);
            float scaleZ = Random.Range(scales.min.z, scales.max.z);
            obj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            float speed = Random.Range(minSpeed, maxSpeed);
            rb.velocity = direction * speed;

            yield return new WaitForSeconds(1f/ density);
        }
    }
}
