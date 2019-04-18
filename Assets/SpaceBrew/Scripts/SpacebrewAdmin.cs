using UnityEngine;
using System.Collections.Generic;
using static SpacebrewClient;
using System;

using ClientRoutes = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<SpacebrewAdmin.Route>>;


public class SpacebrewAdmin : MonoBehaviour {

    public class Route {
        public bool fromServer;
        public Publisher publisher;
        public Subscriber subscriber;
    }


    private SpacebrewClient sbClient;
    private Config serverConfig;
    private List<Config> clientConfigs;
    private Dictionary<string, ClientRoutes> routeTable; // key: clientAddress, value: ClientRoutes (with keys: clientName)


    private void Awake() {
        sbClient = gameObject.GetComponentInParent<SpacebrewClient>();
        if (sbClient == null) {
            throw new Exception("SpacebrewEvent: SpacebrewClient not found in parent object!");
        }

        clientConfigs = new List<Config>();
        routeTable = new Dictionary<string, ClientRoutes>();
    }

    public void SetConfig(Config config) {
        serverConfig = config;
    }

    public void Clear() {
//TODO: could rewrite/optimise
        foreach (string clientAddress in routeTable.Keys) {
            foreach (string clientName in routeTable[clientAddress].Keys) {
                RemoveRoutes(clientAddress, clientName);
            }
        }
        routeTable.Clear(); // should be useless

        clientConfigs.Clear();

//TODO: clear server config!
        //...
    }

    public void OnConfig(Config config) {
        if (serverConfig.clientName != config.clientName) {
            int index = clientConfigs.FindIndex(x => ((x.clientName == config.clientName) && (x.remoteAddress == config.remoteAddress)));
            if (index == -1) {
                clientConfigs.Add(config);
                AddRoutes(config);
            }
            else {
                print("Client '" + config.clientName + " @ " + config.remoteAddress + "' already exists!");
            }
        }
        else {
            if (serverConfig.remoteAddress == "") {
                print("Setting admin IP: " + config.remoteAddress);
                serverConfig.remoteAddress = config.remoteAddress;
            }
        }

    }

/*
    public void OnRouteRemove(string publisherClientName, Publisher publisher, string publisherAddress,
                string subscriberClientName, Subscriber subscriber, string subscriberAddress) {
        bool fromServer;
        if (routes.TryGetValue(publisherClientName, out ClientRoutes clientRoutes)) {
            fromServer = false;
        }
        else if (routes.TryGetValue(subscriberClientName, out clientRoutes)) {
            fromServer = true;
        }
        else {
            print("Unknown route: client name not found - ignoring");
            return;
        }

        if ((fromServer ? subscriberAddress : publisherAddress) != clientRoutes.address) {
            print("Unknown route: no correspoding IP address - ignoring");
            return;
        }

//        Route route = clientRoutes.routes.Find(x => ((x.fromServer == fromServer) && (x.publisher.name == publisher.name) && (x.subscriber.name == subscriber.name)));
//        clientRoutes.routes.Remove(route);
        clientRoutes.routes.RemoveAll(x => ((x.fromServer == fromServer) && (x.publisher.name == publisher.name) && (x.subscriber.name == subscriber.name)));
        if (clientRoutes.routes.Count == 0) {
            routes.Remove(fromServer ? subscriberClientName : publisherClientName);
        }
    }
*/
    public void OnRouteRemove(string publisherClientName, Publisher publisher, string publisherAddress,
                string subscriberClientName, Subscriber subscriber, string subscriberAddress) {
        // find routes for client 'clientName @ remoteAddress'
        bool fromServer;
        if ((routeTable.TryGetValue(publisherAddress, out ClientRoutes clientRoutes))
                && (clientRoutes.TryGetValue(publisherClientName, out List<Route> routes))) {
            fromServer = false;
        }
        else if ((routeTable.TryGetValue(subscriberAddress, out clientRoutes))
                && (clientRoutes.TryGetValue(subscriberClientName, out routes))) {
            fromServer = true;
        }
        else {
            string clientName = publisherClientName;
            string clientAddress = publisherAddress;
            if (serverConfig.clientName == publisherClientName){
                clientName = subscriberClientName;
                clientAddress = subscriberAddress;
            }
            print("Unknown route: client '" + clientName + " @ " + clientAddress + "' not found - ignoring");
            return;
        }

        // delete routes corresponding to 'publisher -> subscriber'
        routes.RemoveAll(x => ((x.fromServer == fromServer) && (x.publisher.name == publisher.name) && (x.subscriber.name == subscriber.name)));
        if (routes.Count == 0) {
            clientRoutes.Remove(fromServer ? subscriberClientName : publisherClientName);
            if (clientRoutes.Count == 0) {
                routeTable.Remove(fromServer ? subscriberAddress : publisherAddress);
            }
        }
    }

    public void OnRemove(string clientName, string clientAddress) {
        RemoveRoutes(clientAddress, clientName);

//        Config config = clientConfigs.Find(x => ((x.clientName == clientName) && (x.remoteAddress == clientAddress)));
//        clientConfigs.Remove(config);
        clientConfigs.RemoveAll(x => ((x.clientName == clientName) && (x.remoteAddress == clientAddress)));
    }


