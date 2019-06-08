$(function () {

    var connection = 1;
    var patient = $("#patientId").val();
    var doctor = $("#doctorId").val();
    var conversationId = $('#conversationId').val();
    var sender = $('#sender').val();

    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.chatHub;

    //оформление чата
   // $(".footer").hide();

    $(".container").css("padding-bottom", "100").css("padding-top", "100");


    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (senderId, time, name, message) {
        // Добавление сообщений на веб-страницу 
        $('#chat')
            .append(
                '<div class=" ' +
                    ((senderId === sender) ? (' mine ') : (' yours ')) +
                    ' messages" >' +
                    '<div class="message">' +
                    htmlEncode(message) +
                    '</div>' +
                    '<p class="timeMessage">' +
                    htmlEncode(time) +
                    '</p>' +
                    '</div >'
            );
    };


    //// Добавляем нового пользователя
    //chat.client.onNewUserConnected = function (id, name) {
    //    AddUser(id, name);
    //};

    //// Удаляем пользователя
    //chat.client.onUserDisconnected = function (id, userName) {
    //    $('#' + id).remove();
    //};

    //// Функция, вызываемая при подключении нового пользователя
    //chat.client.onConnected = function (id, userName) {

    //    $('#loginBlock').hide();
    //    $('#chatBody').show();
    //    // установка в скрытых полях имени и id текущего пользователя
    //    $('#patientId').val(id);
    //    $('#username').val(userName);
    //    $('#header').html('<h3>Добро пожаловать, ' + userName + '</h3>');

    //    // Добавление всех пользователей
    //    //for (i = 0; i < allUsers.length; i++) {
    //    //    AddUser(allUsers[i].ConnectionId, allUsers[i].Name);
    //    //}
    //};

    // Открываем соединение
    $.connection.hub.start().done(function () {

        if (connection) {
            $('#chatBody').show();
            $('#inputForm').show();
            chat.server.connect(patient, doctor);
        };

        connection = 0;

        $('#sendmessage').click(function () {
            var mess = $('#message').val();
            // Вызываем у хаба метод Send
            if (mess.length !== 0) {
                chat.server.send(conversationId, sender, mess);
                $('#message').val('');
            }
        });

        //// обработка логина
        //$("#btnLogin").click(function () {

        //    var name = $("#txtUserName").val();
        //    if (name.length > 0) {
        //        chat.server.connect(name);
        //    }
        //    else {
        //     alert("Введите имя");
        //    }
    });
});
// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
////Добавление нового пользователя
//function AddUser(id, name) {

//    var userId = $('#hdId').val();

//    if (userId !== id) {

//        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
//    }
//}