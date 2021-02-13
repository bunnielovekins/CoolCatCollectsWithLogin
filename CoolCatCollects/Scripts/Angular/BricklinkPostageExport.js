var postageExportApp = angular.module('postageExportApp', []);

postageExportApp.controller('PostageExportController', ['$scope', '$http', function ($scope, $http) {
	var init = () => {
		var overrideStage = $('#overrideStage').val();

		if (overrideStage) {
			overrideStage = '?overrideStage=' + overrideStage;
		}

		$http.get('/BricklinkPostage/GetOrders' + overrideStage).then((res) => {
			$scope.Orders = res.data;
		});
	};

	init();

	$scope.check = (obj, prop) => {
		obj[prop] = !obj[prop];
	}

	$scope.SizeOptions = [
		{ Text: "Large Letter", Value: "large letter" },
		{ Text: "Parcel", Value: "parcel" }
	];

	$scope.ShippingMethodsDomestic = [
		{ Text: "RM 48", Value: "CRL48" },
		{ Text: "RM 24", Value: "CRL24" },
		{ Text: "RM 2nd Signed For", Value: "BPR2" },
		{ Text: "RM 1st Signed For", Value: "BPR1" },
		{ Text: "RM 1st", Value: "BPL1" },
		{ Text: "RM 2nd", Value: "BPL2" },
		{ Text: "Special Delivery Guaranteed by 1pm - £750", Value: "SD1" },
		{ Text: "Special Delivery Guaranteed by 9am - £750", Value: "SD4" },
		{ Text: "Tracked 24", Value: "TPN24" },
		{ Text: "Tracked 48", Value: "TPS48" },
		{ Text: "----- International -----", Value: null, Disabled: true },
		{ Text: "Intl Standard", Value: "OLA" },
		{ Text: "Intl Economy", Value: "OLS" },
		{ Text: "Intl Tracked £50 comp", Value: "OTA" },
		{ Text: "Intl Tracked £250 comp", Value: "OTB" },
		{ Text: "Intl Tracked & Signed £50 comp", Value: "OTC" },
		{ Text: "Intl Tracked & Signed £250 comp", Value: "OTD" }
	];

	$scope.ShippingMethodsInternational = [
		{ Text: "Intl Standard", Value: "OLA" },
		{ Text: "Intl Economy", Value: "OLS" },
		{ Text: "Intl Tracked £50 comp", Value: "OTA" },
		{ Text: "Intl Tracked £250 comp", Value: "OTB" },
		{ Text: "Intl Tracked & Signed £50 comp", Value: "OTC" },
		{ Text: "Intl Tracked & Signed £250 comp", Value: "OTD" },
		{ Text: "----- Domestic -----", Value: null, Disabled: true },
		{ Text: "RM 48", Value: "CRL48" },
		{ Text: "RM 24", Value: "CRL24" },
		{ Text: "RM 2nd Signed For", Value: "BPR2" },
		{ Text: "RM 1st Signed For", Value: "BPR1" },
		{ Text: "RM 1st", Value: "BPL1" },
		{ Text: "RM 2nd", Value: "BPL2" },
		{ Text: "Special Delivery Guaranteed by 1pm - £750", Value: "SD1" },
		{ Text: "Special Delivery Guaranteed by 9am - £750", Value: "SD4" },
		{ Text: "Tracked 24", Value: "TPN24" },
		{ Text: "Tracked 48", Value: "TPS48" }
	];

	$scope.GetShippingMethods = (order) => {
		return order.InternationalOrder ?
			$scope.ShippingMethodsInternational :
			$scope.ShippingMethodsDomestic;
	}

	$scope.SubmitForm = () => {
		var form = $('.js-form');

		form.submit();
	}

	$scope.OrderProperty = 'OrderId';
	$scope.OrderReverse = false;

	$scope.Order = (property) => {
		if (property === $scope.OrderProperty) {
			$scope.OrderReverse = !$scope.OrderReverse;
		}
		else {
			$scope.OrderProperty = property;
			$scope.OrderReverse = false;
		}
	}
}]);
