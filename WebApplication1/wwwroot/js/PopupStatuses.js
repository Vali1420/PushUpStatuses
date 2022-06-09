var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();

connection.start();

connection.on("ReceiveMessageHandler", function (message) {
    $('#exampleModal').modal('show');
    document.getElementById('realTimeText').innerHTML
        = message;
});
