// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.js-update-customer').on('click',
    (event) => {
        //debugger;
        let firstName = $('.js-first-name').val();
        let lastName = $('.js-last-name').val();
        let customerId = $('.js-customer-id').val();

        console.log(`${firstName} ${lastName}`);

        let data = JSON.stringify({
            firstName: firstName,
            lastName: lastName
        });

        // ajax call
        let result = $.ajax({
            url: `/customer/${customerId}`,
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('Update was successful');
        }).fail(failure => {
            // fail
            console.log('Update failed');
        });
    });

$('.js-customers-list tbody tr').on('click',
    (event) => {
        console.log($(event.currentTarget).attr('id'));
    });

$('.js-card-payment').on('click',
    (event) => {
        let cardnumber = $('.js-card-payment-cardnumber').val();
        let expmonth = $('.js-card-payment-expmonth').val();
        let expyear = $('.js-card-payment-expyear').val();
        let amount = $('.js-card-payment-amount').val();

        console.log(`${cardnumber} ${expmonth} ${expyear} ${amount}`);

        let data = JSON.stringify({
            cardnumber: cardnumber,
            expirationmonth: expmonth,
            expirationyear: expyear,
            amount: amount
        });

        // ajax call
        let result = $.ajax({
            url: `/card/checkout`,
            method: 'POST',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('success');
            $('.card-payment-form').hide();
            $('.alert-danger').hide();
            $('.alert-success').toggleClass('d-none');
        }).fail(failure => {
            console.log('fail');
            $('.alert-danger').toggleClass('d-none');
            $('.alert-success').hide();
        });
    });
