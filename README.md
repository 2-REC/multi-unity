# UNITY MULTI

## INTRODUCTION

Server-client for Unity multiplayer projects.<br>

It uses the [SpaceBrew](http://docs.spacebrew.cc/)* toolkit for the server management and the communications between it and the clients.<br>
For the interaction with Unity, the project is based on the [spacebrewUnity](https://github.com/Spacebrew/spacebrewUnity) library.<br>

The SpaceBrew toolkit has been integrated in the project, and its execution can be managed by the Unity project directly (using an [embedded Node.js](//github.com/2-REC/unity-nodejs)*).<br>

(*) The "SpaceBrew" and "Unity Node.js" projects are included in the repository as normal files (not as dependencies/submodules). The corresponding versions can be determined from the timestamps of the last respective commits.
In order to use the latest version, the projects should be downloaded separately. Beware however that the compatibility might not be 100% guaranteed.<br>
It is possible to use external SpaceBrew server and Node.js depending on the used prefabs and their configuration (see below in the "Components" section).<br>


## GETTING STARTED
TODO!

Import assets in project.


## COMPONENTS

It is composed of components (prefabs).<br>
...<br>
- 3 different types:
    - Client: ...
    - Admin: ...
    - Server: ...
-

...<br>

## MESSAGES

Here is a description of the messages sent from the clients to the server.<br>


### Remove

The first message a client sends is a "Remove" message, to remove itself from the serrver tables and make sure everything is clean before going further.<br>

Example:<br>
    ```
    {
        "remove":
        [
            {
                "name": "UnitySpacebrewClient",
                "remoteAddress": "127.0.0.1"
            }
        ],
        "targetType": "admin"
    }
    ```


### Config

Every client sends a "Config" message to provide required information about its configuration.<br>
The messages contain the following information:<br>

- "name": Name of the client.<br>
- "description": A short description of the client.<br>
- "publish": List of the client's publishers.<br>
    => For each publisher, specific inforamtion is provided:<br>
    - "name": Name used to connect to a subscriber from another client.<br>
    - "type": Type of its data ("boolean", "range" or "string").<br>
    - "default": The default value of its data.<br>
- "subscribe": List of the client's subscribers.<br>
    => For each subscriber, specific inforamtion is provided:<br>
    - "name": Name used to connect to a publisher from another client.<br>
    - "type": Type of its data ("boolean", "range" or "string").<br>
- "remoteAddress": The IP address of the client.<br>
- Some additional options can be provided (see the official SpaceBrew doc for details)<br>

Example:<br>
    ```
    {
        "config":
        {
            "name":"UnitySpacebrewClient",
            "description":"Basic Unity Client for Spacebrew",
            "publish":
            {
                "messages":
                [
                    {"name":"pubBool","type":"boolean","default":""},
                    {"name":"pubRange","type":"range","default":""},
                    {"name":"pubString","type":"string","default":""}
                ]
            },
            "subscribe":
            {
                "messages":
                [
                    {"name":"subBool","type":"boolean"},
                    {"name":"subRange","type":"range"},
                    {"name":"subString","type":"string"}
                ]
             },
             "remoteAddress":"127.0.0.1"
        },
        "targetType":"admin"
    }
    "options":{},
    ```

### Route

A route message can be sent to create a link between a publisher and a subscriber.<br>
It provides the descritpion for a publisher of a client and a subscriber of another client.<br>

Example:<br>
    ```
    {
        "route":
        {
            "type":"add",
            "publisher":
            {"clientName":"UnitySpacebrewClient","name":"pubString","type":"string","remoteAddress":"127.0.0.1"},
            "subscriber":
            {"clientName":"UnitySpacebrewClient2","name":"subString","type":"string","remoteAddress":"127.0.0.1"}
        },
        "targetType":"admin"
    }
    ```

### Data Message

When a link between a publisher and a subscriber is established, communication can be done by sending messages.<br>

- "name": Name of the publisher.<br>
- "type": Type of the message data.<br>
- "value": Data of the message. It must be of the specified type.<br>
- "clientName": The name of the cilient sending the message.<br>

The content of the messages is obviously specific to each application, and can be anything as long as it respects the provided type.<br>

Example:<br>
    ```
    {
        "message":
        {
            "name":"subString",
            "type":"string",
            "value":"{\"SRC\":\"client\", \"position\":{\"x\":\"0\", \"y\":\"0\", \"z\":\"0\"}, \"direction\":{\"x\":\"0\", \"y\":\"0\", \"z\":\"100\"}}",
            "clientName":"UnitySpacebrewClient"
        }
    }
    ```
