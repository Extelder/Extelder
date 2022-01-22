using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _camera : MonoBehaviour
{
    private Transform _cosmonaut;
    public float _followSpeed;
    [SerializeField] private Transform _rocket;

    private void Start()
    {
        StartCoroutine(AddSpeed());
        if (!_cosmonaut)
        {
            _cosmonaut = Player._singleton.transform;
        }
    }
    void FixedUpdate()
    {
        if(_cosmonaut != null)
        transform.position = new Vector3(transform.position.x + _followSpeed * Time.fixedDeltaTime, transform.position.y, transform.position.z);
        else
        {
            transform.position = new Vector3(_rocket.position.x, _rocket.position.y, _rocket.position.z);
        }

    }
    public void HandlerDestroyCosmonaut()
    {
        _cosmonaut = null;
    }
    private IEnumerator AddSpeed()
    {
        while (true)
        {
            _followSpeed += 0.3f;
            yield return new WaitForSeconds(2f);
        }
    }

}
