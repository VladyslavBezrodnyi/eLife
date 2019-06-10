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

    $(".container").css("padding-top", "50px").css("visibility", "hidden;");
    $(".col").css("background-color", "transparent");
    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (senderId, time, name, message) {
        var date = (new Date()).toLocaleString("ru", options);
        if ($('p.groupTimeDate').length < 1 ||
            date !== $('#chat-history').children('p.groupTimeDate:last').text()) {
            $('#chat-history')
                .append(
                    '<p class="groupTimeDate text-center">' +
                    date +
                    '</p > '
                );
        }
        // Добавление сообщений на веб-страницу
        if (senderId === sender) {
            $('#chat-history')
                .append(
                    '<div class="clearfix">' +
                    '<div class="message-data align-right">' +
                    '<span class="message-data-time">' +
                    htmlEncode(time) +
                    '</span >  &nbsp; &nbsp;' +
                    '<span class="message-data-name">' +
                    htmlEncode(name) +
                    '</span> <i class="fa fa-circle me"></i>' +
                    '</div>' +
                    '<div class="message other-message float-right">' +
                    htmlEncode(message) +
                    '</div>' +
                    '</div>');
        }
        else {
            $('#chat-history')
                .append(
                    '<div>' +
                    '<div class="message-data">' +
                    '<span class="message-data-name"><i class="fa fa-circle online"></i>' +
                    htmlEncode(name) +
                    '</span>' +
                    '<span class="message-data-time">' +
                    htmlEncode(time) +
                    '</span>' +
                    '</div>' +
                    '<div class="message my-message">' +
                    htmlEncode(message) +
                    '</div>' +
                    '</div>');
        }
        var objDiv = document.getElementById("chat-history");
        objDiv.scrollTop = objDiv.scrollHeight;
    };

    // Открываем соединение
    $.connection.hub.start().done(function () {

        if (connection) {
            $('#chatBody').show();
            $('#inputForm').show();
            chat.server.connect(patient, doctor);
        }

        connection = 0;

        $('#sendmessage').click(function () {
            var mess = $('#message-to-send').val();
            // Вызываем у хаба метод Send
            if (mess.length !== 0) {
                chat.server.send(conversationId, sender, mess);
                $('#message-to-send').val('');
            }
        });
    });
});
// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

var objDiv = document.getElementById("chat-history");
objDiv.scrollTop = objDiv.scrollHeight;