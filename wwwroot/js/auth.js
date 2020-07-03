$('#signIn').on('submit', function (e) {
	e.preventDefault();
	var userName = $('input[name=userName').val();
	var password = md5($('input[name=password').val());
	var merge = true;
	if (localStorage.getItem('cart') !== null) {
		var cart = JSON.parse(localStorage.getItem('cart'));
		var cartDetails = cart.cartDetails;
		if (cartDetails.length > 0) {
			merge = confirm('Do you want to merge cart into your account?');
		}
		cartDetails = JSON.stringify(cartDetails);
	}

	var postBody = '';
	if (merge) postBody = cartDetails;

	$.ajax({
		type: 'POST',
		dataType: 'text',
		contentType: 'application/json; charset=utf-8',
		url: '/Auth/Login?' + 'userName=' + userName + '&password=' + password,
		data: postBody,
		success: function (res) {
			if (res === '/Auth/Index') {
				bootbox.alert('Invalid username or password!', function () {
					window.location.href = res;
				});
			} else {
				localStorage.removeItem('cart');
				window.location.href = res;
			}
		},
	});
});
