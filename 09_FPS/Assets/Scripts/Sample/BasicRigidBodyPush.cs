using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
	public LayerMask pushLayers;//밀릴 대상의 레이어
	public bool canPush;//충돌시 밀 수 있는지
	[Range(0.5f, 5f)] public float strength = 1.1f;

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (canPush) PushRigidBodies(hit);
	}


	/// <summary>
	/// 캐릭터컨트롤러가 이동하다가 충돌이 발생하면 실행되는 이벤트 함수 OnCollision, OnTrigger 랑 비슷하나
	/// rigidBody 대신 케릭터컨트롤러를 이용하면 이 함수를 사용한다.
	/// </summary>
	/// <param name="hit"></param>
	private void PushRigidBodies(ControllerColliderHit hit)
	{
		// https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

		// make sure we hit a non kinematic rigidbody
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;// rigidbody가 없거나 kinematic이면 리턴

        // make sure we only push desired layer(s)
        //설정한 레이어가 아니면 종료
        // 1 =				0000 0000 0000 0000 0000 0000 0000 0001
        // 1 << 10 =		0000 0000 0000 0000 0000 0100 0000 0000
        //pushLayer.value = 0000 0000 0000 0000 0000 0100 0000 0000
		// 둘을 & 해서 1이나오면 같다. 1이 아니면 서로 다른 레이어이기때문에 return
        var bodyLayerMask = 1 << body.gameObject.layer;// ex)layer 가 11번째이면 10이 기록되어있다.
		if ((bodyLayerMask & pushLayers.value) == 0) return;

		// We dont want to push objects below us
		//너무 아랫쪽으로 밀고있으면 리턴
		if (hit.moveDirection.y < -0.3f) return;

		// Calculate push direction from move direction, horizontal motion only
		// x, z 방향으로 밀어낼 값 계산
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

		// Apply the push and take strength into account
		//실제 적용
		body.AddForce(pushDir * strength, ForceMode.Impulse);


	}
}