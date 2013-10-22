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