# TODO

- [ ] Fix problem of spacebrew server resending messages to admins
    => an admin sending a message will receive it as well ... (can ignore it, but doubles the traffic uselessly)
    => Should avoid (have server as a separate app?)

- [ ] Complete/detail doc

- [ ] Change "log" directory for Spacebrew
- [ ] See what to do about "node_modules"
    => Should be downloaded by user instead of being on Git repo.

- [ ] Add some parameters handling for SpaceBrew:
    => Look at the different parameters:
    - "--port"

    - "--ping": enable pinging of clients to track who is potentially disconnected (default)
    - "--noping": opposite of --ping
        <br>=> might "solve" the "setting not validated stuff?

- [ ] Adapt JavaScript scripts

- [ ] Try SpaceBrew server on Android, using Termux
    (& see about "UnityWebRequest" for "StreamingAssets" access)
