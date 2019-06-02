$(function () {

    $('#chatBody').hide();
    $('#loginBlock').show();
    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.chatHub;


    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (time, name, message) {
        // Добавление сообщений на веб-страницу 
        $('#chatroom').append('<p><b>' + time + '</b> <b>' + htmlEncode(name)
            + '</b>: ' + htmlEncode(message) + '</p>');
    };


    // Добавляем нового пользователя
    chat.client.onNewUserConnected = function (id, name) {

        AddUser(id, name);
    }

    // Удаляем пользователя
    chat.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();
    }

    // Функция, вызываемая при подключении нового пользователя
    chat.client.onConnected = function (id, userName) {

        $('#loginBlock').hide();
        $('#chatBody').show();
        // установка в скрытых полях имени и id текущего пользователя
        $('#patientId').val(id);
        $('#username').val(userName);
        $('#header').html('<h3>Добро пожаловать, ' + userName + '</h3>');

        // Добавление всех пользователей
        //for (i = 0; i < allUsers.length; i++) {

        //    AddUser(allUsers[i].ConnectionId, allUsers[i].Name);
       // }
    };

    // Открываем соединение
    $.connection.hub.start().done(function () {
        $("#btnLogin").click(function () {
            $('#loginBlock').hide();
            $('#chatBody').show();
            // установка в скрытых полях имени и id текущего пользователя
            var patient = $("#patientId").val();
            var doctor = $("#doctorId").val();
            $("#patient").val(patient);
            $("#doctor").val(patient);
            chat.server.connect(patient, doctor);
        });

        $('#sendmessage').click(function () {
            // Вызываем у хаба метод Send
            chat.server.send($('#conversationId').val(), $('#sender').val(), $('#message').val());
            $('#message').val('');
        });

        // обработка логина
       // $("#btnLogin").click(function () {

       //     var name = $("#txtUserName").val();
       //     if (name.length > 0) {
     //           chat.server.connect(name);
      //      }
        //    else {
       //      alert("Введите имя");
         //   }
    });
});
// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
//Добавление нового пользователя
function AddUser(id, name) {

    var userId = $('#hdId').val();

    if (userId !== id) {

        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
    }
}