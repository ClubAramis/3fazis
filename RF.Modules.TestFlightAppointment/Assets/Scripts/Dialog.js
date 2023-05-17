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