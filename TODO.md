# TODO

## BUG FIXES / TESTS
- [ ] Fix problem of Spacebrew server resending messages to admins<br>
    - An admin sending a message will receive it as well ...
        <br>(can ignore it, but doubles the traffic uselessly) => Should avoid (have server as a separate app?)
    <br>=> Check if still need to call "makeConfig" for server only object


- [ ] ADD: Detect if client is in same "app" as server, in which case don't send messages via Spacebrew
    <br>(Related to previous point)
    <br>=> Specific communication management to "send" messages locally.<br>


- [ ] Try Spacebrew server on Android, using Termux
    <br>(& see about "UnityWebRequest" for "StreamingAssets" access)

- [ ] Found how logging works for server (JS app)
    <br>=> Where is it logging?
    - Related to "fs.stat.." ? (directory not found though exist)
        <br>=> look at code in:
        - spacebrew.js
        - logger.js
    - Add "> TEST.txt" at end of node server command to log in text file
    ```
    node_server_forever.js > TEST.txt
    ```


## DOCUMENTATION
- [ ] Complete/detail doc<br>


## SETUP / CONFIG
- [ ] Add some parameters handling for Spacebrew:<br>
    => Look at the different parameters:<br>
    - "--port"
    - "--ping": enable pinging of clients to track who is potentially disconnected (default)
    - "--noping": opposite of --ping
        <br>=> might "solve" the "setting not validated stuff?<br>


## BUILD / FILES
- [ ] Spacebrew "data" directory
    - [ ] Change "routes" directory location
        <br>=> not created when starting script with "--nopersist"
    - [ ] Change "log" directory location
        <br>=> Redirect logs (or remove them)
        - Node.js
        - Spacebrew


- [ ] See about "PostprocessBuildPlayer" script, how can do similar in Windows (& Android?)
    <br>=> if needed (related to following points about building)


- [ ] node.js executables
    <br>=> See for other OSes
    - Android
        <br>=> Look at Termux
    - iOS
    - Linux


- [ ] See what to do about "node_modules"
    <br>=> Should be downloaded by user instead of being on Git repo.
    <br>=> Auto download - Requires npm? ("npm install")
