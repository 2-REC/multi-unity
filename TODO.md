# TODO

- [ ] Fix problem of spacebrew server resending messages to admins<br>
    => an admin sending a message will receive it as well ... (can ignore it, but doubles the traffic uselessly)<br>
    => Should avoid (have server as a separate app?)<br>
=> Related to previous point:<br>
- [ ] Detect if client is in same "app" as server, in which case don't send messages via SpaceBrew<br>
    => Specific communication management to "send" messages locally.<br>

- [ ] Complete/detail doc<br>

- [ ] Change "log" directory for Spacebrew<br>
- [ ] See what to do about "node_modules"<br>
    => Should be downloaded by user instead of being on Git repo.<br>

- [ ] Add some parameters handling for SpaceBrew:<br>
    => Look at the different parameters:<br>
    - "--port"<br>

    - "--ping": enable pinging of clients to track who is potentially disconnected (default)<br>
    - "--noping": opposite of --ping<br>
        => might "solve" the "setting not validated stuff?<br>

- [ ] Adapt JavaScript scripts<br>

- [ ] Try SpaceBrew server on Android, using Termux<br>
    (& see about "UnityWebRequest" for "StreamingAssets" access)<br>
