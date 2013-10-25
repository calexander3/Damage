function OpenGadgetSettingsDialog() {
    $('#gadgetSettingsDialog').dialog({
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
    $("#1").sortable({ handle: ".GadgetHeader", connectWith: ".GadgetColumn", stop: function (event, ui) { updateGadgetPositions(); } });
    $("#2").sortable({ handle: ".GadgetHeader", connectWith: ".GadgetColumn", stop: function (event, ui) { updateGadgetPositions(); } });
    $("#3").sortable({ handle: ".GadgetHeader", connectWith: ".GadgetColumn", stop: function (event, ui) { updateGadgetPositions(); } });
}

function updateGadgetPositions() {
    var positionArray = [];

    for (column = 1; column <= 3; column++) {
        var gadgets = $('#' + column).children('.draggable');

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
        dataType: 'json',
    });

}