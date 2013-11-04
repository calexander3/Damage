function OpenGadgetSettingsDialog(userGadgetId) {

    var dialogWindow = $('#gadgetSettingsDialog');

    $.ajax({
        url: "/gadget/GetGadgetSettings",
        data: { userGadgetId: userGadgetId },
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        success: function (data) {
            //Create settings form
            
            //TODO: Move this into a plugin or something
            $.validator.addMethod('integer', function (value, element, param) {
                return (value > 0) && (value == parseInt(value, 10));
            }, 'Please enter a non zero, positive, integer value.');
            

            var validationRules = {};
            var settingsFormHTML = "<form id='settingsForm'><table id='settingTable'>";
            var settingsSchema = JSON.parse(data.SettingsSchema);
            for (var x = 0; x < settingsSchema.length; x++) {
                var dataType = getSettingInputType(settingsSchema[x].DataType);
                if (dataType == "radio") {
                    settingsFormHTML += "<tr><td>" + settingsSchema[x].DisplayName + "</td><td><input id='" + settingsSchema[x].FieldName + "' name='radios' type = '" + dataType + "' /></td></div>";
                }
                else {
                    settingsFormHTML += "<tr><td>" + settingsSchema[x].DisplayName + "</td><td><input id='" + settingsSchema[x].FieldName + "' name='" + settingsSchema[x].FieldName + "' type = '" + dataType + "' /></td></div>";
                    if (settingsSchema[x].Validators > 0) {
                        validationRules[settingsSchema[x].FieldName] = buildValidationRules(settingsSchema[x].Validators);
                    }
                }
            }
            dialogWindow.html(settingsFormHTML + "</table></form>");

            //Populate settings
            if (data.GadgetSettings.length > 0) {
                var settings = JSON.parse(data.GadgetSettings);
                for (var setting in settings) {
                    switch (settings[setting]) {
                        case true:
                            $("#" + setting).prop("checked", "checked");
                            break;
                        case false:
                            break;
                        default:
                            $("#" + setting).val(settings[setting]);
                            break;
                    }
                }
            }


            var form = $('#settingsForm');
            form.validate({
                rules: validationRules
            });

            dialogWindow.dialog({
                modal: true,
                width: 'auto',
                buttons: {
                    Save: function () {

                        if (form.valid()) {
                            //Gather settings
                            var newSettings = {};
                            var settingInputs = $("#settingTable").find("input");
                            for (x = 0; x < settingInputs.length; x++) {
                                if ($(settingInputs[x]).attr('type') == "checkbox" || $(settingInputs[x]).attr('type') == "radio") {
                                    newSettings[settingInputs[x].id] = $(settingInputs[x]).is(':checked');
                                }
                                else {
                                    newSettings[settingInputs[x].id] = $(settingInputs[x]).val();
                                }
                            }


                            $.ajax({
                                url: "/gadget/UpdateGadgetSettings",
                                data: JSON.stringify({ userGadgetId: userGadgetId, newSettings: JSON.stringify(newSettings) }),
                                type: 'POST',
                                contentType: 'application/json',
                                success: function () {
                                    location.reload();
                                }
                            });
                        }
                    },
                    Cancel: function () {
                        dialogWindow.dialog("close");
                    }
                },
                close: function (event, ui) { dialogWindow.html(""); }
            });
        }
    });
}

function buildValidationRules(validatorOptions) {
    var validationRules = {};
    if (validatorOptions & 1) {
        validationRules.required = true;
    }
    if (validatorOptions & 2) {
        validationRules.number = true;
    }
    if (validatorOptions & 4) {
        validationRules.url = true;
    }

    if (validatorOptions & 8) {
        validationRules.date = true;
    }
    else {
        if (validatorOptions & 16) {
            validationRules.dateISO = true;
        }
    }

    if (validatorOptions & 32) {
        validationRules.email = true;
    }
    
    if (validatorOptions & 64) {
        validationRules.integer = true;
    }

    return validationRules;
}

function getSettingInputType(type) {
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

function setupDragAndDrop() {
    $("#displayColumn1").sortable({ handle: ".GadgetHeader", forcePlaceholderSize: true, placeholder: "sortable-placeholder", connectWith: ".droppable", start: function (event, ui) { $("#garbage").css("display", "block"); }, stop: function (event, ui) { updateGadgetPositions(); $("#garbage").css("display", "none"); }, over: function (event, ui) { $(ui.placeholder).css("display", "block"); } });
    $("#displayColumn2").sortable({ handle: ".GadgetHeader", forcePlaceholderSize: true, placeholder: "sortable-placeholder", connectWith: ".droppable", start: function (event, ui) { $("#garbage").css("display", "block"); }, stop: function (event, ui) { updateGadgetPositions(); $("#garbage").css("display", "none"); }, over: function (event, ui) { $(ui.placeholder).css("display", "block"); } });
    $("#displayColumn3").sortable({ handle: ".GadgetHeader", forcePlaceholderSize: true, placeholder: "sortable-placeholder", connectWith: ".droppable", start: function (event, ui) { $("#garbage").css("display", "block"); }, stop: function (event, ui) { updateGadgetPositions(); $("#garbage").css("display", "none"); }, over: function (event, ui) { $(ui.placeholder).css("display", "block"); } });
    $("#garbage").sortable({
        over: function (event, ui) {
            $(ui.placeholder).css("display", "none");
            $(ui.item).children('div').css("background-color", "pink");
            $("#garbage").css("background-image", "url(../Content/Images/garbage-open.png)");
        },
        out: function (event, ui) {
            $("#garbage").css("background-image", "url(../Content/Images/garbage.png)");
            $(ui.item).children('div').css("background-color", "");
        },
        receive: function (event, ui) {
            $("#garbage").css("background-image", "url(../Content/Images/garbage.png)");
            $("#garbage").html("");
            $("#garbage").css("display", "none");
            $.ajax({
                url: "/gadget/DeleteGadget",
                data: JSON.stringify({ userGadgetId: $(ui.item).attr("data-usergadgetid") }),
                type: 'POST',
                contentType: 'application/json'
            });
        }
    });
}

function updateGadgetPositions() {
    var positionArray = [];
    if (!$('.sign-in-request').length) {
        for (var column = 1; column <= 3; column++) {
            var gadgets = $('#displayColumn' + column).children('.draggable');

            for (var x = 0; x < gadgets.length; x++) {
                positionArray.push({
                    UserGadgetId: $(gadgets[x]).attr('data-usergadgetid'),
                    DisplayColumn: column,
                    DisplayOrdinal: x
                });
            }
        }
    }

    $.ajax({
        url: "/gadget/UpdateGadgetPositions",
        data: JSON.stringify({ gadgetPositions: positionArray }),
        type: 'POST',
        contentType: 'application/json'
    });
}

function addNewGadget(gadgetId) {
    $.ajax({
        url: "/gadget/AddNewGadget",
        data: JSON.stringify({ gadgetId: gadgetId }),
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        success: function (data) {
            if (data) {
                dialog("Add Gadget", "Gadget has been added successfully.");
            }
            else {
                dialog("Add Gadget", "There was a problem adding your gadget.");
            }
        }
    });
}