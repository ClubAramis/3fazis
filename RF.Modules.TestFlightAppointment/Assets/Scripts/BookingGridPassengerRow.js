/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class BookingGridPassengerRow {

    constructor($row) {
        this.changedCallback = null;
        this.$row = $row;
        this.attach();
        this.refresh();
    }

    attach() {
        this.$role = this.$row.find('select[name="PassengerRole"]');
        this.$passenger = this.$row.find('input[name="PassengerName"]');
        this.$license = this.$row.find('input[name="PilotLicense"]');

        var that = this;
        this.$role.change(function (e) { that.onTypeChanged(); })
    }

    refresh() {
        var role = this.$role.val();
        this.$passenger.prop('disabled', !role);
        this.$license.prop('disabled', role != 'pilot');
    }

    setVisiblity(visible) {
        if (visible) {
            this.$row.show();
        } else {
            this.$row.hide();
        }
    }

    clearErrors() {
        this.$row.find('.dnnFormError').remove();
    }

    errorFor($element, message) {
        var $error = $element.next('.dnnFormError');
        if (!$error.length) {
            $error = $('<span></span>', {
                'class': 'dnnFormError'
            });
            $element.after($error);

        }

        $error.html(message);
    }

    getData() {
        return {
            role: this.$role.val(),
            name: this.$passenger.val(),
            license: this.$license.val()
        }
    }

    isEmpty() {
        var data = this.getData();
        return !data.role && !data.name && !data.license;
    }

    validate() {
        this.clearErrors();

        if (this.isEmpty())
            return true;

        var result = true;
        var data = this.getData();
        if (!data.role) {
            this.errorFor(this.$role, 'Please select passenger role.');
            result = false;
        }

        if (!data.name) {
            this.errorFor(this.$passenger, 'Please enter passenger name.');
            result = false;
        }

        if (data.role == 'pilot' && !/([a-zA-Z\d]{3})-(\d{5})-(\d{3})-(\d{4})-[sSmMlLcC]/gm.test(data.license)) {
            this.errorFor(this.$license, 'Please enter pilot license (The AAA-00000-000-0000-X formatted code).');
            result = false;

        }
        
        return result;
    }

    onTypeChanged() {
        this.refresh();

        if (this.changedCallback) {
            this.changedCallback();
        }
    }
}


