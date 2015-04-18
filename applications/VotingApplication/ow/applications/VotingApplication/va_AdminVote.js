// --------------------------------------------------------------------------------
// va_AdminVote.js
// Simon Anderson feb 1, 2013
// Contains Javascript relevant to the Votes application admin pages.
// --------------------------------------------------------------------------------


// --- Attach JS to document elements and execute simple statements. ---
Sys.Application.add_load(
function () {
	var OU = OneWeb.Util;

	var ow_dtStartDate = $app_find("ow_dtDisplayDateAndTime"),
		ow_dtEndDate = $app_find("ow_dtExpiryDateAndTime"),
        ow_btnDelete = $app_get("ow_btnDelete"),
		disableTyping = function (e) { if (e.keyCode >= 48 || e.keyCode == 13 || e.keyCode == 32) return OneWeb.Util.failEvent(e); }; // characters, spaces or linebreks

	if (ow_dtStartDate != null) OU.addEvent(ow_dtStartDate.get_dateInput().get_element(), "keydown", disableTyping);
	if (ow_dtEndDate != null) OU.addEvent(ow_dtEndDate.get_dateInput().get_element(), "keydown", disableTyping);
	if (ow_btnDelete != null) OU.addEvent(ow_btnDelete, "click", confirmDelete, false);


	var ow_txtQuestion = $app_get("ow_txtQuestion");
	if (ow_txtQuestion != null) {
		OU.addEvent(ow_txtQuestion, "keydown", ow_TextareaWithMax_KeyDown, false);
		OU.addEvent(ow_txtQuestion, "keyup", ow_TextareaWithMaxKeyUp, false);
		// run the method the first time with the entered values
		ow_TextareaWithMaxKeyUp.call(ow_txtQuestion, null);
	}


}
);

// --- Functions and variables. ---
// Provides a confirm popup when deleting a vote
function confirmDelete(e) {

    //    var cont = confirm(OneWeb.Admin.Resources.getRsrcString("confirmDelete", "ISL.OneWeb.ClientApplications.NSNU.VotingApplication.UI.Admin.Administration")); 
    var cont = confirm('Are you sure you would like to delete the vote?');
    if(cont) {
        return true;
    }
    // If cancel was clicked button execution will be halted.
    e.preventDefault();
}

// Handles onkeydown to prevent the textarea field from exceeding the maxlength
function ow_TextareaWithMax_KeyDown(e) {
	///<summary>Prevent adding more characters to the text area once the max length is reached
    if (this.value.length >= parseInt(this.getAttribute("maxlength")) && (e.keyCode >= 48 || e.keyCode == 13 || e.keyCode == 32)) // characters, spaces or linebreks
		return OneWeb.Util.failEvent(e);
 }

 function ow_TextareaWithMaxKeyUp(e) {
	///<summary>Enforces the max length value after the keyup event, for pasting, for example, as well as updating the associated character counter</summary>
	var max = parseInt(this.getAttribute("maxlength"));
	if (this.value.length > max) {
		this.value = this.value.substring(0, max);
		return OneWeb.Util.failEvent(e);
	} else {
		// update the remaining characters string if available
		var remain = OneWeb.Util.getElementsByClassName("isl_fld_tip", "p", this.parentNode);
		if (remain.length > 0)
			remain[0].innerHTML = remain[0].innerHTML.replace(/\d+/, (max - this.value.length).toString());
	}
}


function ow_TreatAsUTC(date) {
    var result = new Date(date);
    result.setMinutes(result.getMinutes() - result.getTimezoneOffset());
    return result;
}

function ow_DaysBetween(startDate, endDate) {
    var millisecondsPerDay = 24 * 60 * 60 * 1000;
    return (ow_TreatAsUTC(endDate) - ow_TreatAsUTC(startDate)) / millisecondsPerDay;
}

// --------------------------------------------------------------------------------
// checkData()
// Validates the data entered on the AddEvent and SaveEvent pages.
// --------------------------------------------------------------------------------
// Arguments:
//	- none
// Returns:
//	- the results of the validation [boolean]
// --------------------------------------------------------------------------------
if (OneWeb.Admin.Forms != null) {
    OneWeb.Admin.Forms.checkData = function (e, group) {
        var OAF = OneWeb.Admin.Forms;
        var triggerValidator = OAF.triggerValidator;

        // return if the cancel button is clicked
        if (OAF.cancelled)
            return true;

        // dismiss any current validators
        OAF.dismissValidators();

        // check addevent group
        if (typeof group == "undefined" || group == "addvote")
        {

            var ow_txtTitle = $app_get("ow_txtTitle");
            if (ow_txtTitle != null && ow_txtTitle.value.length == 0) {
                triggerValidator("ow_rfvTitle", ow_txtTitle);
            }

            var ow_txtLure = $app_get("ow_txtLure");
            if (ow_txtLure != null && ow_txtLure.value.length == 0) {
                triggerValidator("ow_rfvLure", ow_txtLure);
            }


            var ow_txtQuestion = $app_get("ow_txtQuestion");
            if (ow_txtQuestion != null) {
                if (ow_txtQuestion.value.length == 0) {
                    triggerValidator("ow_rfvQuestion", ow_txtQuestion);
                } else if (ow_txtQuestion.value.length > 4000) {
                    triggerValidator("ow_cvQuestionLength", ow_txtQuestion);
                }
            }

            var ow_txtAnswer1 = $app_get("ow_txtAnswer1");
            if (ow_txtAnswer1 != null && ow_txtAnswer1.value.length == 0) {
                triggerValidator("ow_rfvAnswer1", ow_txtAnswer1);
            }

            var ow_txtAnswer2 = $app_get("ow_txtAnswer2");
            if (ow_txtAnswer2 != null && ow_txtAnswer2.value.length == 0) {
                triggerValidator("ow_rfvAnswer2", ow_txtAnswer2);
            }


            var ow_dtDisplayDateAndTime = $app_find("ow_dtDisplayDateAndTime");
            if (ow_dtDisplayDateAndTime !== null && ow_dtDisplayDateAndTime.get_selectedDate() === null) {
                triggerValidator("ow_rfvDisplayDateAndTime", ow_dtDisplayDateAndTime.get_textBox());
            }


            var ow_dtExpiryDateAndTime = $app_find("ow_dtExpiryDateAndTime");
            if (ow_dtExpiryDateAndTime !== null && ow_dtExpiryDateAndTime.get_selectedDate() === null) {
                triggerValidator("ow_rfvExpiryDateAndTime", ow_dtExpiryDateAndTime.get_textBox());
            } else {
                var sDt = new Date(ow_dtDisplayDateAndTime.get_selectedDate());
                var eDt = new Date(ow_dtExpiryDateAndTime.get_selectedDate());
                if (eDt.getTime() == sDt.getTime()) {
                    triggerValidator("ow_DateEqual", ow_dtExpiryDateAndTime.get_textBox());
                }
                else if (eDt < sDt) {
                    triggerValidator("ow_DateCompare", ow_dtExpiryDateAndTime.get_textBox());
                }
                else if (ow_DaysBetween(sDt, eDt) > 7) {
                    triggerValidator("ow_DateDuration", ow_dtExpiryDateAndTime.get_textBox());
                }
            }
        }

        if (OAF.get_isInvalid(group)) {
            OAF.focusFirstInvalid();
            return false;
        }
        return true;
    }
}
