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