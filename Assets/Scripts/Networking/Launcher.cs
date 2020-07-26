using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Networking Properties
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    string GameVersion = "1";

    bool isConnecting;
    #endregion

    #region UI Properties

    [SerializeField]
    private GameObject loginForm;

    [SerializeField]
    private GameObject progressLabel;


    #endregion

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        loginForm.SetActive(true);
    }

    public void Connect()
    {
        progressLabel.SetActive(true);
        loginForm.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
        }
    }

    #region MonoBehaviourPunCallBacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;

        progressLabel.SetActive(false);
        loginForm.SetActive(true);

        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We load the 'Room for 1' ");

        PhotonNetwork.LoadLevel("Room for 1");

        if (PhotonNetwork.InRoom)
        {
            print($"Sala chamada {PhotonNetwork.CurrentRoom.Name}");
        }
    }

    #endregion
}
