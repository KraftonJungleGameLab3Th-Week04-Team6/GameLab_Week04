using System.Collections;
using UnityEngine;

public class KitchenCamera : MonoBehaviour
{
    Camera _kitchenCamera;
    Coroutine _slicing_Co;
    float _sliceProjectionNum;
    float _defaultProjectionNum;

    private void Start()
    {
        _kitchenCamera = GetComponent<Camera>();
        _sliceProjectionNum = 4.95f;
        _defaultProjectionNum = 5;
    }

    public void SliceMoving()
    {
        if (_slicing_Co == null)
        {
            print("slicemoving");
            _slicing_Co = StartCoroutine(SlicingMoving());
        }
    }

    IEnumerator SlicingMoving()
    {
        while (true)
        {
            _kitchenCamera.orthographicSize = Mathf.Lerp(_kitchenCamera.orthographicSize, _sliceProjectionNum, Time.deltaTime * 25);
            if ((_kitchenCamera.orthographicSize - _sliceProjectionNum) < 0.01f)
            {
                break;
            }
            yield return null;
        }
        while (true)
        {
            _kitchenCamera.orthographicSize = Mathf.Lerp(_kitchenCamera.orthographicSize, _defaultProjectionNum, Time.deltaTime * 10);
            if ((_defaultProjectionNum - _kitchenCamera.orthographicSize) < 0.01f)
            {
                break;
            }
            yield return null;
        }
        _slicing_Co = null;
    }
}
