﻿function OpenGadgetSettingsDialog(userGadgetId) {

    var dialogWindow = $('#gadgetSettingsDialog');

    $.ajax({
        url: "/gadget/GetGadgetSettings",
        data: { userGadgetId: userGadgetId },
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        success: function (data)
        {
            var settingsFormHTML = "<table>";
            var settingsSchema = JSON.parse(data.SettingsSchema);
            
            for (x = 0; x < settingsSchema.length; x++)
            {
                settingsFormHTML += "<tr><td>" + settingsSchema[x].DisplayName + "</td><td><input id=" + settingsSchema[x].FieldName + " type = '" + getSettingInputType(settingsSchema[x].DataType) + "' /></td></div>"
            }
            dialogWindow.html(settingsFormHTML + "</table>");


            var settings = JSON.parse(data.GadgetSettings);
            for (var setting in settings) {
                $("#" + setting).val(settings[setting]);
            }

            
            dialogWindow.dialog({
                modal: true,
                width: 500,
                buttons: {
                    Save: function () {
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                },
                close: function (event, ui) { dialogWindow.html(""); }
            });
        }
    });
}

function getSettingInputType(type)
{
    switch (type) {
        case 1:
            return "text";
        case 2:
            return "checkbox";
        case 3:
            return "radio";
        case 4:
            return "color";
        case 5:
            return "date";
        case 6:
            return "datetime-local";
        case 7:
            return "email";
        case 8:
            return "month";
        case 9:
            return "number";
        case 10:
            return "tel";
        case 11:
            return "time";
        case 12:
            return "url";
        case 13:
            return "week";
    }
    return "text";
}

function OpenSettingsDialog() {
    $('#settingsDialog').dialog({
        width: 525,
        modal: true,
        buttons: {
            Save: function () {
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}


function OpenAddGadgetDialog() {
    $('#addGadgetDialog').dialog({
        modal: true,
        buttons: {
            Add: function () {
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}

function setupDragAndDrop() {
    $("#displayColumn1").sortable({ handle: ".GadgetHeader", connectWith: ".GadgetColumn", stop: function (event, ui) { updateGadgetPositions(); } });
    $("#displayColumn2").sortable({ handle: ".GadgetHeader", connectWith: ".GadgetColumn", stop: function (event, ui) { updateGadgetPositions(); } });
    $("#displayColumn3").sortable({ handle: ".GadgetHeader", connectWith: ".GadgetColumn", stop: function (event, ui) { updateGadgetPositions(); } });
}

function updateGadgetPositions() {
    var positionArray = [];

    for (column = 1; column <= 3; column++) {
        var gadgets = $('#displayColumn' + column).children('.draggable');

        for (x = 0; x < gadgets.length; x++) {
            positionArray.push({
                UserGadgetId: $(gadgets[x]).attr('data-usergadgetid'),
                DisplayColumn: column,
                DisplayOrdinal: x
            });
        }
    }

    $.ajax({
        url: "/gadget/UpdateGadgetPositions",
        data: JSON.stringify({ gadgetPositions: positionArray }),
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json'
    });
}