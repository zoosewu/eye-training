using System.Collections;
using System.Collections.Generic;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using R3;

public class NewBehaviourScript : MonoBehaviour
{
  [SerializeField] private Vector3 position = new Vector3(0, 1.36f, 0);
  [SerializeField] private Vector3 minPosition = new Vector3(0.15f, 0, 0), maxPosition = new Vector3(1, 0, 0);
  [SerializeField] private float moveSpeed = 0.2f;
  [SerializeField] private float stopTime = 1f;
  [SerializeField] private AnimationCurve curve;
  private float nowTime = 0;
  void Start()
  {
    Observable.EveryUpdate().Subscribe(_ =>
    {
    });
  }
  void Update()
  {
    nowTime += Time.deltaTime;
    float x = Mathf.PingPong(Time.time * moveSpeed, 1);
    x = curve.Evaluate(x);
    transform.position = Vector3.Lerp(minPosition, maxPosition, x) + position;
  }
  public void UpdateMinimumPosition(Vector3 min) => minPosition = min;
  public void UpdateMaximumPosition(Vector3 max) => maxPosition = max;
  public void UpdateMoveSpeed(float speed)
  => Observable.Return(speed)
    .Do(speed => moveSpeed = speed)
    .Select(_ => stopTime * moveSpeed)
    .Subscribe(UpdateCurveKey);
  public void UpdateStopTime(float time)
  => Observable.Return(time)
    .Do(time => stopTime = time)
    .Select(_ => stopTime * moveSpeed)
    .Subscribe(UpdateCurveKey);
  private void UpdateCurveKey(float keyPosition)
  {
    curve.MoveKey(0, new Keyframe(keyPosition, 0));
    curve.MoveKey(1, new Keyframe(1 - keyPosition, 1));
  }
}
