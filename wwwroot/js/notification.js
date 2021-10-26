
"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.start();

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("span");
    li.textContent = message;
    li.className = "font - weight - light small - text mb - 0 text - muted";
    li.id = "listitem";
    document.getElementById("msg").appendChild(li);
});