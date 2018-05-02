function UpdateUserDisplayInfo()
{
    $.ajax({
        url: '/User/GetUserInfo',
        success: function(data)
        {
            $('#Global_UserName').html(data.LastName + ' ' + data.FirstName);
            $('#Global_UserEmail').html(data.Email);
            $('#Global_UserId').val(data.UserId);
        }
    });
}

//System <Start>

function UpdateCurrentPageDisplayInfo(current_page) {
    $('#CurrentPage').html(current_page);
}

function PurgeSession() {
    $.ajax({
        url: '/Login/PurgeSession',
        success: function () { },
        error: function () { }
    });
}

function GetRandomValue() {
    var randValue = Math.random().toString(36).substring(2);

    return randValue;
}

function GetValueOrEmptyStr(val) {
    return val == undefined ? '' : val;
}

//System <End>


//Error Message <Start>

function ClearErrorMessages() {
    $('#ErrorMessageDiv').html('');

    $('#ErrorContainer').attr('hidden', 'hidden');
}

function GenerateAlertMessage(message_title, messages) {
    alert(message_title);
}

function GeneratePageMessage(message_title, messages) {
    var messageContent = message_title;

    if (messages != undefined && messages.length > 0) {
        messageContent += '<br />';
        messageContent += '<ul>';

        for (i = 0; i < messages.length; i++) {
            messageContent += '<li>' + messages[i] + '</li>';
        }

        messageContent += '</ul>';
    }

    $('#ErrorMessageDiv').html(messageContent);
    $('#ErrorContainer').removeAttr('hidden');
}

function GenerateMessage(message_title, messages) {
    //GenerateAlertMessage(message_title, messages);
    GeneratePageMessage(message_title, messages);
}

//Error Message <End>

//Date <Start>
function GetTodayDate() {
    var current_date = new Date();

    var current_day = new Date().getDate();
    var current_month = current_date.getMonth() + 1;
    var current_year = current_date.getFullYear();

    var str_current_day = current_day > 9 ? current_day : '0' + current_day;
    var str_current_month = current_month > 9 ? current_month : '0' + current_month;;
    var str_current_year = current_date.getFullYear();

    var str_current_date = str_current_year + '-' + str_current_month + '-' + str_current_day;

    return str_current_date;
}
//Date <End>