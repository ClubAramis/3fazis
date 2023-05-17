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