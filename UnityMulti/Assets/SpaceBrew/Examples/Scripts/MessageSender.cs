using UnityEngine;

public class MessageSender : MonoBehaviour {

    public SpacebrewEvents spacebrewClientEvents;
    public string pubNameMove;
    public string pubNameShoot;


    public void SendMove(string message) {
        spacebrewClientEvents.SendString(pubNameMove, message);
    }

    public void SendShoot(string message) {
        spacebrewClientEvents.SendString(pubNameShoot, message);
    }

}