    private void AddRoutes(Config config) {
        // client publishers
        foreach (Publisher publisher in config.publishers) {
            if (publisher.msgType != MessageType.DEFAULT) {
                foreach (Subscriber serverSubscriber in serverConfig.subscribers) {
                    if (serverSubscriber.msgType == publisher.msgType) {
                        AddRoute(false, config.clientName, publisher, config.remoteAddress, serverConfig.clientName, serverSubscriber, serverConfig.remoteAddress);
                    }
                }
            }
        }

        // client subscribers
        foreach (Subscriber subscriber in config.subscribers) {
            if (subscriber.msgType != MessageType.DEFAULT) {
                foreach (Publisher serverPublisher in serverConfig.publishers) {
                    if (serverPublisher.msgType == subscriber.msgType) {
                        AddRoute(true, serverConfig.clientName, serverPublisher, serverConfig.remoteAddress, config.clientName, subscriber, config.remoteAddress);
                    }
                }
            }
        }
    }

    private void AddRoute(bool fromServer, string publisherClientName, Publisher publisher, string publisherAddress,
            string subscriberClientName, Subscriber subscriber, string subscriberAddress) {
//TODO: should instead be called when receive a "route-add" message!
//=> could have a "pending created routes", waiting for server message to save locally
// (then this function would be useless, only containing "sbClient.SendRouteMessage" - similar to "RemoveRoutes")
        SaveRoute(fromServer, publisherClientName, publisher, publisherAddress, subscriberClientName, subscriber, subscriberAddress);

        // send "add route" message to server
        sbClient.SendRouteMessage(true, publisherClientName, publisher, publisherAddress, subscriberClientName, subscriber, subscriberAddress);
    }

/*
    private void RemoveRoutes(string clientName) {
        if (routes.TryGetValue(clientName, out ClientRoutes clientRoutes)) {
            foreach (Route route in clientRoutes.routes) {
                if (route.fromServer) {
                    sbClient.SendRouteMessage(false, serverConfig.clientName, route.publisher, serverConfig.remoteAddress, clientName, route.subscriber, clientRoutes.address);
                }
                else {
                    sbClient.SendRouteMessage(false, clientName, route.publisher, clientRoutes.address, serverConfig.clientName, route.subscriber, serverConfig.remoteAddress);
                }
            }

//TODO: should instead have "DeleteRoute" (to remove a single route, called when receive a "route-remove" message!), and check when empty for "clientName" before removing it.
//=> could have a "pending deleted routes", waiting for server message to delete locally
            routes.Remove(clientName);
        }
    }
*/
    private void RemoveRoutes(string clientAddress, string clientName) {
        if (routeTable.TryGetValue(clientAddress, out ClientRoutes clientRoutes)) {
            if (clientRoutes.TryGetValue(clientName, out List<Route> routes)) {
                foreach (Route route in routes) {
                    if (route.fromServer) {
                        sbClient.SendRouteMessage(false, serverConfig.clientName, route.publisher, serverConfig.remoteAddress, clientName, route.subscriber, clientAddress);
                    }
                    else {
                        sbClient.SendRouteMessage(false, clientName, route.publisher, clientAddress, serverConfig.clientName, route.subscriber, serverConfig.remoteAddress);
                    }
                }

//TODO: should instead have "DeleteRoute" (to remove a single route, called when receive a "route-remove" message!), and check when empty for "clientName" before removing it.
//=> could have a "pending deleted routes", waiting for server message to delete locally
                clientRoutes.Remove(clientName);
                if (clientRoutes.Count == 0) {
                    routeTable.Remove(clientAddress);
                }
            }
        }
    }

/*
//TODO: should be called by "OnRoute" (when receiving "route-remove" message)
    private void SaveRoute(bool fromServer, string publisherClientName, Publisher publisher, string publisherAddress,
            string subscriberClientName, Subscriber subscriber, string subscriberAddress) {
        string clientName = fromServer ? subscriberClientName : publisherClientName;
        string clientAddress = fromServer ? subscriberAddress : publisherAddress;

        if (!routes.TryGetValue(clientName, out ClientRoutes clientRoutes)) {
            clientRoutes = new ClientRoutes();
            clientRoutes.address = clientAddress;
            routes.Add(clientName, clientRoutes);
        }
        else {
            if (clientRoutes.address != clientAddress) {
                print("ERROR: different address for client!");
//TODO: OK? or should throw exception?
                return;
            }
        }

//TODO: check that ok with fields names
        var route = new Route {
            fromServer = fromServer,
            publisher = publisher,
            subscriber = subscriber,
        };
        clientRoutes.routes.Add(route);
    }
*/
//TODO: should be called by "OnRoute" (when receiving "route-remove" message)
    private void SaveRoute(bool fromServer, string publisherClientName, Publisher publisher, string publisherAddress,
            string subscriberClientName, Subscriber subscriber, string subscriberAddress) {
        string clientName = fromServer ? subscriberClientName : publisherClientName;
        string clientAddress = fromServer ? subscriberAddress : publisherAddress;

        if (!routeTable.TryGetValue(clientAddress, out ClientRoutes clientRoutes)) {
            clientRoutes = new ClientRoutes();
            routeTable.Add(clientAddress, clientRoutes);
        }

        if (!clientRoutes.TryGetValue(clientName, out List<Route> routes)) {
            routes = new List<Route>();
            clientRoutes.Add(clientName, routes);
        }

//TODO: check that ok with fields names
        var route = new Route {
            fromServer = fromServer,
            publisher = publisher,
            subscriber = subscriber,
        };
        routes.Add(route);
    }

}
