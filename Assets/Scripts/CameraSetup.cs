using Cinemachine; // 시네머신 관련 코드
using Photon.Pun; // PUN 관련 코드
using UnityEngine;

// 시네머신 카메라가 로컬 플레이어를 추적하도록 설정
// MonoBehaviourPun : MonoBehaviour + photonview
// 이 스크립트(photon view script가 추가됨)가 추가된 gameobject가 로컬인지 검사 가능
public class CameraSetup : MonoBehaviourPun {
    void Start() {

        // 만약 지금 보는 플레이어가 로컬이라면
        if (photonView.IsMine)
        {
            // 씬에서 가상 카메라 컴포넌트를 찾아 caching
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            // 시네머신 카메라가 자신을 따라다니고 자신을 보도록 설정
            // update()가 없더라도 transform 자체를 박아버려서 ㄱㅊ
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}