using UnityEngine;
using NaughtyAttributes;
using System;

public class Piece : MonoBehaviour
{
    private Quaternion BLACK_TOP = new Quaternion(1,0,0,0);
    private const float MIN_HEIGHT = 1.1f;
    public const float MAX_PLACE_HEIGHT = 12f;
    private const float MAX_FLIP_HEIGHT = 1.6f;
    public const float FLIP_DURATION = 0.75f;
    public const float PLACE_DURATION = 0.75f;
    public const float REMOVE_DURATION = 0.75f;
    private bool bIsPlacing = false;
    private bool bIsFlipping = false;
    private bool bIsRemoving = false;
    private float tmpTime = 0f;
    private bool _bIsWhite = true;
    public bool bIsWhite
    {
        get
        {
            return _bIsWhite;
        }
        set
        {
            if (value)
                transform.rotation = Quaternion.identity;
            else
                transform.rotation = BLACK_TOP;
            _bIsWhite = value;
        }
    }
    public Action OnPlaceFinished;
    public Action OnFlipFinished;
    public Action OnRemovedFinished;

    private void Update()
    {
        if (bIsPlacing)
        {
            tmpTime += Time.deltaTime;

            // update height
            float t = Mathf.Clamp(tmpTime/PLACE_DURATION, 0, 1);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(MAX_PLACE_HEIGHT, MIN_HEIGHT, t),transform.position.z);

            if (tmpTime > PLACE_DURATION)
            {
                bIsPlacing = false;
                tmpTime = 0f;
                if (OnPlaceFinished != null)
                    OnPlaceFinished();
            }
        }

        if (bIsFlipping)
        {
            tmpTime += Time.deltaTime;

            // update rotation
            float t = Mathf.Clamp(tmpTime/FLIP_DURATION, 0, 1);
            if (bIsWhite)
                transform.rotation = Quaternion.Slerp(BLACK_TOP, Quaternion.identity, t);
            else
                transform.rotation = Quaternion.Slerp(Quaternion.identity, BLACK_TOP, t);

            // update height to clear the board
            if (tmpTime < FLIP_DURATION / 2f)
            {
                t *= 2f;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(MIN_HEIGHT, MAX_FLIP_HEIGHT, t),transform.position.z);
            }
            else
            {
                t = (t - 0.5f) * 2f;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(MAX_FLIP_HEIGHT, MIN_HEIGHT, t),transform.position.z);
            }

            if (tmpTime > FLIP_DURATION)
            {
                bIsFlipping = false;
                tmpTime = 0f;
                if (OnFlipFinished != null)
                    OnFlipFinished();
            }
        }

        if (bIsRemoving)
        {
            tmpTime += Time.deltaTime;

            // update height
            float t = Mathf.Clamp(tmpTime/PLACE_DURATION, 0, 1);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(MIN_HEIGHT, MAX_PLACE_HEIGHT, t),transform.position.z);

            if (tmpTime > PLACE_DURATION)
            {
                bIsRemoving = false;
                tmpTime = 0f;
                Destory();
            }
        }
    }

    [Button]
    public void Place()
    {
        bIsPlacing = true;
    }

    [Button]
    public void Flip()
    {
        bIsFlipping = true;
        bIsWhite = !bIsWhite;
    }

    [Button]
    public void Remove()
    {
        bIsRemoving = true;
    }

    [Button]
    public void Destory()
    {
        Destroy(gameObject);
    }
}
