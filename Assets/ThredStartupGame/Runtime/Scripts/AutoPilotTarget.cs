using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilotTarget : MonoBehaviour
{
    [SerializeField] CharacterController_2D player;
    [SerializeField] GameObject target;
    [SerializeField] bool matchTarget;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        MatchTarget();
    }

    public void MatchTarget()
    {
        if (target != null && matchTarget)
        {
            if (target.transform.hasChanged)
            {
                gameObject.transform.position = target.transform.position;
                if (gameObject.TryGetComponent<RectTransform>(out RectTransform _rect))
                {
                    if (target.TryGetComponent<RectTransform>(out RectTransform _rectTarget))
                    {
                        _rect.rect.Set(_rectTarget.rect.x, _rectTarget.rect.y, _rectTarget.rect.width, _rectTarget.rect.height);
                    }
                }
            }
        }
    }

    public void SetPlayer(CharacterController_2D _player)
    {
        if (_player != null) { player = _player; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            if (other.TryGetComponent<CharacterController_2D>(out CharacterController_2D _player))
            {
                if (player == _player)
                    player.AutoPilot_CheckTargetMatch(gameObject);
            }
        }
    }

}
