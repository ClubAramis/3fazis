 /******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class BookingDialog {
    constructor(
        title,
        description
    ) {
        this.title = title;
        this.description = description;
        this.options = {};
    }

    option(choice, option) {
        this.options[choice] = option;

        return this;
    }

    render() {
        var that = this;
        
        var $container = $('<div></div>', {
            'class': 'tf-dialog-container dnnFormPopup'
        });
        $container.append($('<h1>' + this.title + '</h1>'));
        $container.append($('<p>' + this.description + '</p>'));
        $container.click(function(e) {
            e.stopPropagation();
        });

        for (const choice in this.options) {
            var option = this.options[choice];
            var cssClass = "dnn" + (option.type || 'Secondary') + "Action";
            var $button = $('<a href="#" class="' + cssClass + ' right tf-dialog-button">' + option.caption + '</a>');
            $button.click(function() { that.onChoiceClick(choice) });
            $container.append($button);
            $container.append('&nbsp;');
        }

        var $overlay = $('<div></div>', {
            'class': 'tf-dialog-overlay'
        });
        $overlay.append($container);
        $overlay.click(function () {
            that.onOverlayClick();
        });

        
        return $overlay;
    }

    show(callback) {
        if (this.$dialog) {
            var callbacks = this.$dialog.data('callback')
                || [];
            callbacks.push(callback);
            this.$dialog.data('callback', callbacks);
        }

        this.$dialog = this.render();
        this.$dialog.data('callback', [callback]);
        $('body').append(this.$dialog);

    }

    hide(choice) {
        var that = this;
        var callbacks = this.$dialog.data('callback');
        this.$dialog.hide('slow', function() {
            that.$dialog.remove();
            that.$dialog = null;
        })

        if (callbacks) {
            callbacks.forEach(function(item) {
                item(choice);
            });
        }
    }

    onOverlayClick() {
        this.hide('cancel');
    }

    onChoiceClick(choice) {
        this.hide(choice);
    }
}
/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class TestFlightBookingProxy {

    constructor(moduleID, serviceName) {
        this.serviceName = serviceName;
        var sf = $.ServicesFramework(moduleID);
        this.baseUrl = sf.getServiceRoot(serviceName);
    }

    invoke(method, url, data, callback) {
        $.ajax({
            url: url,
            type: method,
            data: data,
            cache: false,
            success: function(response) {
                callback(true, response);
            }
        })
            .fail(function (xhr) {
                var json = xhr.responseJSON ? xhr.responseJSON : null;
                var jsonError = json && json.error ? json.error : null;

                var message = jsonError
                    || `Request from ${url} failed with status: ${xhr.status}`;

                callback(false, message);
            });
    }

    post(url, data, callback) {
        this.invoke('POST', url, data, callback);
    }

    cancel(bookingID, callback) {
        this.post(
            this.baseUrl + 'Booking/Cancel',
            {
                BookingID: bookingID
            },
            callback
        )
    }

    create(departureAt, planID, callback) {
        this.post(
            this.baseUrl + 'Booking/Create',
            {
                DepartureAt: departureAt,
                PlanID: planID
            },
            callback
        )
    }
    
    addPassenger(bookingID, data, callback) {
        this.post(
            this.baseUrl + 'Booking/Add',
            {
                BookingID: bookingID,
                Name: data.name,
                Role: data.role,
                License: data.license
            },
            callback
        );
    }

}
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



/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class BookingGridForm {
    
    constructor (selector) {
        this.$grid = $(selector);
        this.attach();
        this.refresh();
    }

    attach() {
        var that = this;

        that.rows = [];        
        this.$grid.find('[data-role="tf-passenger-row"]')
            .each(function(idx, element) {
                var row = new BookingGridPassengerRow($(element));
                row.changedCallback = function() { that.onRowChanged(); };
                that.rows.push(row);
            });
    }

    refresh() {
        var isVisible = true;
        this.rows.forEach(function(row) {
            var hasData = !row.isEmpty();
            row.setVisiblity(isVisible || hasData);
            isVisible = !row.isEmpty();
        });
    }

    getData() {
        return this.rows
            .filter(function(row) { return !row.isEmpty(); })
            .map(function(row) { return row.getData(); });
    }

    getDataChain() {
        var data = this.getData();
        for (var i = 0; i < data.length; i++) {
            data[i].next = i < (data.length-1)
                ? data[i+1] : null;

        }

        return data;
    }

    validate() {
        var isValid = true;
        this.rows.forEach(function(row) {
            isValid = isValid && row.validate();
        });

        return isValid;
    }

    submit(proxy, data, callback) {
        var that = this;
        proxy.create(
            data.departureAt,
            data.planID,
            function (success, response) {
                if (success) {
                    that.submitPassengers(proxy, response.BookingID, callback);

                } else {
                    callback(success, response);

                }
            });
    }

    submitPassengers(proxy, bookingID, callback) {
        var data = this.getDataChain();
        if (data.length == 0) {
            callback(true, []);
            return;
        }

        var item = data[0];
        var passengers = [];
        var submitNext = function (success, response) {
            if (!success) {
                callback(false, response);
                return;
            }

            passengers.push(response);
            item = item.next;
            if (!item)
                callback(true, passengers);
            else
                proxy.addPassenger(bookingID, item, submitNext);
        }

        proxy.addPassenger(bookingID, item, submitNext);
    }

    onRowChanged() {
        this.refresh();

    }
}