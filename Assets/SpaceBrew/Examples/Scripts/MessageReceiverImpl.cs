using UnityEngine.UI;

public class MessageReceiverImpl : MessageReceiver {

    public Text text;

    override public void Receive(SpacebrewClient.SpacebrewMessage message) {

        print("RECEIVED MESSAGE");
        print("clientName: " + message.clientName);
        print("name: " + message.name);
        print("type: " + message.type);
        print("value: " + message.value);

        text.text = message.value;
    }

}
