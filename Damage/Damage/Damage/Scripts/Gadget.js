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

function OpenSettingsDialog()
{
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
    $("#1").sortable({ connectWith: "#2", remove: function (event, ui) { updateGadgetPositions(1, 2); } });
    $("#1").sortable({ connectWith: "#3", remove: function (event, ui) { updateGadgetPositions(1, 3); } });
    $("#2").sortable({ connectWith: "#1", remove: function (event, ui) { updateGadgetPositions(2, 1); } });
    $("#2").sortable({ connectWith: "#3", remove: function (event, ui) { updateGadgetPositions(2, 3); } });
    $("#3").sortable({ connectWith: "#1", remove: function (event, ui) { updateGadgetPositions(3, 1); } });
    $("#3").sortable({ connectWith: "#2", remove: function (event, ui) { updateGadgetPositions(3, 2); } });
}

function updateGadgetPositions(sourceColumn, destinationColumn) {
    alert("move " + sourceColumn + " to " + destinationColumn);
}