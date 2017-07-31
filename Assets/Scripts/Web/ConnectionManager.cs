using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

public static class ConnectionManager {

    private static Socket socket;

    public static Socket Socket {
        get { return socket; }
        set { socket = value; }
    }


    public static void Connect() {
        socket = IO.Socket("http://192.168.99.100:5000/");

        socket.On(Socket.EVENT_CONNECT, () => {
            socket.Emit("hi");
        });
    }

    public static void Disconnect() {
        socket.Disconnect();
    }
}
