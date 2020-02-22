using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks {
    private string gameVersion = "1"; // 게임 버전

    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 룸 접속 버튼

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start() {
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보로 마스터 서버 접속시도
        PhotonNetwork.ConnectUsingSettings();

        // 접속버튼 상호작용 프로퍼티 비활성화(보이긴 함)
        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속 중입니다..";
    }

    /// <summary>
    /// 마스터 서버 접속 성공시 자동 실행됨 
    /// </summary>
    public override void OnConnectedToMaster() {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버에 접속 성공!";
    }

    /// <summary>
    /// 마스터 서버 접속 실패시 실패 원인을 cause로 들고 와서 자동 실행됨
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause) {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버 연결 오류. " + cause.ToString() + "\n접속 재시도 중..";

        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    // join버튼을 클릭하면 호출되는 메소드
    public void Connect() {
        // 중복 접속 시도 방지
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "빈 룸에 접속 중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "오프라인 : 마스터 서버 연결 오류.\n접속 재시도 중..";

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message) {
        connectionInfoText.text = "빈 방이 없음. 새로운 방 생성 중..";
        // 최대 4명 수용가능한 방을 만듬
        // 리슨 서버 방식으로 동작, 방을 만든 사람이 호스트 겸 서버가 되는 방식
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom() {
        connectionInfoText.text = "방 참가 성공!";

        // scenemanager의 loadscene을 써버리면 이전 씬의 정보를 날리고 로드함
        // 네트워크 정보도 날아가서 loadlevel을 사용
        PhotonNetwork.LoadLevel("Main");
    }
}