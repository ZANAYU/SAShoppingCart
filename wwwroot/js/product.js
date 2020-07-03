$(document).ready(function () {
	var user = $.cookie('User');
	if (user == null) {
		if (localStorage.getItem('cart') !== null) {
			let cart = JSON.parse(localStorage.getItem('cart'));
			let totalQty = cart.cartDetails.reduce(function (prev, cur) {
				return prev + cur.qty;
			}, 0);
			$('#totalQty')
				.empty()
				.append('<i class="fas fa-shopping-cart"></i>', ' ' + totalQty);
		} else {
			$('#totalQty')
				.empty()
				.append('<i class="fas fa-shopping-cart"></i>', ' ' + 0);
		}
	}
});

$('.addToCart').click(function (e) {
	var user = $.cookie('User');
	if (user == null) {
		let productId = $(this).attr('value');
		let product = {
			name: $(this).closest('.card').find('.card-title').text(),
			price: $(this).data('price'),
			description: $(this).closest('.card').find('.card-text').text(),
			image: $(this).closest('.card').find('.card-img-top').attr('src'),
		};

		if (localStorage.getItem('cart') === null) {
			let cart = {
				cartDetails: [
					{
						qty: 1,
						productId: productId,
						product: product,
					},
				],
			};
			localStorage.setItem('cart', JSON.stringify(cart));
			let totalQty = cart.cartDetails.reduce(function (prev, cur) {
				return prev + cur.qty;
			}, 0);
			$('#totalQty')
				.empty()
				.append('<i class="fas fa-shopping-cart"></i>', ' ' + totalQty);
		} else {
			let cart = JSON.parse(localStorage.getItem('cart'));
			let isNewItem = true;
			if (cart.cartDetails.length > 0) {
				cart.cartDetails.forEach((cd) => {
					if (cd.productId === productId) {
						cd.qty++;
						isNewItem = false;
					}
				});
			}
			if (isNewItem) {
				cart.cartDetails.push({
					qty: 1,
					productId: productId,
					product: product,
				});
			}
			localStorage.setItem('cart', JSON.stringify(cart));
			let totalQty = cart.cartDetails.reduce(function (prev, cur) {
				return prev + cur.qty;
			}, 0);
			$('#totalQty')
				.empty()
				.append('<i class="fas fa-shopping-cart"></i>', ' ' + totalQty);
		}
	} else {
		var productId = $(this).attr('value');
		e.preventDefault();
		$.ajax({
			type: 'POST',
			contentType: 'application/json',
			url: '/Cart/AddToCart?' + 'ProductId=' + productId,
			success: function (res) {
				$('#totalQty')
					.empty()
					.append('<i class="fas fa-shopping-cart"></i>', ' ' + res);
			},
		});
	}
});
