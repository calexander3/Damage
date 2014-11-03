function dialog(title, body) {
    var dialogWindow = $("#dialog");
    dialogWindow.html(body);
    dialogWindow.dialog({
        title: title,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        },
        close: function () { dialogWindow.html(""); }
    });
}

function errorDialog(title, body) {
    var dialogWindow = $("#dialog");
    dialogWindow.html("<div style='float:left;margin-right:20px;'><img src='../Content/Images/Error.png' /></div><div>" + body + "</div><div style='clear:both;'></div>");
    dialogWindow.dialog({
        title: title,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        },
        close: function () { dialogWindow.html(""); }
    });
}

$(document).ready(function () {

    $('[autofocus]:not(:focus)').eq(0).focus();

    var offset = new Date();
    $("#timeZoneOffset").val(-offset.getTimezoneOffset());

    $.ajaxSetup({
        cache: false,
        traditional: true,
        type: "POST",
        error: function (request) {
            errorDialog("Error", "An error has occurred: (" + request.status + ") " + request.statusText);
        }
    });


    $("#txtSearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "https://suggestqueries.google.com/complete/search?client=chrome&q=" + encodeURIComponent($("#txtSearch").val()),
                dataType: "jsonp",
                success: function (data) {
                    response($.map(data[1], function (item) {
                        return {
                            label: item,
                            value: item
                        };
                    }));
                },
                error: function () { }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            Search(ui.item.value);
        }
    });

    $("#txtSearch").keyup(function (event) {
        if (event.keyCode == 13) {
            $("#btnSearch").click();
        }
    });

    var datesToConvert = $(".UTCDate");
    for (var i = 0; i < datesToConvert.length; i++) {
        var convertdLocalDate = new Date(datesToConvert[i].innerHTML);
        convertdLocalDate.setMinutes(convertdLocalDate.getMinutes() - convertdLocalDate.getTimezoneOffset());
        datesToConvert[i].innerHTML = convertdLocalDate.toLocaleDateString();
    }

    var timesToConvert = $(".UTCTime");
    for (var i = 0; i < timesToConvert.length; i++) {
        var convertdLocalTime = new Date(timesToConvert[i].innerHTML);
        convertdLocalTime.setMinutes(convertdLocalTime.getMinutes() - convertdLocalTime.getTimezoneOffset());
        timesToConvert[i].innerHTML = convertdLocalTime.toLocaleTimeString();
    }
});